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
}
