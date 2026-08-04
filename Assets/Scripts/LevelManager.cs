using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int currentLevel;
    public static LevelManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(currentLevel);
    }
    public void NextLevel()
    {
        currentLevel++;
        SceneManager.LoadScene(currentLevel);
    }
    public int GetCurrentLevel()
    {
        return currentLevel;
    }
}
