using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bite_Interaction : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float interactionDistance = 0.2f;
    public LayerMask interactableLayer;
    internal GameObject interactor;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance, interactableLayer))
            {
                // Move towards interactee object
                transform.position = Vector3.MoveTowards(transform.position, hit.collider.gameObject.transform.position, moveSpeed * Time.deltaTime);
            }
        }
    }
}
