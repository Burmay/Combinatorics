using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeModeInteractor : Interactor
{
    private GameManager manager;

    public override void OnStart()
    {
        manager =  GameObject.Find("GameManager").GetComponent<GameManager>();
        manager.ChangeState(GameState.GenerateLvl);
    }
}
