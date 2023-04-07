using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Wolf_Moving : MonoBehaviour
{
    private float wolfSpeed = 2f;

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
        int direction = 0;

        // Right Behavior
        if (horizontalInput > 0f)
        {
            animator.SetInteger("Direction", 4); // right
            direction = 4;
        }
        else if (verticalInput > 0f || horizontalInput > 0f)
        {
            animator.SetInteger("Direction", 2); // up + right
            direction = 2;
        }
        else if (verticalInput < 0f || horizontalInput > 0f)
        {
            animator.SetInteger("Direction", 3); // down + right
            direction = 3;
        }
        else if (horizontalInput > 0f || horizontalInput < 0f)
        {
            if (direction == 2 || direction == 4 || direction == 3)
            {
                animator.SetInteger("Direction", 0); // left + right
                direction = 0;
            }
            else
            {
                animator.SetInteger("Direction", 1); // left + right
                direction = 1;
            }
        }

        // Left Behavior
        else if (horizontalInput < 0f)
        {
            animator.SetInteger("Direction", 5); // left
            direction = 5;
        }
        else if (verticalInput > 0f || horizontalInput < 0f)
        {
            animator.SetInteger("Direction", 6); // up + left
            direction = 6;
        }
        else if (verticalInput < 0f || horizontalInput < 0f)
        {
            animator.SetInteger("Direction", 7); // down + left
            direction = 7;
        }
        else if (horizontalInput > 0f || horizontalInput < 0f)
        {
            if (direction == 5 || direction == 6 || direction == 7)
            {
                animator.SetInteger("Direction", 1); // left + right
                direction = 1;
            }
            else
            {
                animator.SetInteger("Direction", 0); // left + right
                direction = 0;
            }

        }
        else
        {
            if (direction == 2 || direction == 4 || direction == 3)
            {
                animator.SetInteger("Direction", 0); // idle right
                direction = 0;
            }
            else
            {
                animator.SetInteger("Direction", 1); // idle left
                direction = 1;
            }

        }
    }
}

