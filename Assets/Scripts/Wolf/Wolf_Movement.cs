using UnityEngine;

public class Wolf_Movement : MonoBehaviour
{
    public float wolf_speed = 20f;
    public float sqr_max_speed = 100f;

    private bool is_walking = false;
    private bool is_biting = false;
    private int active_bite_collider_index = -1;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite_renderer;
    private CircleCollider2D[] bite_colliders;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite_renderer = GetComponent<SpriteRenderer>();
        bite_colliders = GetComponentsInChildren<CircleCollider2D>();

        // sort the colliders by name
        System.Array.Sort(bite_colliders, new Sort());
        // disable all bite colliders
        foreach (CircleCollider2D bite_collider in bite_colliders)
        {
            bite_collider.enabled = false;
        }
    }

    public void BiteAnimationEnd()
    {
        is_biting = false;
        animator.SetInteger("isBite", -1);
        bite_colliders[active_bite_collider_index].enabled = false;
    }
    
    private void Update()
    {
        if (!is_biting && Input.GetKeyDown(KeyCode.Space))
        {
            is_biting = true;
            if (rb.velocity.y < 0)
            {
                animator.SetInteger("isBite", 0);
                active_bite_collider_index = sprite_renderer.flipX ? 2 : 3;
                bite_colliders[active_bite_collider_index].enabled = true;
            }
            else
            {
                animator.SetInteger("isBite", 1);
                active_bite_collider_index = sprite_renderer.flipX ? 1 : 0;
                bite_colliders[active_bite_collider_index].enabled = true;
            }
        }
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
        if (new_is_walking)
        {
            if ((sprite_renderer.flipX && rb.velocity.x > 0) || (!sprite_renderer.flipX && rb.velocity.x < 0))
            {
                sprite_renderer.flipX = !sprite_renderer.flipX;
            }
        }
    }
}
