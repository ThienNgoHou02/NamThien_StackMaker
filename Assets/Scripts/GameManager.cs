using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    [SerializeField]
    private Slider starCollection;
    [SerializeField]
    private GameObject completePanel;    
    [SerializeField]
    private GameObject settingPanel;
    [SerializeField]
    private Image[] starImages;
    [SerializeField]
    private GameObject[] starRatings;

    [SerializeField] private string nextLevelName;

    //Button
    [SerializeField]
    private Button
    nextLevelButton,
    settingButton,
    resumeButton,
    quitButton;
    private int numOfStar;
    private void Awake()
    {
        instance = this;
        starCollection.minValue = 1;
        starCollection.maxValue = 3;
        starCollection.value = 1;
        numOfStar = 0;

        nextLevelButton.gameObject.SetActive(false);
        completePanel.SetActive(false);
        DisableStar();
    }
    private void Start()
    {
        nextLevelButton.onClick.AddListener(()=>NextLevel(nextLevelName));
        settingButton.onClick.AddListener(EnableSettingPanel);
        resumeButton.onClick.AddListener(Resume);
        quitButton.onClick.AddListener(Quit);
    }
    private void DisableStar()
    {
        for (int i = 0; i < starImages.Length; i++)
        {
            starImages[i].color = Color.black;
        }
    }
    private void EnableCompletePanel()
    {
        completePanel.SetActive(true);
        if (numOfStar > 0)
        {
            StartCoroutine(RatingStars());
        }
    }
    private void NextLevel(string level)
    {
        PlayerPrefs.SetString("Level", nextLevelName);
        if (Time.timeScale < 1f)
            Time.timeScale = 1f;
        SceneManager.LoadScene(level);
    }
    private void EnableSettingPanel()
    {
        if (!settingPanel.activeSelf)
        {
            settingPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    private void Resume()
    {
        if (settingPanel.activeSelf)
        {
            settingPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
    private void Quit()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void StarCollect()
    {
        numOfStar++;
        starCollection.value = Mathf.Clamp(starCollection.value + 1, starCollection.minValue, starCollection.maxValue);
        starImages[numOfStar - 1].color = Color.white;
    }
    public void CompletePanel()
    {
        Invoke(nameof(EnableCompletePanel), 2f);
    }
    IEnumerator RatingStars()
    {
        for (int i = 0; i < numOfStar; i++)
        {
            starRatings[i].SetActive(true);
            yield return new WaitForSeconds(1f);
        }
        nextLevelButton.gameObject.SetActive(true);
    }
}
