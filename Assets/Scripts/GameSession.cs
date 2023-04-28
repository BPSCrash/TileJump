using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;
    int addLivesScoreCounter = 0;
    private int highScore;

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject inGameUI;
    private void Awake()
    {
        int numGameSession = FindObjectsOfType<GameSession>().Length;
        if (numGameSession > 1)
        {
            Destroy(gameObject);
        } else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private class PrefKeys
    {
        public const string HIGHSCORE_KEY = "HighScore";
    }

    private void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();

        if (PlayerPrefs.GetInt(PrefKeys.HIGHSCORE_KEY) != null)
        {
            highScore = PlayerPrefs.GetInt(PrefKeys.HIGHSCORE_KEY);
            highScoreText.text = highScore.ToString();
        }
        else
        {
            PlayerPrefs.SetInt(PrefKeys.HIGHSCORE_KEY, 0);
            highScore = PlayerPrefs.GetInt(PrefKeys.HIGHSCORE_KEY);
            highScoreText.text = highScore.ToString();
        }
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        } else
        {
            GameOver();
        }
    }

    private void TakeLife()
    {
        playerLives -= 1;
        livesText.text = playerLives.ToString();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void IncreaseLife()
    {
        playerLives += 1;
        livesText.text = playerLives.ToString();
    }

     public void IncreaseScore(int increaseScoreby)
    {
        score += increaseScoreby;
        addLivesScoreCounter += increaseScoreby;
        scoreText.text = score.ToString();

        if(addLivesScoreCounter >= 500)
        {
            IncreaseLife();
            addLivesScoreCounter = 0;
        }

    }

    public void GameOver()
    {
        if(score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(PrefKeys.HIGHSCORE_KEY, highScore);
            highScore = PlayerPrefs.GetInt(PrefKeys.HIGHSCORE_KEY);
            highScoreText.text = highScore.ToString();
        }
        inGameUI.SetActive(false);
        gameOverScreen.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetGame()
    {
        inGameUI.SetActive(true);
        FindObjectOfType<ScenePersistance>().ResetPersistance();
        SceneManager.LoadScene(1);
        Destroy(gameObject);
    }
}
