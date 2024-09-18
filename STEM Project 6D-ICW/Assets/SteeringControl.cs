using UnityEngine;

public class SteeringControl : MonoBehaviour
{
    public WheelCollider frontWheel; // Dit is de variabele die je ziet in de Inspector
    public float maxSteeringAngle = 30f; // Maximale stuurhoek

    void Update()
    {
        float steerInput = Input.GetAxis("Horizontal");
        float steeringAngle = maxSteeringAngle * steerInput;
        frontWheel.steerAngle = steeringAngle;
    }
}
