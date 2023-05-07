using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Wolf_Movement : MonoBehaviour
{
    public float wolf_speed = 20f;
    public float sqr_max_speed = 100f;

    private bool is_walking = false;
    private bool is_biting = false;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite_renderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    public void BiteAnimationEnd()
    {
        is_biting = false;
        animator.SetInteger("isBite", -1);
    }
    private void Update()
    {
        if (!is_biting && Input.GetKeyDown(KeyCode.Space))
        {
            is_biting = true;
            if (rb.velocity.y < 0)
            {
                animator.SetInteger("isBite", 0);
            }
            else
            {
                animator.SetInteger("isBite", 1);
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
