using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class SceneConfig
{
    // Создатель всех экземпляров
    public abstract Dictionary<Type, Repository> CreateAllRepositories();
    public abstract Dictionary<Type, Interactor> CreateAllInteractors();
    
    public abstract string sceneName { get;}

    public void CreateInteractor<T>(Dictionary<Type, Interactor> interactorsRoll) where T: Interactor, new()
    {
        var interactor = new T();
        var type = typeof(T);

        interactorsRoll[type] = interactor;
    }

    public void CreateRepository<T>(Dictionary<Type, Repository> repositoriesRoll) where T : Repository, new()
    {
        var repository = new T();
        var type = typeof(T);

        repositoriesRoll[type] = repository;
    }

}
