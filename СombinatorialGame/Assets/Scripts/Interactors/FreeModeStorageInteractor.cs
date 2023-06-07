using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeModeStorageInteractor : Interactor
{
    private int elementsPerStroke;
    private int maxOrederElemet;
    private ConditionExitLvl _conditionExit;
    private float _probabilityFireEl, _probabilityStoneEl, _probabilityWaterEl;

    public List<Block> BlocksLis;
    public List<Node> NodesList;

}
