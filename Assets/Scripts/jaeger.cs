using System.Collections;
using UnityEngine;

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
        } else
        {
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
        if (animator.GetBool("isDead") != true)
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
       

        //// check if the aim angle is pointing downwards or upwards
        //if (aimAngle > 90f || aimAngle < -90f)
        //{
        //    // flip the sprite along the X-axis
        //    transform.localScale = new Vector3(-0.1415845f, 0.15559f, 1f);
        //}
        //else
        //{
        //    // Reset the sprite scale if not flipped
        //    transform.localScale = Vector3.one;
        //}

        //// Set the rotation of the sprite itself
        //transform.eulerAngles = new Vector3(0f, 0f, aimAngle);


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == biteLayer)
        {
            animator.SetBool("isDead", true);
            rb.rotation = 0;
            StartCoroutine(DestroyAfterAnimation());
        }
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
