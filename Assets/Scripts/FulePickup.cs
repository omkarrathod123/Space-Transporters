using UnityEngine;

public class FuelPickup : MonoBehaviour
{
    float refuelAmount = 10f;
    public float GetRefuelAmount()
    {
        return refuelAmount;
    }
    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}