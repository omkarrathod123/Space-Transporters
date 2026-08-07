using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashSceneManager : MonoBehaviour
{
    [SerializeField] private Animator splashScene;
    [SerializeField] private GameObject menu;
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    private void Start()
    {
        if (splashScene == null)
        {
            Application.Quit();
        }else
        {
            splashScene.Play("Run");
        }
        startButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(1);
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        Invoke("EnableMenu", 2f);
    }
    private void EnableMenu()
    {
        menu.SetActive(true);
    }
}
