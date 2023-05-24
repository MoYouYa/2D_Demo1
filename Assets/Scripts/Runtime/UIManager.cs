using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public GameObject UI;
    public GameObject MapUI;
    public GameObject MainUI;
    public Text scoreText;

    // Start is called before the first frame update
    public static UIManager instance;
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

    public void EnterLevel()
    {
        MapUI.SetActive(true);
        MainUI.SetActive(false);
    }

    public void AddScore(int score)
    {
        scoreText.text=(Int32.Parse(scoreText.text)  + score).ToString();
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
