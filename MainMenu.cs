using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Canvas BTNHolder;
    public Canvas TitleText;
    public Canvas ControlsCanvas;
    public Canvas CreditsCanvas;
    public Canvas LeaderBoardCanvas;

    public GameObject EnemiesPanel;
    public GameObject BossPanel;
    public GameObject CollectablesPanel;
    public GameObject ExtraNotesPanel;

    public Text FirstHighScore;
    public Text SecondHighScore;
    public Text ThirdHighScore;
    public Text FourthHighScore;
    public Text FifthHighScore;

    public AudioSource buttonClick;

    public bool isShowingControls = false;
    public bool isShowingCredits = false;

    // Start is called before the first frame update
    void Start()
    {
        float highScoreOne = PlayerPrefs.GetFloat("highscore1");
        float highScoreTwo = PlayerPrefs.GetFloat("highscore2");
        float highScoreThree = PlayerPrefs.GetFloat("highscore3");
        float highScoreFour = PlayerPrefs.GetFloat("highscore4");
        float highScoreFive = PlayerPrefs.GetFloat("highscore5");

        string playerNameOne = PlayerPrefs.GetString("playername1");
        string playerNameTwo = PlayerPrefs.GetString("playername2");
        string playerNameThree = PlayerPrefs.GetString("playername3");
        string playerNameFour = PlayerPrefs.GetString("playername4");
        string playerNameFive = PlayerPrefs.GetString("playername5");

        FirstHighScore.text = highScoreOne + " - " + playerNameOne;
        SecondHighScore.text = highScoreTwo + " - " + playerNameTwo;
        ThirdHighScore.text = highScoreThree + " - " + playerNameThree;
        FourthHighScore.text = highScoreFour + " - " + playerNameFour;
        FifthHighScore.text = highScoreFive + " - " + playerNameFive;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Quit()
    {
        buttonClick.Play();
        Application.Quit();
        Debug.Log("QUIT!");
    }

    public void ShowControls()
    {
        if (!isShowingControls)
        {
            TitleText.enabled = false;
            BTNHolder.enabled = false;
            LeaderBoardCanvas.enabled = false;
            ControlsCanvas.enabled = true;
            EnemiesPanel.SetActive(true);
            BossPanel.SetActive(false);
            CollectablesPanel.SetActive(false);
            ExtraNotesPanel.SetActive(false);
            isShowingControls = true;
            buttonClick.Play();
        }
        else
        {
            ControlsCanvas.enabled = false;
            LeaderBoardCanvas.enabled = true;
            TitleText.enabled = true;
            BTNHolder.enabled = true;
            isShowingControls = false;
            buttonClick.Play();
        }
    }

    public void ShowCredits()
    {
        if (!isShowingCredits)
        {
            TitleText.enabled = false;
            BTNHolder.enabled = false;
            LeaderBoardCanvas.enabled = false;
            CreditsCanvas.enabled = true;
            isShowingCredits = true;
            buttonClick.Play();
        }
        else
        {
            CreditsCanvas.enabled = false;
            LeaderBoardCanvas.enabled = true;
            TitleText.enabled = true;
            BTNHolder.enabled = true;
            isShowingCredits = false;
            buttonClick.Play();
        }
    }

    public void EnemiesNextBTN()
    {
        BossPanel.SetActive(true);
        EnemiesPanel.SetActive(false);
        buttonClick.Play();
    }

    public void BossNextBTN()
    {
        CollectablesPanel.SetActive(true);
        BossPanel.SetActive(false);
        buttonClick.Play();
    }

    public void BossBackBTN()
    {
        EnemiesPanel.SetActive(true);
        BossPanel.SetActive(false);
        buttonClick.Play();
    }

    public void CollectablesNextBTN()
    {
        ExtraNotesPanel.SetActive(true);
        CollectablesPanel.SetActive(false);
        buttonClick.Play();
    }

    public void CollectablesBackBTN()
    {
        BossPanel.SetActive(true);
        CollectablesPanel.SetActive(false);
        buttonClick.Play();
    }

    public void ExtraNotesBackBTN()
    {
        CollectablesPanel.SetActive(true);
        ExtraNotesPanel.SetActive(false);
        buttonClick.Play();
    }
}
