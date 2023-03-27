using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeSceneConfig : SceneConfig
{
    public const string SCENE_NAME = "FreeMode";

    public override string sceneName => SCENE_NAME;

    // «десь создаютс€ все экземпл€ры классов дл€ конктерной сцены, списком
    public override Dictionary<Type, Interactor> CreateAllInteractors()
    {
        var interactorsRoll = new Dictionary<Type, Interactor>();
        this.CreateInteractor<FreeModeInteractor>(interactorsRoll);
        // Int

        return interactorsRoll;
    }

    public override Dictionary<Type, Repository> CreateAllRepositories()
    {
        var repositoriesRoll = new Dictionary<Type, Repository>();

        // Repo

        return repositoriesRoll;
    }

}
