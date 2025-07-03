using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{

    public enum GameState { MainMenu, Paused, Playing, GameOver, GameWon};
    public GameState currentState;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI lifeText;
    public Image redKeyUI, blueKeyUI, yellowKeyUI;
    public GameObject allGameUI, mainMenuPanel, pauseMenuPanel, gameOverPanel, titleText, wonPanel;

    // Start is called before the first frame update
    private void Awake()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            CheckGameState(GameState.MainMenu);
        }
        else
        {
            CheckGameState(GameState.Playing);
        }
    }

    public void CheckGameState(GameState newGameState)
    {
        currentState = newGameState;
        switch (currentState)
        {
            case GameState.MainMenu:
                MainMenuSetup();
                break;
            case GameState.Paused:
                GamePaused();
                Manager.gamePaused = true;
                Time.timeScale = 0f; //game is paused and time stops
                break;
            case GameState.Playing:
                GameActive();
                Manager.gamePaused = false;
                Time.timeScale = 1f;
                break;
            case GameState.GameOver:
                GameOver();
                Manager.gamePaused = true;
                Time.timeScale = 0f;
                break;
            case GameState.GameWon:
                GameWon();
                Manager.gamePaused = true;
                Time.timeScale = 0f;
                break;
        }
    }

    public void MainMenuSetup()
    {
        allGameUI.SetActive(false);
        mainMenuPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        titleText.SetActive(true);
    }

    public void GameActive()
    {
        allGameUI.SetActive(true);
        mainMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        titleText.SetActive(false);
    }

    public void GamePaused()
    {
        allGameUI.SetActive(true);
        mainMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        titleText.SetActive(true);
    }

    public void GameOver()
    {
        allGameUI.SetActive(false);
        mainMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        titleText.SetActive(true);
    }

    public void GameWon()
    {
        allGameUI.SetActive(false);
        mainMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        titleText.SetActive(true);
        wonPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputs();
    }

    void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // escape is pressed
        {
            if(currentState == GameState.Playing) //if game is playing, pause it
            {
                CheckGameState(GameState.Paused);
            }
            else if (currentState == GameState.Paused) //if already paused, playing
            {
                CheckGameState(GameState.Playing);
            }
        }
    }

    //button setup
    public void StartGame() 
    {
        SceneManager.LoadScene("Level01");
        CheckGameState(GameState.Playing);
    }

    public void PauseGame()
    {
        CheckGameState(GameState.Paused);
    }

    public void ResumeGame()
    {
        CheckGameState(GameState.Playing);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        CheckGameState(GameState.MainMenu);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UpdateCoins()
    {
        coinText.text = Manager.coins.ToString();
    }

    public void UpdateLives()
    {
        lifeText.text = Manager.lives.ToString();
    }

    public void UpdateKeys(Manager.DoorKeyColours keyColours)
    {
        switch (keyColours)
        {
            case Manager.DoorKeyColours.Red:
                redKeyUI.GetComponent<Image>().color = Color.red;
                break;
            case Manager.DoorKeyColours.Blue:
                blueKeyUI.GetComponent<Image>().color = Color.blue;
                break;
            case Manager.DoorKeyColours.Yellow:
                yellowKeyUI.GetComponent<Image>().color = Color.yellow;
                break;
        }
    }
}
