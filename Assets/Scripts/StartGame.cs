using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    [SerializeField] Button playButton;

    private string level;
    private void Start()
    {
        playButton.onClick.AddListener(PlayGame);

        if (!PlayerPrefs.HasKey("Level"))
        {
            PlayerPrefs.SetString("Level", "Level1");
            level = "Level1";
        }
        else
            level = PlayerPrefs.GetString("Level");
    }
    private void PlayGame() 
    { 
        SceneManager.LoadScene(level);
    }
}
