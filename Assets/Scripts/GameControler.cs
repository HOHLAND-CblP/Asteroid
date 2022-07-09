using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControler : MonoBehaviour
{
    [Header("StarShip")]
    public GameObject starShip;

    [Header("Score")]
    public int scoreBigAsteroid;
    public int scoreMiddleAsteroid;
    public int scoreSmallAsteroid;
    public int scoreUFO;

    int score;

    [Header("UI")]
    public Text scoreText;
    [Space]
    public GameObject gameOverPanel;
    public Text finalScore;
    [Space]
    public GameObject pausePanel;
    public Button resumeButton;


    bool isPause;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause)
            {
                if(resumeButton.interactable)
                    ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void AsteroidDestroed(Asteroid.AsteroidSize size)
    {
        switch (size)
        {
            case Asteroid.AsteroidSize.Big:
                score += scoreBigAsteroid;
                break;

            case Asteroid.AsteroidSize.Middle:
                score += scoreMiddleAsteroid;
                break;

            case Asteroid.AsteroidSize.Small:
                score += scoreSmallAsteroid;
                break;
        }

        scoreText.text = score.ToString();
    }

    public void UFODestroed()
    {
        score += 200;

        scoreText.text = score.ToString();
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.Confined;

        pausePanel.SetActive(true);
        isPause = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        isPause = false;
        Time.timeScale = 1;

        starShip.GetComponent<StarShip>().ResumeGame(); 
    }

    public void NewGame()
    {
        resumeButton.interactable = true;
        ResumeGame();

        score = 0;
        scoreText.text = score.ToString();

        GetComponent<SpawnControler>().NewGame();
        starShip.GetComponent<StarShip>().NewGame();
    }

    public void GameOver()
    {
        Cursor.lockState = CursorLockMode.Confined;

        resumeButton.interactable = false;
        gameOverPanel.SetActive(true);
        finalScore.text = "You score: " + score.ToString();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
