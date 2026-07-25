using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int score;
    private float time;
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Lander.Instance.onCoinPickup += Lander_onCoinPickup;
        Lander.Instance.onLanded += Lander_onLanded;
    }
    private void Update()
    {
        time += Time.deltaTime;
    }

    private void Lander_onLanded(object sender, Assets.Scripts.onLandedEventArgs e)
    {
        AddScore(e.score);
    }

    private void Lander_onCoinPickup(object sender, System.EventArgs e)
    {
        AddScore(1);
    }

    public void AddScore(int score)
    {
        this.score += score;
        Debug.Log("Current Score is " + this.score);
    }
    public int GetScore()
    {
        return this.score;
    }
    public float GetTime()
    {
        return time;
    }
}