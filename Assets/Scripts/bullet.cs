using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    private void Update()
    {
        // update y coordinate for rendering order
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y / 10000f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
