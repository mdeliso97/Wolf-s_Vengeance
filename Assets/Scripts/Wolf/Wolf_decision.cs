using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf_decision : StateMachineBehaviour
{
    public GameObject Wolf_StandingRight;
    public GameObject Wolf_StandingLeft;

    private bool facingRight = true;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Check for input to switch environment
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput < 0f && facingRight)
        {
            // Switch to left environment
            Wolf_StandingRight.SetActive(false);
            Wolf_StandingLeft.SetActive(true);
            facingRight = false;
        }
        else if (horizontalInput > 0f && !facingRight)
        {
            // Switch to right environment
            Wolf_StandingRight.SetActive(true);
            Wolf_StandingLeft.SetActive(false);
            facingRight = true;
        }
    }
}
