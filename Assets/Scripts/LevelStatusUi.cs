using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelStatusUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dataText;
    [SerializeField] private Image fuelImage;
    private void Update()
    {
        UpdateDataText();
    }
    private void UpdateDataText()
    {
        dataText.text =
            GameManager.Instance.GetScore() + "\n" +
            Mathf.Round(GameManager.Instance.GetTime()) + "\n" +
            Mathf.Round(Lander.Instance.GetFuel()) * 10 ;
        fuelImage.fillAmount = Lander.Instance.GetFuelAmountNormalized();
    }
}
