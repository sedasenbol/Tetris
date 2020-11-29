using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    public static event Action OnPlayButtonClicked;
    public static event Action OnPauseButtonClicked;
    public static event Action OnMenuButtonClicked;

    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Button pauseButton;
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button menuButton;
    [SerializeField]
    private Text gameOverText;

    private GameState gameState;

    private void OnEnable()
    {
        Tetro.OnGameOverCollision += LoadGameOverScreen;
    }

    private void OnDisable()
    {
        Tetro.OnGameOverCollision -= LoadGameOverScreen;
    }

    public void PlayButtonClicked()
    {
        OnPlayButtonClicked?.Invoke();
        playButton.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(true);
    }

    public void PauseButtonClicked()
    {
        OnPauseButtonClicked?.Invoke();
    }

    private void LoadGameOverScreen()
    {
        pauseButton.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(true);

        gameOverText.text = "Game Over" + System.Environment.NewLine + "Your Score Is: " + gameState.Score.ToString();
    }

    public void MenuButtonClicked()
    {
        OnMenuButtonClicked?.Invoke();
    }

    private void Start()
    {
        scoreText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);

        gameState = FindObjectOfType<GameManager>().StateOfTheGame;
    }

    private void Update()
    {
        scoreText.text = gameState.Score.ToString();
    }
}
