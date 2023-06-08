using UnityEngine;

public class bomb : MonoBehaviour
{
    public float duration = 5f;
    public float startTime;
    public AudioSource bombAudio;
    public GameObject shockwave;

    private bool isExploding = false;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Animator shockwaveAnimator;
    private CircleCollider2D shockwaveCollider;

    public void Start()
    {
        startTime = Time.time;
        duration += Random.Range(-1f, 1f);

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        if (isExploding) return;

        isExploding = true;

        bombAudio.Play();
        animator.SetBool("explode", true);
        rb.rotation = 0f;
        rb.freezeRotation = true;

        shockwaveAnimator.SetBool("shockwave", true);
        shockwaveCollider.enabled = true;
    }

    private void ExplodeAnimationEnd()
    {
        shockwaveCollider.enabled = false;
        spriteRenderer.enabled = false;
        // wait with destroying the gameObject until the explosion sound has finished playing
        this.Invoke("destroy", 1.5f);
    }

    private void destroy()
    {
        Destroy(gameObject);
    }
}
