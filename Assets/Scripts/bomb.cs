using UnityEngine;

public class bomb : MonoBehaviour
{
    public float duration = 5f;
    public float startTime;
    public AudioSource bombAudio;
    public GameObject shockwave;

    private Rigidbody2D rb;
    private Animator animator;
    private Animator shockwaveAnimator;
    private CircleCollider2D shockwaveCollider;

    public void Start()
    {
        startTime = Time.time;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        shockwaveAnimator = shockwave.GetComponent<Animator>();
        shockwaveCollider = shockwave.GetComponent<CircleCollider2D>();
        shockwaveCollider.enabled = false;

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
        animator.SetBool("explode", true);
        bombAudio.Play();
        rb.rotation = 0f;
        rb.freezeRotation = true;

        shockwaveAnimator.SetBool("shockwave", true);
        shockwaveCollider.enabled = true;
    }

    private void ExplodeAnimationEnd()
    {
        Destroy(gameObject);
    }
}
