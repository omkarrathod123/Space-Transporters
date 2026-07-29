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
    private const float GRAVITY_NORMAL = 0.7f;
    public event EventHandler onUpForce;
    public event EventHandler onRightForce;
    public event EventHandler onLeftForce;
    public event EventHandler onBeforeForce;
    public event EventHandler onCoinPickup;
    public event EventHandler<onLandedEventArgs> onLanded;
    public event EventHandler<onStateChangedEventArgs> onStateChanged;
    public static Lander Instance { get; private set; }
    public enum LandingType
    {
        Success,
        WrongLandingArea,
        TooSteepAngle,
        TooFastLanding,
    }
    public enum State
    {
        WaitingToStart,
        Normal,
        GameOver,
    }
    private State state;
    private void Awake()
    {
        landerRigidbody2D = GetComponent<Rigidbody2D>();
        Instance = this;
        landerRigidbody2D.gravityScale = 0f;
        SetState(State.WaitingToStart);
    }
    private void FixedUpdate()
    {
        
        switch (state)
        {
            default:
                
            case State.WaitingToStart:
                if(Keyboard.current.upArrowKey.isPressed||
                    Keyboard.current.downArrowKey.isPressed||
                    Keyboard.current.leftArrowKey.isPressed||
                    Keyboard.current.rightArrowKey.isPressed ||
                    Keyboard.current.wKey.isPressed||
                    Keyboard.current.sKey.isPressed||
                    Keyboard.current.dKey.isPressed||
                    Keyboard.current.aKey.isPressed)
                {
                    landerRigidbody2D.gravityScale = GRAVITY_NORMAL;
                    SetState(State.Normal);
                }
                break;
            case State.Normal:
                if (fuelAmount <= 0)
                {
                    onBeforeForce.Invoke(this, EventArgs.Empty);
                    return;
                }
                LanderMovement();
                break;
            case State.GameOver:
                break;
        }
        
    }
    private void SetState(State state)
    {
        this.state = state;
        onStateChanged?.Invoke(this, new onStateChangedEventArgs
        {
            state = state,
        });
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

        if (!collision2D.gameObject.TryGetComponent(out LandingPad landingPad)) {
            onLanded?.Invoke(this, new onLandedEventArgs
            {
                landingType = LandingType.WrongLandingArea,
                landingAngle = 0,
                landingSpeed = 0,
                scoreMultiplier = 0,
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }
        if (relativeVelocityMagnitude > softLandingVelocityMahnitude)
        {
            onLanded.Invoke(this, new onLandedEventArgs
            {
                landingType = LandingType.TooFastLanding,
                landingAngle = landingAngleScore,
                landingSpeed = 0,
                scoreMultiplier = landingPad.GetScoreMultiplier(),
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }

        if (steepAngle < minSteepAngle)
        {
            onLanded.Invoke(this, new onLandedEventArgs
            {
                landingType = LandingType.TooSteepAngle,
                landingAngle = 0,
                landingSpeed = landingSpeedScore,
                scoreMultiplier = landingPad.GetScoreMultiplier(),
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }
        landingAngleScore = maxAngleScore - Mathf.Abs(steepAngle - 1f) * scoreAngleMultiplier * maxAngleScore;
        landingSpeedScore = (softLandingVelocityMahnitude - relativeVelocityMagnitude) * maxSpeedScore;
        score = Mathf.RoundToInt((landingAngleScore + landingSpeedScore) * landingPad.GetScoreMultiplier());
        onLanded.Invoke(this, new onLandedEventArgs
        {
            landingType = LandingType.Success,
            landingAngle = landingAngleScore,
            landingSpeed = landingSpeedScore,
            scoreMultiplier = landingPad.GetScoreMultiplier(),
            score = score,
        });
        SetState(State.GameOver);
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