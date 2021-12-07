using UnityEngine;

public class DisplayHitbox : MonoBehaviour
{
    public Mesh meshDisplayed;

    public Color hitboxColor;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = hitboxColor;
        Gizmos.DrawMesh(meshDisplayed, transform.position, transform.rotation, transform.localScale);
        //Gizmos.DrawWireMesh(meshDisplayed, transform.position, transform.rotation, transform.localScale);
    }
}