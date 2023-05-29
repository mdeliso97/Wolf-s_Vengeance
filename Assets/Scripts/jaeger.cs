using System.Collections;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float moveSpeed = 1f;
    public Rigidbody2D rb;
    public Weapon weapon;
    public float walkingDistance = 12;

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
        biteLayer = LayerMask.NameToLayer("bite");

        animator = GetComponent<Animator>();
        sprite_renderer = GetComponent<SpriteRenderer>();

        wolf = GameObject.Find("Wolf");
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

        bool newIsWalking = distanceToWolf > walkingDistance;
        if (isWalking != newIsWalking)
        {
            isWalking = newIsWalking;
            animator.SetBool("isWalk", isWalking);
        }

        if (animator.GetBool("isShooting"))
        {
            animator.SetBool("isShooting", false);
        }

        if (isShooting > 3 && !isWalking)
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
            animator.SetBool("isDead", true);
            isDead = true;
            rb.simulated = false;
        }
    }

    private void AnimationFire()
    {
        weapon.Fire();
        print("shoot");
    }

    private void AnimationDeadEnd()
    {
        Destroy(gameObject);
    }
}
