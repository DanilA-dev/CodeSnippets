using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class DiggableMeshGenerator : MonoBehaviour
{
    [SerializeField, Min(0.01f)] private float _size = 1;
    [SerializeField,Min(1)] private int _gridSize = 16;

    private MeshFilter _meshFilter;
    private MeshVertexProbesController _vertexController;
    private MeshCollider _meshCollider;
	
    [Button]
    private void CreatePlane()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshCollider =  GetComponent<MeshCollider>();
        Mesh mesh = new Mesh();

        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();
        for (int x = 0; x < _gridSize + 1; ++x)
        {
            for (int y = 0; y < _gridSize + 1; ++y)
            {
                vertices.Add(new Vector3(-_size * 0.5f + _size * (x / ((float)_gridSize)), 0, -_size * 0.5f + _size * (y / ((float)_gridSize))));
                normals.Add(Vector3.up);
                uvs.Add(new Vector2(x / (float)_gridSize, y / (float)_gridSize));
            }
        }

        var triangles = new List<int>();
        int vertCount = _gridSize + 1;
        for (int i = 0; i < vertCount * vertCount - vertCount; ++i)
        {
            if ((i + 1)%vertCount == 0)
                continue;

            triangles.AddRange(new List<int>()
            {
                i + 1 + vertCount, i + vertCount, i,
                i, i + 1, i + vertCount + 1
            });
        }

        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangles, 0);
        mesh.name = Guid.NewGuid().ToString();
        _meshFilter.sharedMesh = mesh;
        _meshCollider.sharedMesh = mesh;

        var vertexController = GetComponent<MeshVertexProbesController>();
        if(null != vertexController)
            vertexController.DeleteProbes();
    }

    [Button]
    private void GenerateNewGUID()
    {
        _meshFilter = _meshFilter ?? GetComponent<MeshFilter>();
        _meshFilter.sharedMesh.name = Guid.NewGuid().ToString();
    }
}
