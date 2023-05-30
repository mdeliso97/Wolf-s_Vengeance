using UnityEngine;

public class bomb : MonoBehaviour
{
    public float duration = 5f;
    public float startTime;
    public float radius = 10f;
    public AudioSource bombAudio;

    private Rigidbody2D rb;
    private Animator animator;

    public void Start()
    {
        startTime = Time.time;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb.AddTorque(0.5f, ForceMode2D.Impulse);
    }

    public void Update()
    {
        // update y coordinate for rendering order
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y / 10000f);

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

        animator.SetBool("explode", true);
        bombAudio.Play();
        rb.rotation = 0f;
        rb.freezeRotation = true;

        foreach (var col in colliders)
        {
            if (col.tag == "tree")
            {
                Destroy(col.gameObject);
            }
        }
    }

    private void ExplodeAnimationEnd()
    {
        Destroy(gameObject);
    }
}
