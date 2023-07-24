using System;
using System.Collections.Generic;

public class A1CompanyConfig : SceneConfig
{
    public const string SCENE_NAME = "A1Company";

    public override string sceneName => SCENE_NAME;

    public override Dictionary<Type, Interactor> CreateAllInteractors()
    {
        var interactorsRoll = new Dictionary<Type, Interactor>();
        this.CreateInteractor<A1CompanyInteractor>(interactorsRoll);
        this.CreateInteractor<StorageCompanyInteractor>(interactorsRoll);
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
