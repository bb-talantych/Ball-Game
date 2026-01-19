using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance
    { get; private set; }

    [SerializeField]
    private string firstScene;

    string currentScene;

    public static event Action<string, bool> Event_SceneLoaded;

    private void Awake()
    {
        Instance = this;
        GameManager.Event_ResetGame += OnResetGame;   
    }
    private void OnDestroy()
    {
        GameManager.Event_ResetGame -= OnResetGame;
    }

    private void Start()
    {
        if(firstScene != null)
            LoadScene(firstScene);
    }

    public void LoadScene(string _scene, bool _isReload = false)
    {
        StartCoroutine(ILoadScene(_scene, _isReload));
    }

    IEnumerator ILoadScene(string _scene, bool _isReload)
    {
        // Start Unloading previous Scene
        if (currentScene != null)
        {
            AsyncOperation unloadSceneAsync =
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currentScene);

            while (!unloadSceneAsync.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        // Start Loading New Scene
        AsyncOperation loadSceneAsync =
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_scene, LoadSceneMode.Additive);

        while (!loadSceneAsync.isDone)
        {
            yield return new WaitForEndOfFrame();
        }

        currentScene = _scene;
        Event_SceneLoaded?.Invoke(currentScene, _isReload);
    }

    void OnResetGame(object obj, EventArgs e)
    {
        Debug.Log("Reloading Level");
        if(currentScene != null) 
        {
            LoadScene(currentScene, true);   
        }
    }
}
