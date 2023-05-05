using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Wolf_Movement : MonoBehaviour
{
    private float wolfSpeed = 2f;
    bool direction_facing = true; // False is StandingLeft, True is StandingRight

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    private void Update()
    {

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        transform.position += new Vector3(horizontalInput * wolfSpeed * Time.deltaTime, verticalInput * wolfSpeed * Time.deltaTime, 0f);
        

        Animator animator = GetComponent<Animator>();

        animator.SetInteger("Direction", 0); // right walk

        if (horizontalInput > 0f)
        {
            animator.SetInteger("Direction", 1); // right walk
            direction_facing = true;
        }
        //else if (horizontalInput > 0f || Input.GetKey(KeyCode.LeftShift))
        //{
        //    animator.SetInteger("Direction", 4); // right run
        //    wolfSpeed = 5f;
        //    direction_facing = true;
        //}
        else if (verticalInput > 0f && horizontalInput > 0f)
        {
            animator.SetInteger("Direction", 1); // up + right
            direction_facing = true;
        }
        else if (verticalInput < 0f && horizontalInput > 0f)
        {
            animator.SetInteger("Direction", 1); // down + right
            direction_facing = true;
        }
        else if (horizontalInput < 0f)
        {
            animator.SetInteger("Direction", 2); // left walk
            direction_facing = false;
        }
        else if (direction_facing == true && verticalInput > 0 && horizontalInput == 0)
        {
            animator.SetInteger("Direction", 1); // Moving right direction up if last movement was right

        }
        else if (direction_facing == true && verticalInput < 0 && horizontalInput == 0)
        {
            animator.SetInteger("Direction", 1); // Moving right direction down if last movement was right
        }
        else if (direction_facing == false && verticalInput > 0 && horizontalInput == 0)
        {
            animator.SetInteger("Direction", 2); // Moving left direction up if last movement was left
        }
        else if (direction_facing == false && verticalInput < 0 && horizontalInput == 0)
        {
            animator.SetInteger("Direction", 2); // Moving left diretion down if last movement was left
        }
        else if (verticalInput > 0f && horizontalInput < 0f)
        {
            animator.SetInteger("Direction", 2); // up + left
            direction_facing = false;
        }

        else if (verticalInput < 0f && horizontalInput < 0f)
        {
            animator.SetInteger("Direction", 2); // down + left
            direction_facing = false;
        }
        else
        {
            if (direction_facing == true) 
                animator.SetInteger("Direction", 0); // idle right
            else
                animator.SetInteger("Direction", 3); // idle left
            if (horizontalInput > 0f && verticalInput == 0f && direction_facing == true || horizontalInput < 0f && verticalInput == 0f && direction_facing == true)
            {
                animator.SetInteger("Direction", 0); // left + right StandingRight
            }
            else if (horizontalInput > 0f && verticalInput == 0f && direction_facing == false || horizontalInput < 0f && verticalInput == 0f && direction_facing == false)
                animator.SetInteger("Direction", 1); // left + right StandingLeft
            if (horizontalInput == 0f && verticalInput < 0f && direction_facing == true || horizontalInput == 0f && verticalInput > 0f && direction_facing == true)
            {
                animator.SetInteger("Direction", 0); // up + down StandingRight
            }
            else if (horizontalInput == 0f && verticalInput < 0f && direction_facing == false || horizontalInput == 0f && verticalInput > 0f && direction_facing == false)
                animator.SetInteger("Direction", 3); // up + down StandingLeft
        }
    }
}
