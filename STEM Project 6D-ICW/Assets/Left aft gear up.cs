using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftAftGear : MonoBehaviour
{

    public Animator LeftAftGearAnimator;  // Reference to the Animator component
    private bool isUp = false; // State to track if the canopy is open or closed

    void Update()
    {
        // Check for button press (e.g., "G" key or any assigned input)
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (isUp)
            {
                LeftAftGearAnimator.SetTrigger("Left aft gear down");
                isUp = false;
            }
            else
            {
                // Trigger the animation
                LeftAftGearAnimator.SetTrigger("Left aft gear up");
                isUp = true;
            }

        }

    }
}
