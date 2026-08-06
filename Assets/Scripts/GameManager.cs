using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int score;
    private float time;
    private bool isTimerActive;
    [SerializeField] private List<CoinPickup> coins;
    private int currrentCoins;
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
        coins = FindObjectsByType<CoinPickup>(FindObjectsSortMode.None).ToList();
        currrentCoins = 0;
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
    public int AddCoin()
    {
        currrentCoins++;
        return currrentCoins;
    }
    public int GetCurrentCoins()
    {
        return currrentCoins;
    }
    public int GetTotatCoins()
    {
        return coins.Count;
    }
}