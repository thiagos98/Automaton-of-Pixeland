using System.IO;
using TilesGenerators;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GameController
{
    public class GameController : MonoBehaviour
    {
        public static GameController instance;

        [FormerlySerializedAs("ScoreText")] public Text scoreText;
        public Text currentLevelText;

        [FormerlySerializedAs("GameOverPanel")] public GameObject gameOverPanel;
        [FormerlySerializedAs("VictoryPanel")] public GameObject victoryPanel;

        private int score;
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
            score = CurrentScore;
            UpdateScoreText();
        }

        private void LoadFromJson()
        {
            string json = File.ReadAllText(Application.dataPath + "/Resources/Files/seed.json");
            PlayerPrefs.SetString("InputLevel", json);
        }

        public void AddScore(int value)
        {
            score += value;
            UpdateScoreText();
        }

        public void SetScore(int value)
        {
            PlayerPrefs.SetInt(scoreKey, value);
        }

        private void UpdateScoreText()
        {
            scoreText.text = score.ToString();
        }

        public void NewGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // iniciar na primeira fase
        }

        public void SetGameOver(bool value)
        {
            gameOverPanel.SetActive(value);
        }

        public void SetVictory(bool value)
        {
            victoryPanel.SetActive(value);
        }

        public void ReloadGame(bool value)
        {
            SetScore(score);
            SetGameOver(false);
            FindObjectOfType<LevelGenerator>().GetComponent<LevelGenerator>().ExecuteScript();
        }

        public int GetCurrentLevel()
        {
            return currentLevel;
        }
    
        public void AddCurrentLevel()
        {
            currentLevel += 1;
        }

        public void GoToScene(string value)
        {
            SceneManager.LoadScene(value);
        }
    }
}