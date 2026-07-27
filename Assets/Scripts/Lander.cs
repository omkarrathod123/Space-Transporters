using Assets.Scripts;
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
    private float maxFuelAmount = 10f;
    public event EventHandler onUpForce;
    public event EventHandler onRightForce;
    public event EventHandler onLeftForce;
    public event EventHandler onBeforeForce;
    public event EventHandler onCoinPickup;
    public event EventHandler<onLandedEventArgs> onLanded;
    public static Lander Instance { get; private set; }
    public enum LandingType
    {
        Success,
        WrongLandingArea,
        TooSteepAngle,
        TooFastLanding,
    }

    private void Awake()
    {
        landerRigidbody2D = GetComponent<Rigidbody2D>();
        Instance = this;
    }
    private void FixedUpdate()
    {
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
            onLanded.Invoke(this, new onLandedEventArgs
            {
                score = score,
            });
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision2d)
    {
        if(collision2d.gameObject.TryGetComponent(out FuelPickup fuelPickup))
        {
            fuelAmount = fuelPickup.GetRefuelAmount();
            fuelPickup.SelfDestroy();
        }
        if(collision2d.gameObject.TryGetComponent(out CoinPickup coinPickup))
        {
            onCoinPickup.Invoke(this, EventArgs.Empty);
            coinPickup.SelfDestroy();
        }
    }
    public float GetFuel()
    {
        return fuelAmount;
    }
    public float GetFuelAmountNormalized()
    {
        return fuelAmount / maxFuelAmount;
    }
}