using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{


    public WheelCollider frontWheel; // Sleep hier je voorste WheelCollider in vanuit de inspector
    public float maxSteeringAngle = 30f; // Maximale stuurhoek

    void Update()
    {
        // Haal de stuurinvoer op (pijltjestoetsen of een andere input)
        float steerInput = Input.GetAxis("Horizontal");

        // Bereken de stuurhoek
        float steeringAngle = maxSteeringAngle * steerInput;

        // Pas de stuurhoek toe op de voorste WheelCollider
        frontWheel.steerAngle = steeringAngle;
    }
}


