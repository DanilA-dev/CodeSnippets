using UnityEngine;

public class MeshVertexProbe : MonoBehaviour
{
    public void Init(float radius)
    {
        transform.localScale = new Vector3(radius, radius, radius);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, transform.localScale.x);
    }
}
