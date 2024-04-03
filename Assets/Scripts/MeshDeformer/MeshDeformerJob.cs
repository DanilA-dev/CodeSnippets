using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

[BurstCompile]
public struct MeshDeformerJob : IJobParallelFor
{
    public float deltaTime;
    public Vector3 center;
    public Vector3 maxDistance;
    public float radius;
    public float force;
    public bool ingnoreDeltaTime;
    public NativeArray<Vector3> normals;
    public NativeArray<Vector3> vertices;
    public NativeArray<VertexData> limitProbesData;

    public void Execute(int index)
    {
        Vector3 vertex = vertices[index];

        float endTime = ingnoreDeltaTime ? 1 : deltaTime;
        float offset = 0.1f;
        float a = Mathf.Pow(vertex.x - center.x, 2);
        float b = Mathf.Pow((vertex.y -  center.y * offset) / limitProbesData[index].startPos.y, 2);
        float c = Mathf.Pow(vertex.z - center.z, 2);
        if( a + b + c < Mathf.Pow(radius, 2) )
        {
            vertex += normals[index] * force * endTime;
            vertices[index] = vertex;
        }
    }
}
