using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public void SelfDestroy()
    {
        GameManager.Instance.AddCoin();
        Destroy(gameObject);
    }
}
