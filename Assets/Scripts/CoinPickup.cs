using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
