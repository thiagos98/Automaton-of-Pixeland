using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private int Score;
    public Text ScoreText;
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
    }
    
    public void UpdateScoreText()
    {
        ScoreText.text = Score.ToString();
    }
}
