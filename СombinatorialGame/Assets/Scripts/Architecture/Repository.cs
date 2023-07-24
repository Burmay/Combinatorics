using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Repository
{
    public abstract void Initialize(); // ѕолучение данных от Db
    public abstract void Save(); // подрузка данных в Unity
    public abstract void OnCreate(); 
    public abstract void OnStart();
}
