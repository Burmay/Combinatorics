using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractorsBase
{
    private Dictionary<Type, Interactor> interactorsRoll;
    private SceneConfig sceneConfig;

    public InteractorsBase(SceneConfig sceneConfig)
    {
        this.sceneConfig = sceneConfig;
    }

    // Вызывается первым, отдаёт реализацию SceneConfig
    public void CreateAllInteractors()
    {
        this.interactorsRoll = this.sceneConfig.CreateAllInteractors();
    }


    // запуск OnCreate с задержкой карутины после
    public void SendOnCreateToAllInteractors()
    {
        var allInteractors = this.interactorsRoll.Values;
        foreach(var interactor in allInteractors)
        {
            interactor.OnCreate();
        }
    }

    public void InitializeAllInteractors()
    {
        var allInteractors = this.interactorsRoll.Values;
        foreach (var interactor in allInteractors)
        {
            interactor.Initialize();
        }
    }

    public void SendOnStartToAllInteractors()
    {
        var allInteractors = this.interactorsRoll.Values;
        foreach (var interactor in allInteractors)
        {
            interactor.OnStart();
        }
    }

    // возвращает ссылку на экземляр интерактора, принимая его тип
    public T GetInteractor <T>() where T : Interactor
    {
        var type = typeof(T);
        Debug.Log(type);
        return (T)this.interactorsRoll[type];
    }
}
