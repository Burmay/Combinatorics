using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private bool _isLoading;

    public static SceneLoader instance { get; private set; }

    private void Awake()
    {
        if( instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(this.gameObject);
    }

    public void LoadScene(string sceneName)
    {
        if(_isLoading) return;

        string curruntSceneName = SceneManager.GetActiveScene().name;
        if (curruntSceneName == sceneName) throw new Exception("You are trying to load alredy loaded scene");
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        _isLoading = true;

        bool waitFading = true;
        Fader.instance.FadeIn(() => waitFading = false);

        while (waitFading)
        {
            yield return null;
        }

        var async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        while(async.progress < 0.9f)
        {
            yield return null;
        }

        async.allowSceneActivation = true;

        waitFading = true;
        Fader.instance.FadeOut(() => waitFading = false);

        while (waitFading)
        {
            yield return null;
        }

        _isLoading = false;
    }
}
