using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Assertions;

public class Wolf_Movement : MonoBehaviour
{
    public float wolf_speed = 20f;
    public float sqr_max_speed = 100f;
    public float hitCooldownTime = 2f;

    public CapsuleCollider2D[] bite_colliders;

    public ParticleSystem blood_particles;

    private bool is_walking = false;
    private bool is_biting = false;
    private int active_bite_collider_index = -1;

    private int bombExpLayer;
    private int bulletLayer;
    private int jaegerLayer;
    private int bossHitLayer;
    private float hitTime = 0f;
    private bool isHit = false;
    private int stuckInsideBossCount = 0;

    private Health health;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite_renderer;
    private CapsuleCollider2D capsule_collider;
    private AudioSource bite0;
    private AudioSource bite1;

    void Start()
    {
        bombExpLayer = LayerMask.NameToLayer("bombExp");
        bulletLayer = LayerMask.NameToLayer("bullet");
        jaegerLayer = LayerMask.NameToLayer("jaeger");
        bossHitLayer = LayerMask.NameToLayer("bossHit");

        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite_renderer = GetComponent<SpriteRenderer>();
        capsule_collider = GetComponent<CapsuleCollider2D>();

        bite0 = GameObject.Find("SoundBite0").GetComponent<AudioSource>();
        bite1 = GameObject.Find("SoundBite1").GetComponent<AudioSource>();

        // sort the colliders by name
        System.Array.Sort(bite_colliders, new Sort());
        // disable all bite colliders
        foreach (CapsuleCollider2D bite_collider in bite_colliders)
        {
            bite_collider.enabled = false;
            print(bite_collider.name);
        }

        print(capsule_collider.name);
    }
    
    private void Update()
    {
        // update hit cooldown
        if (isHit) {
            hitTime += Time.deltaTime;

            if (hitTime >= hitCooldownTime) {
                isHit = false;
                hitTime = 0f;
                capsule_collider.excludeLayers = LayerMask.GetMask();
                sprite_renderer.color = new Color(sprite_renderer.color.r, sprite_renderer.color.g, sprite_renderer.color.b, 1f);
            }
        }

        // start biting
        if (!is_biting && Input.GetKeyDown(KeyCode.Space))
        {
            is_biting = true;
            if (rb.velocity.y < 0)
            {
                bite0.Play();
                animator.SetInteger("isBite", 0);
                active_bite_collider_index = sprite_renderer.flipX ? 2 : 3;
                bite_colliders[active_bite_collider_index].enabled = true;
            }
            else
            {
                bite1.Play();
                animator.SetInteger("isBite", 1);
                active_bite_collider_index = sprite_renderer.flipX ? 1 : 0;
                bite_colliders[active_bite_collider_index].enabled = true;
            }
        }

        // update y coordinate for rendering order
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y / 10000f);
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput != 0f || verticalInput != 0f)
        {
            Vector2 force = new Vector2(horizontalInput * wolf_speed, verticalInput * wolf_speed);
            if (Vector2.Dot(force, rb.velocity) < 0 || rb.velocity.sqrMagnitude < sqr_max_speed)
            {
                rb.AddForce(force);
            }
        }

        // set animator isWalk parameter
        bool new_is_walking = rb.velocity.sqrMagnitude > 0.1;
        if (is_walking != new_is_walking)
        {
            is_walking = new_is_walking;
            animator.SetBool("isWalk", is_walking);
        }
        // flip sprite depending on current walk direction
        if (new_is_walking)
        {
            if ((sprite_renderer.flipX && rb.velocity.x > 0) || (!sprite_renderer.flipX && rb.velocity.x < 0))
            {
                sprite_renderer.flipX = !sprite_renderer.flipX;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // hit by bullet
        if (collision.gameObject.layer == bulletLayer)
        {
            getHit(collision.transform.position);
        }
    }

    /* private void OnCollisionStay2D(Collision2D collision)
    {
        // stuck inside boss jaeger
        if (collision.gameObject.layer == jaegerLayer && collision.gameObject.tag == "bossJaeger")
        {
            stuckInsideBossCount++;

            if (stuckInsideBossCount > 10)
            {
                print("du unstuck");
                stuckInsideBossCount = 0;
                Vector3 collisionDirection3 = transform.position - collision.gameObject.transform.position;
                Vector2 collisionDirection = new Vector2(collisionDirection3.x, collisionDirection3.y).normalized;
                transform.position = rb.position + collisionDirection * 5;
            }
        }
    } */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // hit by bomb explosion radius
        if (collision.gameObject.layer == bombExpLayer)
        {
            getHit(collision.transform.position);
        }
        // hit by boss during dash attack
        else if (collision.gameObject.layer == bossHitLayer)
        {
            getHit(collision.transform.position);
            Vector3 collisionDirection = transform.position - collision.gameObject.transform.position;
            rb.AddForce(new Vector2(collisionDirection.x, collisionDirection.y).normalized * 20, ForceMode2D.Impulse);
        }
    }

    private void getHit(Vector3 collisionPosition)
    {
        if (!isHit)
        {
            health.health--;

            isHit = true;
            capsule_collider.excludeLayers = LayerMask.GetMask("bullet");
            sprite_renderer.color = new Color(sprite_renderer.color.r, sprite_renderer.color.g, sprite_renderer.color.b, 0.2f);

            ParticleSystem blood = Instantiate(blood_particles);
            blood.transform.position = collisionPosition;
            blood.Play();

            if (health.health == 0)
            {
                animator.SetBool("isDead", true);
                rb.drag = 100;
            }
            else
            {
                animator.SetBool("isHit", true);
                if (is_biting)
                {
                    BiteAnimationEnd();
                }
            }
        }
    }

    private void BiteAnimationEnd()
    {
        is_biting = false;
        animator.SetInteger("isBite", -1);
        bite_colliders[active_bite_collider_index].enabled = false;
    }

    private void DeadAnimationEnd()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private void HitAnimationEnd()
    {
        animator.SetBool("isHit", false);
    }
}
