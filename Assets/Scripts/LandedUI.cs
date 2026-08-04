using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bannerTitleText;
    [SerializeField] private TextMeshProUGUI dataText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(() =>
        {
            LevelManager.Instance.RestartLevel();
        });
        nextButton.onClick.AddListener(() =>
        {
            LevelManager.Instance.NextLevel();
        });
    }

    private void Start()
    {
        nextButton.gameObject.SetActive(false);
        Lander.Instance.onLanded += Lander_onLanded;
        GameOver(false);
    }

    private void Lander_onLanded(object sender, Assets.Scripts.onLandedEventArgs e)
    {
        if(e.landingType == Lander.LandingType.Success)
        {
            bannerTitleText.text = "SUCCESSFUL LANDING!";
            nextButton.gameObject.SetActive(true);
        }
        else
        {
            bannerTitleText.text = "LANDING FAILED!";
            nextButton.gameObject.SetActive(false);
        }
        dataText.text =
            Mathf.Round(e.landingSpeed * 1.3f) + "\n" +
            Mathf.Round(e.landingAngle) + "\n" +
            e.scoreMultiplier + "\n" +
            e.score;
        GameOver(true);
    }
    private void GameOver(bool b)
    {
        gameObject.SetActive(b);
    }
}