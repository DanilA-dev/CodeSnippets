using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;


[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
public class MeshDeformer : MonoBehaviour
{
    [SerializeField] private MeshVertexProbesController _probeController;

    private Mesh _mesh;
    private MeshCollider _meshColldier;
    private NativeArray<Vector3> _vertices;
    private NativeArray<Vector3> _normals;
    private NativeArray<VertexData> _limitVerticesProbes;

    private List<Vector3> _savedVertices = new List<Vector3>();
    private List<Vector3> _savedNormals = new List<Vector3>();


    private JobHandle _jobHandle;
    private MeshDeformerJob _deformJob;
    private bool _isScheduled = false;

    private void Awake()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
        _meshColldier = GetComponent<MeshCollider>();
        _mesh.MarkDynamic();
        _meshColldier.sharedMesh = _mesh;
    }

    private void Start()
    {
        if(_savedVertices.Count > 0 && _savedNormals.Count > 0)
        {
            _mesh.vertices = _savedVertices.ToArray();
            _mesh.normals = _savedNormals.ToArray();
            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();
            _meshColldier.sharedMesh = _mesh;
        }

        _vertices = new NativeArray<Vector3>(_mesh.vertices, Allocator.Persistent);
        _normals = new NativeArray<Vector3>(_mesh.normals, Allocator.Persistent);
       _limitVerticesProbes = new NativeArray<VertexData>(_probeController.VerticesProbes.ToArray(), Allocator.Persistent);
    }

    private void OnDestroy()
    {
        _limitVerticesProbes.Dispose();
        _vertices.Dispose();
        _normals.Dispose();
    }


    private void LateUpdate()
    {
        if(!_isScheduled)
            return;

        _jobHandle.Complete();
        _isScheduled = false;

        _deformJob.vertices.CopyTo(_vertices);

        _mesh.vertices = _vertices.ToArray();
        _mesh.RecalculateBounds();

        _meshColldier.enabled = false;
        _meshColldier.enabled = true;
    }

    public void Deform(Vector3 deformPoint, float radius, float force, float timeMultiplier,bool ignoreDeltaTime = false)
    {
        _deformJob = new MeshDeformerJob();
        _deformJob.ingnoreDeltaTime = ignoreDeltaTime;
        _deformJob.deltaTime = Time.deltaTime * timeMultiplier;
        _deformJob.center = transform.InverseTransformPoint(deformPoint);
        _deformJob.radius = radius;
        _deformJob.force = force;
        _deformJob.limitProbesData = _limitVerticesProbes;
        _deformJob.vertices = _vertices;
        _deformJob.normals = _normals;
        _isScheduled = true;
        _savedNormals = _normals.ToList();
        _savedVertices = _vertices.ToList();
        _jobHandle = _deformJob.Schedule(_vertices.Length, 64);
        _mesh.RecalculateNormals();
    }
}
