﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    private const float SQUARE_LENGTH = 0.5f;
    private Camera cam;
    private GameState gameState;

    private void OnEnable()
    {
        UIManager.OnMenuButtonClicked += LoadMenu;
        UIManager.OnPauseButtonClicked += PauseOrResumeGame;
        UIManager.OnPlayButtonClicked += StartGame;
        Tetro.OnGameOverCollision += GameOver;
        Board.OnClearLines += IncreaseScore;
    }

    private void OnDisable()
    {
        UIManager.OnMenuButtonClicked -= LoadMenu;
        UIManager.OnPauseButtonClicked -= PauseOrResumeGame;
        UIManager.OnPlayButtonClicked -= StartGame;
        Tetro.OnGameOverCollision -= GameOver;
        Board.OnClearLines -= IncreaseScore;
    }

    private void StartGame()
    {
        gameState.CurrentState = GameState.State.OnPlay;
        Time.timeScale = 1f;
    }

    private void PauseOrResumeGame()
    {
        if (gameState.CurrentState == GameState.State.Paused)
        {
            gameState.CurrentState = GameState.State.OnPlay;
        }
        else
        {
            gameState.CurrentState = GameState.State.Paused;
        }
        Time.timeScale = Mathf.Abs(Time.timeScale - 1);
    }

    private void GameOver()
    {
        gameState.CurrentState = GameState.State.GameOver;
        Time.timeScale = 0f;
    }

    private void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void IncreaseScore()
    {
        gameState.Score += 100;
    }

    private void Start()
    {
        gameState = new GameState();
        gameState.Score = 0;
        gameState.CurrentState = GameState.State.Start;

        Time.timeScale = 0f;

        cam = FindObjectOfType<Camera>();
        cam.orthographicSize = 15 * SQUARE_LENGTH;
    }

    public GameState StateOfTheGame { get { return gameState; } }
}
