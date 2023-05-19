using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class NewBehaviourScript : MonoBehaviour
{
    public float moveSpeed = 1f;
    public Rigidbody2D rb;
    public Weapon weapon;
    //public AudioClip gunshot;
    public float walkingDistance = 12;

    private Vector2 wolfPosition;
    private Vector2 selfPosition;

    private float isShooting = 0f;
    private bool isWalking = true;
    private float distanceToWolf = 0f;
    private int biteLayer;

    private Animator animator;
    private SpriteRenderer sprite_renderer;

    private void Start()
    {
        biteLayer = LayerMask.NameToLayer("bite");

        animator = GetComponent<Animator>();
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        wolfPosition = GameObject.Find("Wolf").transform.position;
        selfPosition = transform.position;
        distanceToWolf = Vector2.Distance(selfPosition, wolfPosition);//Mathf.Abs((wolfPosition - selfPosition).magnitude);

        if (distanceToWolf > walkingDistance)
        {
            animator.SetBool("isWalk", true);
            isWalking = true;

        } else
        {
            animator.SetBool("isWalk", false);
            isWalking = false;
        }

        if(isShooting>3 && ! isWalking)
        {
            weapon.Fire();
            animator.SetBool("isShooting", true);
            isShooting = 0;
            animator.SetBool("isShooting", false);
        }
        isShooting += Time.deltaTime;

        //if (isWalking)
        //{

            Vector2 aimDirection = wolfPosition - rb.position;
            float d = Vector2.Distance(wolfPosition, rb.position);
            transform.position = Vector2.MoveTowards(selfPosition, wolfPosition - aimDirection.normalized * 
                (walkingDistance -2) , moveSpeed*Time.deltaTime*d);

            //rb.AddForce(new Vector2(aimDirection.x * moveSpeed, aimDirection.y * moveSpeed));
            //rb.velocity = new Vector2(aimDirection.x * moveSpeed, aimDirection.y * moveSpeed);
        //}
        //else
        //{
          //  rb.velocity = new Vector2(0, 0);

        //}




    }
    private void FixedUpdate()
    {

        Vector2 aimDirection = wolfPosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        rb.rotation = aimAngle;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // set animator isWalk parameter
        bool new_is_walking = rb.velocity.sqrMagnitude > 0.1;
        if (isWalking != new_is_walking)
        {
            isWalking = new_is_walking;
            animator.SetBool("isWalk", isWalking);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == biteLayer)
        {
            animator.SetBool("isDead", true);

            Destroy(gameObject);
        }
    }
}
