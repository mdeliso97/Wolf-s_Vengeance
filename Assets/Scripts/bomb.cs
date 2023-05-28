using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class bomb : MonoBehaviour
{
    public float duration = 5f;
    public float startTime;
    public float radius = 10f;
    private Animator animator;
    public AudioSource bombAudio;



    public void Start()
    {
        print("instantiated bomb");
        startTime = Time.time;
        animator = GetComponent<Animator>();
        bombAudio = GameObject.Find("ExplosionSound").GetComponent<AudioSource>();
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

        animator.SetBool("Explosion", true);
        bombAudio.Play();

        foreach (var col in colliders)
        {
            if (col.tag == "tree")
            {
                Destroy(col.gameObject);
            }
        }
        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.7f);

        Destroy(gameObject);
    }
}
