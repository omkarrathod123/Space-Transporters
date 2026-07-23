using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    private float force = 700f;
    private float torque = 100f;
    private Rigidbody2D landerRigidbody2D;
    private float fuelAmount = 5;
    public event EventHandler onUpForce;
    public event EventHandler onRightForce;
    public event EventHandler onLeftForce;
    public event EventHandler onBeforeForce;
    private void Awake()
    {
        landerRigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        Debug.Log(fuelAmount);
        if(fuelAmount <= 0) { 
            onBeforeForce.Invoke(this, EventArgs.Empty);
            return; 
        }
        LanderMovement();
    }

    private void LanderMovement()
    {
        onBeforeForce?.Invoke(this, EventArgs.Empty);
        if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed)
        {
            landerRigidbody2D.AddForce(force * transform.up * Time.deltaTime);
            FuelConsumption(0.6f);
            onUpForce?.Invoke(this, EventArgs.Empty);
        }
        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
        {
            landerRigidbody2D.AddTorque(torque * Time.deltaTime);
            FuelConsumption(0.2f);
            onLeftForce?.Invoke(this, EventArgs.Empty);
        }
        if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
        {
            landerRigidbody2D.AddTorque(-torque * Time.deltaTime);
            FuelConsumption(0.2f);
            onRightForce?.Invoke(this, EventArgs.Empty);
        }
    }

    private void FuelConsumption(float fuelConsumptionAmount)
    {
        fuelAmount -= fuelConsumptionAmount * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        float softLandingVelocityMahnitude = 3f;
        float minSteepAngle = 0.90f;
        float steepAngle = Vector2.Dot(Vector2.up, transform.up);
        float maxAngleScore = 100f;
        float scoreAngleMultiplier = 10f;
        float maxSpeedScore = 100f;
        float landingSpeedScore = new float();
        float landingAngleScore = new float();
        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;
        int score;

        if (collision2D.gameObject.TryGetComponent(out LandingPad landingPad)) {
            Debug.Log("Landing Pad is Detected!");
            if (relativeVelocityMagnitude > softLandingVelocityMahnitude)
            {
                Debug.Log(relativeVelocityMagnitude + " " + "Soft Landing failed");
                return;
            }

            if (steepAngle < minSteepAngle)
            {
                Debug.LogError(steepAngle + " Landing fail.");
                return;
            }
            landingAngleScore = maxAngleScore - Mathf.Abs(steepAngle - 1f) * scoreAngleMultiplier * maxAngleScore;
            landingSpeedScore = (softLandingVelocityMahnitude - relativeVelocityMagnitude) * maxSpeedScore;
            score = Mathf.RoundToInt((landingAngleScore + landingSpeedScore) * landingPad.GetScoreMultiplier());
            Debug.Log(collision2D.relativeVelocity.magnitude + " " + steepAngle + " Landing Successful!");
            Debug.Log("Speed Score: " + landingSpeedScore + " Angle Score: " + landingAngleScore + " Score: " + score);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision2d)
    {
        if(collision2d.gameObject.TryGetComponent(out FuelPickup fuelPickup))
        {
            fuelAmount = fuelPickup.GetRefuelAmount();
            fuelPickup.SelfDestroy();
        }
    }
}