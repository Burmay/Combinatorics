using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStorageService
{
    void Save(string key, object data, Action<bool> callback = null);
    void Load<T>(string key, Action<T> callback);
}
