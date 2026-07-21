using System;
using TMPro;
using UnityEngine;

public class LandingPad : MonoBehaviour
{
    [SerializeField] private int scoreMultiplier;
    [SerializeField] private TextMeshPro multiplierText;

    public int GetScoreMultiplier() { 
        return scoreMultiplier; 
    }
    private void Awake()
    {
        if(multiplierText != null)
        {
            multiplierText.text = "X" + GetScoreMultiplier();
        }
        else
        {
            Debug.LogError("Multiplier Text not fount!");
        }
    }
}
