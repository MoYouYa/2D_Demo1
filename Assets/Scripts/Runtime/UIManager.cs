using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public GameObject LevelUI;
    public GameObject MainUI;
    public Text scoreText;
    public Button restartButton,returnMenuButton,nextLevelButton;


    // Start is called before the first frame update
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    private void Start()
    {
        EnterMainFram();
    }
    public void EnterMainFram()
    {
        MainUI.SetActive(true);
        LevelUI.SetActive(false);
        GameManager.Instance.DestroyMap();
    }

    public void EnterOptionFram()
    {

    }
    public void EnterLevel(int levelIndex)
    {
        MainUI.SetActive(false);
        LevelUI.SetActive(true);
        restartButton.enabled = false;
        returnMenuButton.enabled = false;
        nextLevelButton.enabled = false;
        GameManager.Instance.DestroyMap();
        GameManager.Instance.LoadMap("Assets/Resources/Maps/Map"+levelIndex.ToString()+".xml");
    }

    public void AddScore(int score)
    {
        scoreText.text=(Int32.Parse(scoreText.text)  + score).ToString();
    }

    //public void ArriveDestination()
    //{

    //}

    //public void LeaveDestination()
    //{

    //}

    //public void Win()
    //{
    //    restartButton.enabled = true;
    //    returnMenuButton.enabled = true;
    //    nextLevelButton.enabled = true;
    //    SetPlayerPause();
    //}

    //public void Defeat()
    //{
    //    restartButton.enabled = true;
    //    returnMenuButton.enabled = true;
    //    nextLevelButton.enabled = false;
    //    SetPlayerPause();
    //}

    public void GameOver(bool isWin)
    {
        restartButton.enabled = true;
        returnMenuButton.enabled = true;
        if (isWin) {
            nextLevelButton.enabled = true;
        }
        else
        {
            nextLevelButton.enabled = false;
        }
        SetPlayerPause();
    }
    private void SetPlayerPause()
    {
        FindObjectOfType<Player>().enabled= false;
        //GetComponent<Player>().enabled= false;
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying= false;
#else
        Application.Quit();
#endif
    }
}
