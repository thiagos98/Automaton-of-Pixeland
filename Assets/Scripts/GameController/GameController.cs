using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TilesGenerators;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public Text ScoreText;

    public GameObject GameOverPanel;
    public GameObject VictoryPanel;

    private int Score;
    private readonly string scoreKey = "Score";
    public int CurrentScore { get; set; }
    private int currentLevel;

    private void Awake()
    {
        currentLevel = 0;
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
        PlayerPrefs.SetString("InputLevel", json);
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

    public void SetGameOver(bool value)
    {
        GameOverPanel.SetActive(value);
    }

    public void SetVictory(bool value)
    {
        VictoryPanel.SetActive(value);
    }

    public void ReloadGame(bool value)
    {
        SetScore(Score);
        FindObjectOfType<Player>().Live();
        FindObjectOfType<NextLevelPoint>().RestartLevelPoint();
        // FindObjectOfType<CellularAutomata>().GetComponent<CellularAutomata>().ExecuteScript();
        FindObjectOfType<SpawnerObjects>().GetComponent<SpawnerObjects>().ExecuteScript();
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
    
    public void AddCurrentLevel()
    {
        currentLevel += 1;
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}