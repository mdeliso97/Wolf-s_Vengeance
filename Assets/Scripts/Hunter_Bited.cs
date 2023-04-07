using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter_Bited : MonoBehaviour
{
    public float interactionDistance = 2.0f;
    public int health = 100;

    private Bite_Interaction biteInteraction;

    private void Awake()
    {
        // Get the Bite_Interaction component
        biteInteraction = GetComponent<Bite_Interaction>();
    }

    // Update is called once per frame
    void OnTriggerStay(Collider obj_collide)
    {
        if (obj_collide.gameObject == biteInteraction.interactor && Input.GetKeyDown(KeyCode.Space))
        {
            // Perform interaction with interactor object
            health -= 20;
            if (health <= 0)
            {
                gameObject.SetActive(false); // Disable the game object
            }
        }
    }
}
