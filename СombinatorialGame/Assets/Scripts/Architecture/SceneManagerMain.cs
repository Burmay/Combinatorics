using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerMain : SceneManagerBase
{
    public override void InitScenesRoll()
    {
        this.sceneConfigRoll[FreeSceneConfig.SCENE_NAME] = new FreeSceneConfig();
        this.sceneConfigRoll[A1CompanyConfig.SCENE_NAME] = new A1CompanyConfig();
    }
}