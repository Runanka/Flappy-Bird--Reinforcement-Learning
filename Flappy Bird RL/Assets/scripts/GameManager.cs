using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;

    public static GameManager Instance;

    public GameObject startPage;
    public GameObject gameOverPage;
    public Text scoreText;

    enum PageState
    {
        None,
        Start,
        GameOver
    }

    int score = 0;
    bool gameOver = false;

    public bool GameOver { get { return gameOver; } }

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        TapController.OnPlayerDied += OnPlayerDied;
        TapController.OnPlayerScored += OnPlayerScored;
    }

    private void OnDisable()
    {
        TapController.OnPlayerDied -= OnPlayerDied;
        TapController.OnPlayerScored -= OnPlayerScored;
    }

    void OnPlayerDied()
    {
        gameOver = true;
        SetPageState(PageState.GameOver);
    }

    void OnPlayerScored()
    {
        score++;
        scoreText.text = score.ToString();
    }

    void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                break;

            case PageState.Start:
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                break;

            case PageState.GameOver:
                startPage.SetActive(true);
                gameOverPage.SetActive(true);
                break;
        }
    }


    public void StartGame()
    {
        SetPageState(PageState.None);
        OnGameStarted();
        score = 0;
        gameOver = false;
    }
}
