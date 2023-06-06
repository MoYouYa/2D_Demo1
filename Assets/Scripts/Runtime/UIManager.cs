using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public GameObject LevelUI;
    public GameObject MainUI;
    public Text scoreText;
    public Button puaseButton;
    public GameObject continueButton,nextLevelButton,restartButton,returnMenuButton;


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

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && puaseButton.enabled)
        {
            GamePuase();
        }
    }
    public void EnterMainFram()
    {
        MainUI.SetActive(true);
        LevelUI.SetActive(false);
        GameManager.Instance.DestroyMap();
        AudioManager.Instance.StopAudio("BGM");
    }

    public void EnterOptionFram()
    {

    }
    public void EnterLevel(int levelIndex)
    {
        MainUI.SetActive(false);
        LevelUI.SetActive(true);

        puaseButton.enabled = true;
        continueButton.SetActive(false);
        nextLevelButton.SetActive(false);
        restartButton.SetActive(false);
        returnMenuButton.SetActive(false);

        scoreText.text = ("0");

        GameManager.Instance.DestroyMap();
        GameManager.Instance.LoadMap("Assets/Resources/Maps/Map"+levelIndex.ToString()+".xml");

        AudioManager.Instance.PlayAudio("BGM");
    }

    public void AddScore(int score)
    {
        scoreText.text=(Int32.Parse(scoreText.text)  + score).ToString();
    }

    public void GameOver(bool isWin)
    {
        Debug.Log("Gameover" + isWin);
        puaseButton.enabled = false;
        continueButton.SetActive(false);
        if (isWin) {
            nextLevelButton.SetActive(true);
        }
        else
        {
            nextLevelButton.SetActive(false);
        }
        restartButton.SetActive(true);
        returnMenuButton.SetActive(true);
        //GameManager.Instance.DestroyMap();
        SetPlayerPause();
    }

    public void GamePuase()
    {
        puaseButton.enabled = false;
        continueButton.SetActive(true);
        nextLevelButton.SetActive(false);
        restartButton.SetActive(true);
        returnMenuButton.SetActive(true);
        SetPlayerPause();
    }

    public void GameContinue()
    {
        puaseButton.enabled = true;
        continueButton.SetActive(false);
        nextLevelButton.SetActive(false);
        restartButton.SetActive(false);
        returnMenuButton.SetActive(false);
        SetPlayerContinue();
    }
    private void SetPlayerPause()
    {
        Player player = FindObjectOfType<Player>();
        if(player != null )
        {
            player.enabled= false;
            player.GetComponent<Rigidbody2D>().Sleep();
        }
        //GetComponent<Player>().enabled= false;
    }

    private void SetPlayerContinue()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.enabled = true;
            player.GetComponent<Rigidbody2D>().WakeUp();
        }
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
