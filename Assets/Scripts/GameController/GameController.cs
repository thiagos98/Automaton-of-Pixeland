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
        [FormerlySerializedAs("reloadLevelButton")] public Button pauseLevelButton;
        public GameObject pauseLevelPanel;

        [FormerlySerializedAs("GameOverPanel")] public GameObject gameOverPanel;
        [FormerlySerializedAs("VictoryPanel")] public GameObject victoryPanel;

        private int score;
        private readonly string scoreKey = "Score";
        public int CurrentScore { get; set; }
        private int currentLevel;

        private void Awake()
        {
            Time.timeScale = 1;
            currentLevel = 0;
            CurrentScore = PlayerPrefs.GetInt(scoreKey);
            LoadFromJson();
        }

        private void Start()
        {
            instance = this;
            score = CurrentScore;
            UpdateScoreText();
            currentLevelText.text = (currentLevel + 1).ToString();
            if (!pauseLevelButton.IsActive() && SceneManager.GetActiveScene().buildIndex != 0)
            {
                SetPauseLevelButton(true);
            }
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
            Time.timeScale = 0;
        }

        public void SetPauseLevelButton(bool value)
        {
            pauseLevelButton.gameObject.SetActive(value);
        }

        public void SetPauseLevelPanel(bool value)
        {
            Time.timeScale = value ? 0f : 1f;
            pauseLevelPanel.gameObject.SetActive(value);
        }

        public void ReloadGame()
        {
            SetScore(score);
            SetGameOver(false);
            FindObjectOfType<LevelGenerator>().GetComponent<LevelGenerator>().ExecuteScript();
            SetPauseLevelButton(true);
        }

        public int GetCurrentLevel()
        {
            return currentLevel;
        }
    
        public void AddCurrentLevel()
        {
            currentLevel += 1;
            currentLevelText.text = (currentLevel + 1).ToString();
        }

        public void GoToScene(string value)
        {
            SceneManager.LoadScene(value);
        }
    }
}