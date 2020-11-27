using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static event Action OnGameOver;

    private GameState gameState;
    private void OnEnable()
    {
        UIManager.OnMenuButtonClicked += LoadMenu;
        UIManager.OnPauseButtonClicked += PauseOrResumeGame;
        UIManager.OnPlayButtonClicked += StartGame;
    }

    private void OnDisable()
    {
        UIManager.OnMenuButtonClicked -= LoadMenu;
        UIManager.OnPauseButtonClicked -= PauseOrResumeGame;
        UIManager.OnPlayButtonClicked -= StartGame;
    }
    
    private void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void PauseOrResumeGame()
    {
        Time.timeScale = Mathf.Abs(Time.timeScale - 1);
    }

    private void StartGame()
    {
        Time.timeScale = 1f;
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
        OnGameOver?.Invoke();
    }

    private void Start()
    {
        Time.timeScale = 0f;
    }

    private void Update()
    {
        
    }

    public GameState StateOfTheGame { get { return gameState; } }
}
