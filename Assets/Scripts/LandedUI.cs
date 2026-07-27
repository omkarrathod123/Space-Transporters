using TMPro;
using UnityEngine;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bannerTitleText;
    [SerializeField] private TextMeshProUGUI dataText;

    private void Start()
    {
        Lander.Instance.onLanded += Lander_onLanded;
        GameOver(false);
    }

    private void Lander_onLanded(object sender, Assets.Scripts.onLandedEventArgs e)
    {
        if(e.landingType == Lander.LandingType.Success)
        {
            bannerTitleText.text = "SUCCESSFUL LANDING!";
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