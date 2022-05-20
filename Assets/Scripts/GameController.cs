﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private int Score;
    public int highScore;
    
    public Text ScoreText;
    public Text HighScoreText;
    
    public GameObject GameOverPanel;
    public static GameController instance;

    private void Start()
    {
        instance = this;
    }

    public int GetScore()
    {
        return Score;
    }
    public void AddScore(int value)
    {
        Score += value;
        UpdateScoreText();
        
    }

    private void UpdateScoreText()
    {
        ScoreText.text = Score.ToString();
    }

    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // iniciar na primeira fase
    }

    public void ShowGameOver()
    {
        GameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - SceneManager.sceneCountInBuildSettings);
        }
    }
}
