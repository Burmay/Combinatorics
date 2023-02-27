using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerMain : SceneManagerBase
{
    public override void InitScenesRoll()
    {
        this.sceneConfigRoll[FirstSceneConfig.SCENE_NAME] = new FirstSceneConfig();
    }
}