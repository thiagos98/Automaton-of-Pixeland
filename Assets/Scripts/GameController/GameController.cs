using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public Text ScoreText;

    public GameObject GameOverPanel;
    private int Score;
    private readonly string scoreKey = "Score";
    public int CurrentScore { get; set; }

    private void Awake()
    {
        CurrentScore = PlayerPrefs.GetInt(scoreKey);
        
        LoadFromJson();
    }

    private void Start()
    {
        instance = this;
        Score = CurrentScore;
        UpdateScoreText();
    }
    
    public void LoadFromJson()
    {
        string json = File.ReadAllText(Application.dataPath + "/Resources/Files/Input.json");
        PlayerPrefs.SetString("CollectablesPerLevel", json);
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

    public void SetScore(int score)
    {
        PlayerPrefs.SetInt(scoreKey, score);
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
        SetScore(Score);
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - SceneManager.sceneCountInBuildSettings);
    }
}