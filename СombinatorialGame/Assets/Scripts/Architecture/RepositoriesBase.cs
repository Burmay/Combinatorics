using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RepositoriesBase
{
    private Dictionary<Type, Repository> repositoriesRoll;
    private SceneConfig sceneConfig;

    public RepositoriesBase(SceneConfig sceneConfig)
    {
        this.sceneConfig = sceneConfig;
    }

    //Вызывается первым, отдаёт реализацию SceneConfig
    public void CreateAllRepositories()
    {
        this.repositoriesRoll = this.sceneConfig.CreateAllRepositories();
    }


    //Инициализирует каждый из объявленных классов. Вызывается вторым
    public void SendOnCreateToAllRepository()
    {
        var allRepository = this.repositoriesRoll.Values;
        foreach (var repository in allRepository)
        {
            repository.OnCreate();
        }
    }

    public void InitializeAllRepository()
    {
        var allRepository = this.repositoriesRoll.Values;
        foreach (var repository in allRepository)
        {
            repository.Initialize();
        }
    }

    public void SendOnStartAllRepository()
    {
        var allRepository = this.repositoriesRoll.Values;
        foreach (var repository in allRepository)
        {
            repository.OnStart();
        }
    }

    // возвращает ссылку на экземляр интерактора, принимая его тип
    public T GetRepository<T>() where T : Repository
    {
        var type = typeof(T);
        return (T)this.repositoriesRoll[type];
    }
}
