using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    private int treeLayer;

    void Start()
    {
        treeLayer = LayerMask.NameToLayer("tree");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == treeLayer)
        {
            Destroy(collision.gameObject);
        }
    }
}
