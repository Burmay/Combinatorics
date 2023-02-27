using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class SceneManagerBase
{
    public Scene scene { get; private set; }
    public bool isLoading { get; private set; }


    protected Dictionary<string, SceneConfig> sceneConfigRoll;

    public SceneManagerBase()
    {
        this.sceneConfigRoll = new Dictionary<string, SceneConfig>();
    }

    public abstract void InitScenesRoll();

    // событие, активируемое после загрузки сцены. Привязать можно любые события

    public event Action<Scene> OnSceneLoadedEvent;

    public Coroutine LoadCurrentSceneAsync()
    {
        if (this.isLoading)
        {
            throw new Exception("Scene is loading now");
        }
        var sceneName = SceneManager.GetActiveScene().name;
        var config = this.sceneConfigRoll[sceneName];
        return Coroutines.StartRoutine(this.LoadCurrentSceneRoutine(config));
    }

    public Coroutine LoadNewSceneAsync(string sceneName)
    {
        if (this.isLoading)
        {
            throw new Exception("Scene is loading now");
        }
        var config = this.sceneConfigRoll[sceneName];
        return Coroutines.StartRoutine(this.LoadNewSceneRoutine(config));
    }

    // Загрузка новых сцен
    private IEnumerator LoadNewSceneRoutine(SceneConfig sceneConfig)
    {
        this.isLoading = true;

        yield return Coroutines.StartRoutine(this.LoadNewSceneRoutine(sceneConfig));
        yield return Coroutines.StartRoutine(this.InitialiseSceneRoutine(sceneConfig));

        this.isLoading = false;
        this.OnSceneLoadedEvent?.Invoke(this.scene);
    }

    // Загрузка текущей сцены

    private IEnumerator LoadCurrentSceneRoutine(SceneConfig sceneConfig)
    {
        this.isLoading = true;

        yield return Coroutines.StartRoutine(this.InitialiseSceneRoutine(sceneConfig));

        this.isLoading = false;
        this.OnSceneLoadedEvent?.Invoke(this.scene);
    }

    private IEnumerator LoadSceneRoutine(SceneConfig sceneConfig)
    {
        var async = SceneManager.LoadSceneAsync(sceneConfig.sceneName);
        async.allowSceneActivation = false;

        while(async.progress < 0.9)
        {
            yield return null;
        }
        async.allowSceneActivation = true;
    }

    private IEnumerator InitialiseSceneRoutine(SceneConfig sceneConfig)
    {
        this.scene = new Scene(sceneConfig);
        yield return this.scene.InitializeAsync();

    }

    public T GetRepository<T>() where T : Repository
    {
        return this.scene.GetRepository<T>();
    }

    public T GetInteractor<T>() where T : Interactor
    {
        return this.scene.GetInteractor<T>();
    }
}
