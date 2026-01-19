using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool gameOver = false;

    public static EventHandler Event_ResetGame;

    [SerializeField]
    private GameObject gameOverScreen;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip mainMenuClip;
    [SerializeField]
    private AudioClip gameClip;

    private void Awake()
    {
        Enemy.Event_PlayerTouched += OnPlayerTouched;
        SceneManager.Event_SceneLoaded += OnSceneLoaded;

        gameOverScreen.SetActive(false);
    }

    private void OnDestroy()
    {
        Enemy.Event_PlayerTouched -= OnPlayerTouched;
        SceneManager.Event_SceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if(gameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    void OnPlayerTouched(object obj, EventArgs e)
    {
        Debug.Log(obj + " touched player");

        gameOver = true;
        Time.timeScale = 0;
        gameOverScreen.SetActive(true);
    }

    private void RestartGame()
    {
        Debug.Log("Restart Game");

        Event_ResetGame?.Invoke(this, EventArgs.Empty);

        gameOver = false;
        Time.timeScale = 1;
        gameOverScreen.SetActive(false);
    }

    void OnSceneLoaded(string _level, bool _isReload)
    {
        if (_isReload)
            return;

        if (_level == "MainMenuScene")
        {
            audioSource.clip = mainMenuClip;
            audioSource.loop = true;
            audioSource.Play();
        }
        else if (_level == "SampleScene")
        {
            audioSource.clip = gameClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
