using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    public Text HighScoreText;

    private void Start()
    {
        UpdateHighScore();
        HighScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
    }

    private void UpdateHighScore()
    {
        if (PlayerPrefs.GetInt("Score") > PlayerPrefs.GetInt("HighScore"))
            PlayerPrefs.SetInt("HighScore", PlayerPrefs.GetInt("Score"));
    }
}