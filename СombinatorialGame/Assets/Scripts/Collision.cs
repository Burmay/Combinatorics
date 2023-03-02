using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    [SerializeField] private GameManager _manager;
    private Element _firePrefab;
    private Element _waterPrefab;
    private Element _stonePrefab;
    [SerializeField] private Element _fireTwoPrefab;
    [SerializeField] private Element _waterTwoPrefab;
    [SerializeField] private Element _stomeTwoPrefab;
    [SerializeField] private Element _lavaPrefab;
    [SerializeField] private Element _steamPrefab;
    [SerializeField] private Element _platnPrefab;

    public void SetElementsPrefab(Element fire, Element water, Element stone)
    {
        _firePrefab = fire;
        _waterPrefab = water;
        _stonePrefab = stone;
    }

    // Проверка, есть ли вообще столкновение

    public bool CollisionResult(Block incoming, Block standing, int maxOrderElement)
    {
        if (incoming is Element && standing is Element) { return CollisionElementsResult(incoming, standing, maxOrderElement); }
        if (incoming is Player && standing is Enemy) { return CollosionEnemyWithPlayer(incoming); }
        if (incoming is Enemy && standing is Player) { return CollosionEnemyWithPlayer(standing); }
        if (incoming is Element && !(standing is Element)) { return CollisionUnitWithElement(standing, incoming); }
        if (standing is Element && !(incoming is Element)) { return CollisionUnitWithElement(incoming, standing); }
        if (incoming is Enemy && standing is Enemy) { return false; }
        else { Debug.Log("Произошла странная коллизия"); return false; } // дебаг-строчка
    }

    private bool CollisionElementsResult(Block incoming, Block standing, int maxOrderElement)
    {
        Element el1 = incoming as Element;
        Element el2 = standing as Element;

        if (el1.orderElement == el2.orderElement && el1.orderElement != maxOrderElement)
        {
            if (GetElementsType(el1, el2) != Elements.Null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else { return false; }
    }

    public bool CollosionEnemyWithPlayer(Block player)
    {
        return true;
    }

    public bool CollisionUnitWithElement(Block suffering, Block element)
    {
        Character character = suffering as Character;
        Element el = element as Element;
        if (el.orderElement < 2)
        {
            return false;
        }
        else
        {
            if (el.type == Elements.Stone || el.type == Elements.Lava) { return false; }
            else { return true; }
        }
    }

    // Обработка коллизии - выбор сценария 

    public void MergeBlocks(Block standing, Block incoming)
    {
        if (standing is Element && incoming is Element) { MegreElements(incoming, standing); }
        else if (standing is Character && incoming is Element || incoming is Character && standing is Element) { MergeElementWithUnit(standing, incoming); }
        else if (standing is Character && incoming is Character) { MergeUnit(standing, incoming); }
    }

    // Определение резльтата столкновения двух элементов и спавн нового типа

    private void MegreElements(Block incoming, Block standing)
    {
        _manager.SpawnElement(standing.node, GetElementsPrefab(standing, incoming));
        _manager.RemoveBlock(standing);
        _manager.RemoveBlock(incoming);
    }

    private Elements GetElementsType(Block el1, Block el2)
    {
        Element element1 = el1 as Element;
        Element element2 = el2 as Element;
        if (element1.type == Elements.Fire && element2.type == Elements.Fire) { return Elements.Fire; }
        else if (element1.type == Elements.Water && element2.type == Elements.Water) { return Elements.Water; }
        else if (element1.type == Elements.Stone && element2.type == Elements.Stone) { return Elements.Stone; }
        ///
        else if (element1.type == Elements.Fire && element2.type == Elements.Water || element1.type == Elements.Water && element2.type == Elements.Fire) { return Elements.Steam; }
        else if (element1.type == Elements.Fire && element2.type == Elements.Stone || element1.type == Elements.Stone && element2.type == Elements.Fire) { return Elements.Lava; }
        else if (element1.type == Elements.Stone && element2.type == Elements.Water || element1.type == Elements.Water && element2.type == Elements.Stone) { return Elements.Plant; }
        ///
        else { return Elements.Null; }
    }

    public Element GetElementsPrefab(Block el1, Block el2)
    {
        Element element1 = el1 as Element;
        Element element2 = el2 as Element;
        Elements elements = GetElementsType(el1, el2);

        if (elements == Elements.Fire && element1.orderElement == 1) { return _fireTwoPrefab; }
        else if (elements == Elements.Water && element1.orderElement == 1) { return _waterTwoPrefab; }
        else if (elements == Elements.Stone && element1.orderElement == 1) { return _stomeTwoPrefab; }
        else if (elements == Elements.Lava && element1.orderElement == 1) { return _lavaPrefab; }
        else if (elements == Elements.Steam && element1.orderElement == 1) { return _steamPrefab; }
        else if(elements == Elements.Plant && element1.orderElement == 1) { return _platnPrefab; }
        else { return null; }
    }

    // Определение резльтата столкновения двух Элемента и Юнита

    private void MergeElementWithUnit(Block standing, Block incoming)
    {
        Element element; Character unit;
        if (standing is Element) { element = standing as Element; unit = incoming as Character; } else { element = incoming as Element; unit = standing as Character; }
        if (element.type == Elements.Fire) { FireEffect(unit); } 
        else if (element.type == Elements.Plant) { PlantEffect(unit); }
        else if (element.type == Elements.Water) { WaterEffect(unit); }
        else if (element.type == Elements.Steam) { SteamEffect(unit); }

        _manager.RemoveBlock(element);
    }

    // Результат столкновение врага с юнитом
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
        //???
    }

    private void SteamEffect(Character unit)
    {
        // CD
    }

    private void PlantEffect(Character unit)
    {
        // оплетение
    }
}
