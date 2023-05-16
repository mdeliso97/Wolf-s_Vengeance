using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class bomb : MonoBehaviour
{
    public float duration = 5f;

    public float startTime;

    public float radius = 10f;



    public void Start()
    {
        print("instantiated bomb");
        startTime = Time.time;
    }

    public void Update()
    {
        if (Time.time - startTime > duration)
        {
            explode();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        explode();
    
    }

    private void explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        print(colliders.Length);
        foreach (var col in colliders)
        {
            if (col.tag == "tree")
            {
                Destroy(col.gameObject);
            }
        }

        Destroy(gameObject);
    }
}
