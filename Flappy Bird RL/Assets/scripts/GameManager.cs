using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;

    public static GameManager Instance;

    public GameObject startPage;
    public Text scoreText;

    enum PageState
    {
        None,
        Start
    }

    int score = 0;
    bool gameOver = false;

    public bool GameOver { 
        get { return gameOver; }
        set { gameOver = false; }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
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
    }
    

    void OnPlayerScored()
    {
        score++;
        print(score);
    }

    void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                startPage.SetActive(false);
                break;

            case PageState.Start:
                startPage.SetActive(true);
                break;
        }
    }

    public void StartGame()
    {
        SetPageState(PageState.None);
        OnGameStarted();
        score = 0;
    }
}
