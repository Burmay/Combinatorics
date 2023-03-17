using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private GameManager _manager;
    private Element _fireOnePrefab, _waterOnePrefab, _stoneOnePrefab, _fireTwoPrefab, _waterTwoPrefab, _stomeTwoPrefab, _lavaPrefab, _steamPrefab, _plantPrefab;
    System.Random random = new System.Random();

    public void SetElementsPrefab(Element fire, Element water, Element stone, Element fireTwo, Element waterTwo, Element stoneTwo, Element lava, Element steam, Element plant)
    {
        _fireOnePrefab = fire;
        _waterOnePrefab = water;
        _stoneOnePrefab = stone;
        _fireTwoPrefab = fireTwo;
        _waterTwoPrefab = waterTwo;
        _stomeTwoPrefab = stoneTwo;
        _lavaPrefab = lava;
        _steamPrefab = steam;
        _plantPrefab = plant;

    }

    // Проверка, есть ли вообще столкновение

    public bool CollisionResult(Block incoming, Block standing, int maxOrderElement)
    {
        if (incoming is Element && standing is Element) { return CollisionElementsResult(incoming, standing, maxOrderElement); }
        else if (incoming is Player && standing is Enemy) { return CollosionEnemyWithPlayer(incoming); }
        else if (incoming is Enemy && standing is Player) { return CollosionEnemyWithPlayer(standing); }
        else if (incoming is Enemy && standing is Enemy) { return false; }
        else if (incoming is Player && standing is Teleport) { return CollisionWithTeport(standing); }
        else if (incoming is Teleport && standing is Player) { return CollisionWithTeport(incoming); }
        else if (incoming is Teleport && !(standing is Player) || standing is Teleport && !(incoming is Player)) { return false; }
        else if (incoming is Element && !(standing is Element)) { return CollisionUnitWithElement(standing, incoming); }
        else if (standing is Element && !(incoming is Element)) { return CollisionUnitWithElement(incoming, standing); }
        else { Debug.Log("Произошла странная коллизия " + incoming + " " + standing); return false; } // дебаг-строчка
    }

    private bool CollisionElementsResult(Block incoming, Block standing, int maxOrderElement)
    {
        Element el1 = incoming as Element;
        Element el2 = standing as Element;

        if (el1.type == BlockType.Lava || el2.type == BlockType.Lava)
        {
            if (el1.type == BlockType.Lava && el2.type == BlockType.Lava) { return false; }
            else { return true; }
        }
        else if (el1.orderElement == el2.orderElement && el1.orderElement != maxOrderElement)
        {
            if (GetElementsType(el1, el2) != BlockType.Null) { return true; } else { return false; }
        }
        else
        {
            return false;
        }
    }

    private bool CollosionEnemyWithPlayer(Block block)
    {
        Player player = block as Player;
        if (player.Shield) { player.ShieldOff(); return false; }
        else return true;
    }

    private bool CollisionUnitWithElement(Block suffering, Block element)
    {
        Character character = suffering as Character;
        Element el = element as Element;
        if (el.orderElement < 2)
        {
            return false;
        }
        else
        {
            if (el.type == BlockType.Stone || el.type == BlockType.Lava) { return false; }
            else { return true; }
        }
    }

    private bool CollisionWithTeport(Block block)
    {
        Teleport teleport = block as Teleport;
        return teleport.Condition;
    }

    // Обработка коллизии - выбор сценария 

    public void MergeBlocks(Block standing, Block incoming)
    {
        if (standing is Element && incoming is Element) { MegreElements(incoming as Element, standing as Element); }
        else if (standing is Character && incoming is Element || incoming is Character && standing is Element) { MergeElementWithUnit(standing, incoming); }
        else if (standing is Character && incoming is Character) { MergeUnit(standing, incoming); }
        else if (incoming is Teleport || standing is Teleport) { _manager.ChangeState(GameState.Win); }
    }

    // Определение резльтата столкновения двух элементов и спавн нового типа

    private void MegreElements(Element incoming, Element standing)
    {
        if (incoming.type == BlockType.Lava || standing.type == BlockType.Lava) { LavaDestroyElement(incoming, standing); }
        else
        {
            _manager.SpawnElement(standing.node, GetNewElementsPrefab(standing, incoming));
            _manager.RemoveBlock(standing);
            _manager.RemoveBlock(incoming);
        }
    }

    private void LavaDestroyElement(Element incoming, Element standing)
    {
        if (incoming.type == BlockType.Lava) { _manager.RemoveBlock(standing); }
        else { _manager.RemoveBlock(incoming); }
    }

    private BlockType GetElementsType(Block el1, Block el2)
    {
        Element element1 = el1 as Element;
        Element element2 = el2 as Element;
        if (element1.type == BlockType.Fire && element2.type == BlockType.Fire) { return BlockType.Fire; }
        else if (element1.type == BlockType.Water && element2.type == BlockType.Water) { return BlockType.Water; }
        else if (element1.type == BlockType.Stone && element2.type == BlockType.Stone) { return BlockType.Stone; }
        ///
        else if (element1.type == BlockType.Fire && element2.type == BlockType.Water || element1.type == BlockType.Water && element2.type == BlockType.Fire) { return BlockType.Steam; }
        else if (element1.type == BlockType.Fire && element2.type == BlockType.Stone || element1.type == BlockType.Stone && element2.type == BlockType.Fire) { return BlockType.Lava; }
        else if (element1.type == BlockType.Stone && element2.type == BlockType.Water || element1.type == BlockType.Water && element2.type == BlockType.Stone) { return BlockType.Plant; }
        ///
        else { return BlockType.Null; }
    }

    public Element GetNewElementsPrefab(Block el1, Block el2)
    {
        Element element1 = el1 as Element;
        Element element2 = el2 as Element;
        BlockType elements = GetElementsType(el1, el2);

        if (elements == BlockType.Fire && element1.orderElement == 1) { return _fireTwoPrefab; }
        else if (elements == BlockType.Water && element1.orderElement == 1) { return _waterTwoPrefab; }
        else if (elements == BlockType.Stone && element1.orderElement == 1) { return _stomeTwoPrefab; }
        else if (elements == BlockType.Lava && element1.orderElement == 1) { return _lavaPrefab; }
        else if (elements == BlockType.Steam && element1.orderElement == 1) { return _steamPrefab; }
        else if(elements == BlockType.Plant && element1.orderElement == 1) { return _plantPrefab; }
        else { return null; }
    }

    // Определение резльтата столкновения двух Элемента и Юнита

    private void MergeElementWithUnit(Block standing, Block incoming)
    {
        Element element; Character unit;
        if (standing is Element) { element = standing as Element; unit = incoming as Character; } else { element = incoming as Element; unit = standing as Character; }
        if (element.type == BlockType.Fire) { FireEffect(unit); } 
        else if (element.type == BlockType.Plant) { PlantEffect(unit); }
        else if (element.type == BlockType.Water) { WaterEffect(unit); }
        else if (element.type == BlockType.Steam) { SteamEffect(unit); }

        
        _manager.RemoveBlock(element);
    }

    // Результат столкновения врага с юнитом
    private void MergeUnit(Block standing, Block incoming)
    {
        Player player;
        if(standing is Player) { player = standing as Player; } else { player = incoming as Player; }
        player.SubtractHP(1);
    }

    // эффекты от столкновения с элементами

    private void WaterEffect(Character unit)
    {
        if (unit.Shield) { }
        else
        {
            unit.ShieldOn();
        }
    }

    private void FireEffect(Character unit)
    {
        unit.SubtractHP(int.MaxValue);
    }

    private void LavaEffect(Character unit)
    {
        unit.SubtractHP(int.MaxValue);
    }

    private void SteamEffect(Character unit)
    {
        Debug.Log("Отражение");
        // возврат в обратную сторону
        _manager.SteamMove(unit);
    }

    private void PlantEffect(Character unit)
    {
        unit.StunOn();
    }
}
