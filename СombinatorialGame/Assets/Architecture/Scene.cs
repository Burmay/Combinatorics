using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene
{
    // Хранение всех ссылок
    private InteractorsBase interactorsBase;
    private RepositoriesBase repositoriesBase;
    private SceneConfig sceneConfig;

    public Scene(SceneConfig sceneConfig)
    {
        this.sceneConfig = sceneConfig;
        this.interactorsBase = new InteractorsBase(sceneConfig);
        this.repositoriesBase = new RepositoriesBase(sceneConfig);
    }

    public Coroutine InitializeAsync()
    {
        return Coroutines.StartRoutine(this.InitializeRoutine());
    }

    public IEnumerator InitializeRoutine()
    {
        interactorsBase.CreateAllInteractors();
        repositoriesBase.CreateAllRepositories();
        yield return null;

        Debug.Log("1. All interactors / repositories are created");
        interactorsBase.SendOnCreateToAllInteractors();
        repositoriesBase.SendOnCreateToAllRepository();
        yield return null;
        Debug.Log("2. Actions in the creation are performed");
        interactorsBase.InitializeAllInteractors();
        repositoriesBase.InitializeAllRepository();
        yield return null;
        Debug.Log("3. Interactors / repositories are initialize");
        interactorsBase.SendOnStartToAllInteractors();
        Debug.Log("4. Interactors work");
        repositoriesBase.SendOnStartAllRepository();
        Debug.Log("5. Repositories work");

    }

    public T GetRepository<T>() where T : Repository
    {
        return this.repositoriesBase.GetRepository<T>();
    }

    public T GetInteractor<T>() where T : Interactor
    {
        return this.interactorsBase.GetInteractor<T>();
    }
}
