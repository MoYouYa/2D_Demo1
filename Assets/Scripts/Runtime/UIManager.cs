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
    public GameObject mainFram,optionFram,levelUI,gamePuaseUI,victryUI,defeatUI;
    private Text scoreText;
    //private Camera mainCamera;


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

    private void Update()
    {
        ListeningInput();
    }

    private void ListeningInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GamePuase();
        }
    }
    public void EnterMainFram()
    {
        mainFram.SetActive(true);
        optionFram.SetActive(false);
        levelUI.SetActive(false);
        gamePuaseUI.SetActive(false);
        victryUI.SetActive(false); 
        defeatUI.SetActive(false);
        GameManager.Instance.DestroyMap();
        AudioManager.Instance.StopAudio("LevelBGM");
        AudioManager.Instance.PlayAudio("MenuBGM");
    }

    public void EnterOptionFram()
    {
        mainFram.SetActive(false);
        optionFram.SetActive(true);
        levelUI.SetActive(false);
        gamePuaseUI.SetActive(false);
        victryUI.SetActive(false);
        defeatUI.SetActive(false);
    }
    public void EnterLevel(int levelIndex)
    {
        mainFram.SetActive(false);
        optionFram.SetActive(false);
        levelUI.SetActive(true);
        gamePuaseUI.SetActive(false);
        victryUI.SetActive(false);
        defeatUI.SetActive(false);

        GetScoreText().text = ("0");

        Camera.main.transform.position = new Vector3(0, 0, -10);

        GameManager.Instance.DestroyMap();
        GameManager.Instance.LoadMap("/StreamingAssets/Maps/Map"+levelIndex.ToString()+".xml");

        //AudioManager.Instance.PlayAudio("BGM");
        AudioManager.Instance.StopAudio("MenuBGM");
        AudioManager.Instance.PlayAudio("LevelBGM");
    }

    public void AddScore(int score)
    {
        GetScoreText().text=(Int32.Parse(GetScoreText().text)  + score).ToString();
    }

    public void GameOver(bool isWin)
    {
        Debug.Log("Gameover and result is " + isWin);
        if(isWin)
        {
            victryUI.SetActive(true);
        }
        else
        {
            defeatUI.SetActive(true);
        }
        GameManager.Instance.GamePuase();
        //GameManager.Instance.DestroyMap();
        //SetPlayerPause();
    }

    public void GamePuase()
    {
        gamePuaseUI.SetActive(true);
        GameManager.Instance.GamePuase();
    }

    public void GameContinue()
    {
        gamePuaseUI.SetActive(false);
        GameManager.Instance.GameContinue();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying= false;
#else
        Application.Quit();
#endif
    }

    private Text GetScoreText()
    {
        if(scoreText == null)
        {
            Text[] texts = levelUI.GetComponentsInChildren<Text>();
            foreach (Text text in texts)
            {
                Debug.Log(text.name);
                if (text.name == "ScoreText")
                {
                    scoreText = text;
                    break;
                }
            }
        }
        return scoreText;
    }

    //private Camera GetMainCamera()
    //{
    //    if(mainCamera==null)
    //    {
    //        mainCamera = Camera.;
    //    }
    //}
}
