using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    private float force = 700f;
    private float torque = 100f;
    private Rigidbody2D landerRigidbody2D;
    private void Awake()
    {
        landerRigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        LanderMovement();
    }

    private void LanderMovement()
    {
        if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed)
        {
            landerRigidbody2D.AddForce(force * transform.up * Time.deltaTime);
        }
        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
        {
            landerRigidbody2D.AddTorque(torque * Time.deltaTime);
        }
        if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
        {
            landerRigidbody2D.AddTorque(-torque * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        float softLandingVelocityMahnitude = 3f;
        float minSteepAngle = 0.90f;
        float steepAngle = Vector2.Dot(Vector2.up, transform.up);

        if(collision2D.relativeVelocity.magnitude > softLandingVelocityMahnitude) {
            Debug.Log(collision2D.relativeVelocity.magnitude+" " +"Soft Landing failed");
            return;
        }

        if(steepAngle < minSteepAngle)
        {
            Debug.LogError(steepAngle + " Landing fail.");
            return;
        }

        Debug.Log(collision2D.relativeVelocity.magnitude + " " + steepAngle + " Landing Successful!");
        
    }
}