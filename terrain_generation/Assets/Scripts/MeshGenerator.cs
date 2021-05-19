using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    private Mesh mesh;

    private Vector3[] vertices;
    private int[] triangles;
    private Color[] colors;

    public Gradient gradient;

    public int xSize = 20;
    public int zSize = 20;

    [Range(0, 1f)] public float scale = 0.1f;
    [Range(-5f, 5f)] public float height = 2f;
    [Range(1, 10)] public int quality = 1;

    private float terrainMovingCoef = 0;

    public int M = 10; // horizontal
    public int N = 10; // vertical
    public float radius = 1f;
    
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        
        // CreateShape();
        // UpdateMesh();
    }

    private void Update()
    {
        CreateShape();
        // UpdateMesh();
        //
        // terrainMovingCoef += 0.01f;
    }

    void CreateShape()
    {
        // vertices = new Vector3[(xSize * quality + 1) * (zSize * quality + 1)];
        //
        // for (int z = 0, i = 0; z <= zSize * quality; z++)
        // {
        //     for (int x = 0; x <= xSize * quality; x++, i++)
        //     {
        //         float y = Mathf.PerlinNoise(x * scale / quality + terrainMovingCoef * 0, z * scale / quality + terrainMovingCoef) * height;
        //         
        //         // if (y - (int) y < 0.5f) y = (int) y; // муравейник
        //         // else y = (int) y + 0.5f;
        //         
        //         vertices[i] = new Vector3((float) x / quality, y, (float) z / quality);
        //     }
        // }

        // triangles = new int[xSize * zSize * 6 * quality * quality];
        //
        // for (int z = 0, vert = 0, tris = 0; z < zSize * quality; z++, vert++)
        // {
        //     for (int x = 0; x < xSize * quality; x++, vert++, tris += 6)
        //     {
        //         triangles[tris + 0] = vert + 0;
        //         triangles[tris + 1] = vert + xSize * quality + 1;
        //         triangles[tris + 2] = vert + 1;
        //         triangles[tris + 3] = vert + 1;
        //         triangles[tris + 4] = vert + xSize * quality + 1;
        //         triangles[tris + 5] = vert + xSize * quality + 2;
        //     }
        // }

        vertices = new Vector3[(M + 1) * (N + 1)];
        for (int m = 0, i = 0; m <= M; m++)
        {
            for (int n = 0; n <= N; n++, i++)
            {
                float x = Mathf.Sin(Mathf.PI * m / M) * Mathf.Cos(2 * Mathf.PI * n / N) * radius;
                float y = Mathf.Sin(Mathf.PI * m / M) * Mathf.Sin(2 * Mathf.PI * n / N) * radius;
                float z = Mathf.Cos(Mathf.PI * m / M) * radius;
                
                // x += Mathf.PerlinNoise(y * scale + terrainMovingCoef, z * scale + terrainMovingCoef);
                // y += Mathf.PerlinNoise(x * scale + terrainMovingCoef, z * scale + terrainMovingCoef);
                // z += Mathf.PerlinNoise(x * scale + terrainMovingCoef, y * scale + terrainMovingCoef);

                vertices[i] = new Vector3(x, y, z);
            }
        }

        triangles = new int[M * N * 6];
        for (int m = 0, vert = 0, tris = 0; m < M; m++, vert++)
        {
            for (int n = 0; n < N; n++, vert++, tris += 6)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + N + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + N + 1;
                triangles[tris + 5] = vert + N + 2;
            }
        }

        // colors = new Color[vertices.Length];
        //
        // for (int z = 0, i = 0; z <= zSize * quality; z++)
        // {
        //     for (int x = 0; x <= xSize * quality; x++, i++)
        //     {
        //         // uvs[i] = new Vector2((float) x / xSize / quality, (float) z / zSize / quality);
        //         float height = vertices[i].y;
        //         colors[i] = gradient.Evaluate(height);
        //     }
        // }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        // mesh.uv = uvs;
        // mesh.colors = colors;
        
        mesh.RecalculateNormals();
    }
    
    private void OnDrawGizmos()
    {
        if (vertices == null) return;
    
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}
