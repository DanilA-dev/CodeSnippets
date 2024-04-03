using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshDeformer))]
public class MeshVertexProbesController : MonoBehaviour
{
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private float _probeRadius;
    [SerializeField] private MeshVertexProbe _probePrefab;
    [SerializeField] private List<MeshVertexProbe> _verticesProbes = new List<MeshVertexProbe>();
    [SerializeField] private List<VertexData> _vertitesData = new List<VertexData>();


    public List<VertexData> VerticesProbes {get => _vertitesData; set => _vertitesData = value;}

    private void Reset()
    {
        _meshFilter = GetComponent<MeshFilter>();
    }

    private void Start()
    {
        HideProbes();
    }
    
    [Button]
    private void CreateVertexProbes()
    {
        if(null == _meshFilter)
            return;

        var mesh = _meshFilter.sharedMesh;
        var vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            var newProbe = Instantiate(_probePrefab, this.transform);
            newProbe.transform.localPosition = vertices[i];
            newProbe.Init(_probeRadius);
            _verticesProbes.Add(newProbe);
        }
    }

    [Button, GUIColor(0,1,0)]
    private void BakeProbesPositions()
    {
        if(_verticesProbes.Count <= 0)
            return;

         _vertitesData.Clear();

        for (int i = 0; i < _verticesProbes.Count; i++)
        {
            var newVertData = new VertexData
            {
               index = i,
               startPos = _verticesProbes[i].transform.localPosition
            };
            _vertitesData.Add(newVertData);
        }
        Debug.Log($"<color=green> Vertex probes positions are baked </color>");

    }

    [Button, GUIColor(1,0,0)]
    public void DeleteProbes()
    {
        if(_verticesProbes.Count <= 0)
            return;

        _verticesProbes.ForEach(x => DestroyImmediate(x.gameObject));
        _verticesProbes.Clear();
        _vertitesData.Clear();
    }

    [HorizontalGroup("H")]
    [Button(ButtonSizes.Medium)]
    private void ShowProbes()
    {
        if(_verticesProbes.Count > 0)
            _verticesProbes.ForEach(v => v.gameObject.SetActive(true));
    }

    [HorizontalGroup("H")]
    [Button(ButtonSizes.Medium)]
    private void HideProbes()
    {
        if(_verticesProbes.Count > 0)
            _verticesProbes.ForEach(v => v.gameObject.SetActive(false));
    }
    
}

[System.Serializable]
public struct VertexData
{
    public int index;
    public Vector3 startPos;
}