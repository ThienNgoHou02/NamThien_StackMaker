using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    [SerializeField] Button playButton;

    private string level = "Level1";
    private void Start()
    {
        playButton.onClick.AddListener(PlayGame);
    }
    private void PlayGame() 
    { 
        SceneManager.LoadScene(level);
    }
}
