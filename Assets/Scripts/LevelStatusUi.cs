using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelStatusUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dataText;
    [SerializeField] private TextMeshProUGUI labelText;
    [SerializeField] private Image fuelImage;
    private void Start()
    {
        AddLabelText();
    }

    private void Update()
    {
        UpdateDataText();
    }
    private void UpdateDataText()
    {
        dataText.text =
            LevelManager.Instance.GetCurrentLevel() + "\n" +
            GameManager.Instance.GetCurrentCoins() + ":" + GameManager.Instance.GetTotatCoins() + "\n" +
            GameManager.Instance.GetScore() + "\n" +
            Mathf.Round(GameManager.Instance.GetTime()) + "\n" +
            Mathf.Round(Lander.Instance.GetFuel()) * 10 ;
        fuelImage.fillAmount = Lander.Instance.GetFuelAmountNormalized();
    }
    private void AddLabelText()
    {
        labelText.text =
            "Level: \n" +
            "Coin: \n" +
            "Score: \n" +
            "Time: \n" +
            "Fuel:";
    }
}
