using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float moveSpeed = 1f;
    public Rigidbody2D rb;
    public Weapon weapon;
    public float walkingDistance = 12;

    public AudioSource death0;
    public AudioSource death1;
    public AudioSource death2;
    public AudioSource death3;
    public AudioSource death4;
    public AudioSource death5;
    public AudioSource death6;
    public AudioSource death7;

    public AudioSource[] deathSounds;

    private Vector2 wolfPosition;
    private GameObject wolf;
    private Vector2 selfPosition;

    private float isShooting = 0f;
    private bool isWalking = true;
    private float distanceToWolf = 0f;
    private int biteLayer;
    private bool isDead = false;

    private Animator animator;
    private SpriteRenderer sprite_renderer;

    private void Start()
    {
        deathSounds = new AudioSource[]
        {
            death0,
            death1,
            death2,
            death3,
            death4,
            death5,
            death6,
            death7
        };

        biteLayer = LayerMask.NameToLayer("bite");

        animator = GetComponent<Animator>();
        sprite_renderer = GetComponent<SpriteRenderer>();

        wolf = GameObject.Find("Wolf");
    }

    private void Update()
    {
        // update y coordinate for rendering order
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y / 10000f);
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        wolfPosition = wolf.transform.position;
        selfPosition = transform.position;
        distanceToWolf = Vector2.Distance(selfPosition, wolfPosition);

        bool newIsWalking = distanceToWolf > walkingDistance + (isWalking ? 0 : 4);
        if (isWalking != newIsWalking)
        {
            isWalking = newIsWalking;
            animator.SetBool("isWalk", isWalking);
        }

        if (isShooting > 4 && !isWalking)
        {
            animator.SetBool("isShooting", true);
            isShooting = 0;
        }
        isShooting += Time.deltaTime;

        Vector2 aimDirection = wolfPosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        rb.rotation = aimAngle;

        // flip sprite depending on current walk direction
        if ((sprite_renderer.flipY && aimDirection.x > 0) || (!sprite_renderer.flipY && aimDirection.x < 0))
        {
            sprite_renderer.flipY = !sprite_renderer.flipY;
        }

        if (isWalking)
        {
            float d = Vector2.Distance(wolfPosition, rb.position);
            transform.position = Vector2.MoveTowards(
                selfPosition,
                wolfPosition - aimDirection.normalized * (walkingDistance - 2),
                moveSpeed * Time.deltaTime * d
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == biteLayer)
        {
            AudioSource deadSound = SelectSound();
            animator.SetBool("isDead", true);
            deadSound.Play();

            isDead = true;
            rb.simulated = false;
        }
    }

    private void AnimationFire()
    {
        animator.SetBool("isShooting", false);
        weapon.Fire();
    }

    private void AnimationDeadEnd()
    {
        Destroy(gameObject);
    }

    private AudioSource SelectSound()
    {
        int randomIndex = Random.Range(0, deathSounds.Length);
        return deathSounds[randomIndex];
    }
}
