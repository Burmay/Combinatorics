using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactor : MonoBehaviour
{
    public virtual void OnCreate(){ } // когда все созданы

    public virtual void Initialize() { } // когда все сделали OnCreate

    public virtual void OnStart() { } // когда все проинициализированы
}
