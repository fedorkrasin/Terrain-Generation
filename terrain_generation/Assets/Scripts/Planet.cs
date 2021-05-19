﻿// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class Planet : MonoBehaviour
// {
//     private GameObject planet;
//     
//     public List<Polygon> polygons;
//     public List<Vector3> vertices;
//     public Material material;
//
//     void Start()
//     {
//         CreateIcosahedron();
//         Subdivide(2);
//         GenerateMesh();
//     }
//     
//     public void CreateIcosahedron()
//     {
//         polygons = new List<Polygon>();
//         vertices = new List<Vector3>();
//         
//         float t = (1.0f + Mathf.Sqrt (5.0f)) / 2.0f;
//
//         // Vertices
//         vertices.Add(new Vector3(-1, t, 0).normalized);
//         vertices.Add(new Vector3(1, t, 0).normalized);
//         vertices.Add(new Vector3(-1, -t, 0).normalized);
//         vertices.Add(new Vector3(1, -t, 0).normalized);
//         vertices.Add(new Vector3(0, -1, t).normalized);
//         vertices.Add(new Vector3(0, 1, t).normalized);
//         vertices.Add(new Vector3(0, -1, -t).normalized);
//         vertices.Add(new Vector3(0, 1, -t).normalized);
//         vertices.Add(new Vector3(t, 0, -1).normalized);
//         vertices.Add(new Vector3(t, 0, 1).normalized);
//         vertices.Add(new Vector3(-t, 0, -1).normalized);
//         vertices.Add(new Vector3(-t, 0, 1).normalized);
//         
//         // Polygons
//         polygons.Add(new Polygon(0, 11, 5));
//         polygons.Add(new Polygon(0, 5, 1));
//         polygons.Add(new Polygon(0, 1, 7));
//         polygons.Add(new Polygon(0, 7, 10));
//         polygons.Add(new Polygon(0, 10, 11));
//         polygons.Add(new Polygon(1, 5, 9));
//         polygons.Add(new Polygon(5, 11, 4));
//         polygons.Add(new Polygon(11, 10, 2));
//         polygons.Add(new Polygon(10, 7, 6));
//         polygons.Add(new Polygon(7, 1, 8));
//         polygons.Add(new Polygon(3, 9, 4));
//         polygons.Add(new Polygon(3, 4, 2));
//         polygons.Add(new Polygon(3, 2, 6));
//         polygons.Add(new Polygon(3, 6, 8));
//         polygons.Add(new Polygon(3, 8, 9));
//         polygons.Add(new Polygon(4, 9, 5));
//         polygons.Add(new Polygon(2, 4, 11));
//         polygons.Add(new Polygon(6, 2, 10));
//         polygons.Add(new Polygon(8, 6, 7));
//         polygons.Add(new Polygon(9, 8, 1));
//     }
//
//     public void Subdivide(int subdivisions)
//     {
//         var midPointCache = new Dictionary<int, int> ();
//
//         for (int i = 0; i < subdivisions; i++)
//         {
//             var newPolygons = new List<Polygon>();
//
//             foreach (var polygon in polygons)
//             {
//                 int a = polygon.vertices[0];
//                 int b = polygon.vertices[1];
//                 int c = polygon.vertices[2];
//
//                 int ab = GetMidPointIndex(midPointCache, a, b);
//                 int bc = GetMidPointIndex(midPointCache, b, c);
//                 int ca = GetMidPointIndex(midPointCache, c, a);
//
//                 newPolygons.Add(new Polygon(a, ab, ca));
//                 newPolygons.Add(new Polygon(b, bc, ab));
//                 newPolygons.Add(new Polygon(c, ca, bc));
//                 newPolygons.Add(new Polygon(ab, bc, ca));
//             }
//
//             polygons = newPolygons;
//         }
//     }
//
//     public int GetMidPointIndex(Dictionary<int, int> cache, int a, int b)
//     {
//         int smallerIndex = Mathf.Min(a, b);
//         int greaterIndex = Mathf.Max(a, b);
//         int key = (smallerIndex << 16) + greaterIndex;
//         
//         int ret;
//         if (cache.TryGetValue (key, out ret))
//             return ret;
//
//         Vector3 p1 = vertices[a];
//         Vector3 p2 = vertices[b];
//         Vector3 middle = Vector3.Lerp (p1, p2, 0.5f).normalized;
//
//         ret = vertices.Count;
//         vertices.Add(middle);
//         cache.Add(key, ret);
//
//         return ret;
//     }
//
//     public void GenerateMesh()
//     {
//         if (planet) Destroy(planet);
//
//         planet = new GameObject("Planet Mesh");
//         MeshRenderer surfaceRenderer = planet.AddComponent<MeshRenderer>();
//         surfaceRenderer.material = material;
//
//         Mesh terrainMesh = new Mesh();
//
//         int vertexCount = polygons.Count * 3;
//         
//         int[] indices = new int[vertexCount];
//         
//         Vector3[] vertices = new Vector3[vertexCount];
//         Vector3[] normals  = new Vector3[vertexCount];
//         Color32[] colors   = new Color32[vertexCount];
//         
//         Color32 green = new Color32(20, 255, 30, 255);
//         Color32 brown = new Color32(220, 150, 70, 255);
//         
//         for (int i = 0; i < polygons.Count; i++)
//         {
//             var poly = polygons[i];
//             indices[i * 3 + 0] = i * 3 + 0;
//             indices[i * 3 + 1] = i * 3 + 1;
//             indices[i * 3 + 2] = i * 3 + 2;
//             vertices[i * 3 + 0] = vertices[poly.vertices[0]];
//             vertices[i * 3 + 1] = vertices[poly.vertices[1]];
//             vertices[i * 3 + 2] = vertices[poly.vertices[2]];
//             
//             Color32 polyColor = Color32.Lerp(green, brown, Random.Range(0.0f, 1.0f));
//             colors[i * 3 + 0] = polyColor;
//             colors[i * 3 + 1] = polyColor;
//             colors[i * 3 + 2] = polyColor;
//             
//             normals[i * 3 + 0] = vertices[poly.vertices[0]];
//             normals[i * 3 + 1] = vertices[poly.vertices[1]];
//             normals[i * 3 + 2] = vertices[poly.vertices[2]];
//         }
//         
//         terrainMesh.vertices = vertices;
//         terrainMesh.normals = normals;
//         terrainMesh.colors32 = colors;
//         terrainMesh.SetTriangles(indices, 0);
//         MeshFilter terrainFilter = planet.AddComponent<MeshFilter>();
//         terrainFilter.mesh = terrainMesh;
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    private GameObject m_Planet;

    List<Polygon> m_Polygons;
    List<Vector3> m_Vertices;
    public Material m_Material;

    public void Start()
    {
        InitAsIcosohedron();
        Subdivide(0);
        GenerateMesh();
    }

    public void InitAsIcosohedron()
    {
        m_Polygons = new List<Polygon>();
        m_Vertices = new List<Vector3>();

        float t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;
        
        // Vertices
        m_Vertices.Add(new Vector3(-1,  t,  0).normalized);
        m_Vertices.Add(new Vector3( 1,  t,  0).normalized);
        m_Vertices.Add(new Vector3(-1, -t,  0).normalized);
        m_Vertices.Add(new Vector3( 1, -t,  0).normalized);
        m_Vertices.Add(new Vector3( 0, -1,  t).normalized);
        m_Vertices.Add(new Vector3( 0,  1,  t).normalized);
        m_Vertices.Add(new Vector3( 0, -1, -t).normalized);
        m_Vertices.Add(new Vector3( 0,  1, -t).normalized);
        m_Vertices.Add(new Vector3( t,  0, -1).normalized);
        m_Vertices.Add(new Vector3( t,  0,  1).normalized);
        m_Vertices.Add(new Vector3(-t,  0, -1).normalized);
        m_Vertices.Add(new Vector3(-t,  0,  1).normalized);

        // Polygons
        m_Polygons.Add(new Polygon( 0, 11,  5));
        m_Polygons.Add(new Polygon( 0,  5,  1));
        m_Polygons.Add(new Polygon( 0,  1,  7));
        m_Polygons.Add(new Polygon( 0,  7, 10));
        m_Polygons.Add(new Polygon( 0, 10, 11));
        m_Polygons.Add(new Polygon( 1,  5,  9));
        m_Polygons.Add(new Polygon( 5, 11,  4));
        m_Polygons.Add(new Polygon(11, 10,  2));
        m_Polygons.Add(new Polygon(10,  7,  6));
        m_Polygons.Add(new Polygon( 7,  1,  8));
        m_Polygons.Add(new Polygon( 3,  9,  4));
        m_Polygons.Add(new Polygon( 3,  4,  2));
        m_Polygons.Add(new Polygon( 3,  2,  6));
        m_Polygons.Add(new Polygon( 3,  6,  8));
        m_Polygons.Add(new Polygon( 3,  8,  9));
        m_Polygons.Add(new Polygon( 4,  9,  5));
        m_Polygons.Add(new Polygon( 2,  4, 11));
        m_Polygons.Add(new Polygon( 6,  2, 10));
        m_Polygons.Add(new Polygon( 8,  6,  7));
        m_Polygons.Add(new Polygon( 9,  8,  1));
    }

    public void Subdivide(int recursions)
    {
        var midPointCache = new Dictionary<int, int>();

        for (int i = 0; i < recursions; i++)
        {
            var newPolys = new List<Polygon>();
            foreach (var poly in m_Polygons)
            {
                int a = poly.m_Vertices[0];
                int b = poly.m_Vertices[1];
                int c = poly.m_Vertices[2];

                int ab = GetMidPointIndex(midPointCache, a, b);
                int bc = GetMidPointIndex(midPointCache, b, c);
                int ca = GetMidPointIndex(midPointCache, c, a);

                newPolys.Add(new Polygon(a, ab, ca));
                newPolys.Add(new Polygon(b, bc, ab));
                newPolys.Add(new Polygon(c, ca, bc));
                newPolys.Add(new Polygon(ab, bc, ca));
            }
            
            m_Polygons = newPolys;
        }
    }
    public int GetMidPointIndex(Dictionary<int, int> cache, int indexA, int indexB)
    {
        int smallerIndex = Mathf.Min(indexA, indexB);
        int greaterIndex = Mathf.Max(indexA, indexB);
        int key = (smallerIndex << 16) + greaterIndex;

        int ret;
        if (cache.TryGetValue(key, out ret)) return ret;

        Vector3 p1 = m_Vertices[indexA];
        Vector3 p2 = m_Vertices[indexB];
        Vector3 middle = Vector3.Lerp(p1, p2, 0.5f).normalized;

        ret = m_Vertices.Count;
        m_Vertices.Add(middle);

        cache.Add(key, ret);
        return ret;
    }

    public void GenerateMesh()
    {
        if (m_Planet)
            Destroy(m_Planet);

        m_Planet = new GameObject("Planet Mesh");

        MeshRenderer surfaceRenderer = m_Planet.AddComponent<MeshRenderer>();
        surfaceRenderer.material = m_Material;

        Mesh terrainMesh = new Mesh();

        int vertexCount = m_Polygons.Count * 3;

        int[] indices = new int[vertexCount];

        Vector3[] vertices = new Vector3[vertexCount];
        Vector3[] normals  = new Vector3[vertexCount];
        Color32[] colors   = new Color32[vertexCount];

        Color32 green = new Color32(255, 255, 255, 255);
        Color32 brown = new Color32(0, 0, 0, 255);

        for (int i = 0; i < m_Polygons.Count; i++)
        {
            var poly = m_Polygons[i];

            indices[i * 3 + 0] = i * 3 + 0;
            indices[i * 3 + 1] = i * 3 + 1;
            indices[i * 3 + 2] = i * 3 + 2;

            vertices[i * 3 + 0] = m_Vertices[poly.m_Vertices[0]];
            vertices[i * 3 + 1] = m_Vertices[poly.m_Vertices[1]];
            vertices[i * 3 + 2] = m_Vertices[poly.m_Vertices[2]];

            Color32 polyColor = Color32.Lerp(green, brown, Random.Range(0.0f, 1.0f)); 

            colors[i * 3 + 0] = polyColor;
            colors[i * 3 + 1] = polyColor;
            colors[i * 3 + 2] = polyColor;

            normals[i * 3 + 0] = m_Vertices[poly.m_Vertices[0]];
            normals[i * 3 + 1] = m_Vertices[poly.m_Vertices[1]];
            normals[i * 3 + 2] = m_Vertices[poly.m_Vertices[2]];
        }

        terrainMesh.vertices = vertices;
        terrainMesh.normals  = normals;
        terrainMesh.colors32 = colors;

        terrainMesh.SetTriangles(indices, 0);

        MeshFilter terrainFilter = m_Planet.AddComponent<MeshFilter>();
        terrainFilter.mesh = terrainMesh;
    }
}