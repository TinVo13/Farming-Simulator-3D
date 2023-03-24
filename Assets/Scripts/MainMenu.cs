using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenu : MonoBehaviour
{
    public Button loadGameButton;

    // Start is called before the first frame update
    void Start()
    {
        loadGameButton.interactable = SaveManager.HasSave();
    }

    public void NewGame()
    {
        /*SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);*/
        /*  SceneManager.LoadScene(SceneTransitionManager.Location.PlayerHome.ToString());*/
        StartCoroutine(LoadGameAsync(SceneTransitionManager.Location.PlayerHome, null));
    }
    public void Continue()
    {
        StartCoroutine(LoadGameAsync(SceneTransitionManager.Location.PlayerHome, LoadGame));
    }
    public void Setting()
    {

    }

    void LoadGame()
    {
        if(GameStateManager.Instance == null)
        {
            Debug.LogError("Cannot find Game State Manager");
            return;
        }
        GameStateManager.Instance.LoadSave();
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator LoadGameAsync(SceneTransitionManager.Location scene, Action onFirstFrameLoad)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.ToString());
        DontDestroyOnLoad(gameObject);
        while (!asyncLoad.isDone)
        {
            yield return null;
            Debug.Log("Loading!");
        }

        //Scene load
        Debug.Log("Loaded!");

        yield return new WaitForEndOfFrame();
        Debug.Log("First frame is loaded");
        onFirstFrameLoad?.Invoke();

        Destroy(gameObject);
    }

    public void MoveToStartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
