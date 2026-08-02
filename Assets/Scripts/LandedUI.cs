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
            SceneManager.LoadScene(0);
        });
        nextButton.onClick.AddListener(() =>
        {
            int nextScene = GameManager.Instance.SetCurrentLevel(GameManager.Instance.GetCurrentLevel() + 1);
            SceneManager.LoadScene(nextScene);
        });
    }

    private void Start()
    {
        nextButton.enabled = false;
        Lander.Instance.onLanded += Lander_onLanded;
        GameOver(false);
    }

    private void Lander_onLanded(object sender, Assets.Scripts.onLandedEventArgs e)
    {
        if(e.landingType == Lander.LandingType.Success)
        {
            bannerTitleText.text = "SUCCESSFUL LANDING!";
            nextButton.enabled = true;
        }
        else
        {
            bannerTitleText.text = "LANDING FAILED!";
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