using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int score;
    private float time;
    private bool isTimerActive;
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Lander.Instance.onCoinPickup += Lander_onCoinPickup;
        Lander.Instance.onLanded += Lander_onLanded;
        Lander.Instance.onStateChanged += Lander_onStateChanged;
    }

    private void Lander_onStateChanged(object sender, onStateChangedEventArgs e)
    {
        isTimerActive = e.state == Lander.State.Normal;
    }

    private void Update()
    {
        if (isTimerActive)
        {
            time += Time.deltaTime;
        }
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