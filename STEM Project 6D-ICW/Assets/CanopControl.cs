using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanopControl : MonoBehaviour
{

    public Animator canopyAnimator;  // Reference to the Animator component
    private bool isOpen = false; // State to track if the canopy is open or closed

    void Update()
    {
        // Check for button press (e.g., "C" key or any assigned input)
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isOpen)
            {
                canopyAnimator.SetTrigger("Close");
                isOpen = false;
            }else
            {
                // Trigger the animation
                canopyAnimator.SetTrigger("Open");
                isOpen = true;
            }

        }

    }
}


