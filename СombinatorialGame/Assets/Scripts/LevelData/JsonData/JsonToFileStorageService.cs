using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class JsonToFileStorageService : IStorageService
{
    private static JsonToFileStorageService _instance;
    public JsonToFileStorageService instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = this;
            }
            return _instance;   
        }
    }

    public void Save(string key, object data, Action<bool> callback = null)
    {
        string path = BuildPath(key);
        string json = JsonConvert.SerializeObject(data);

        using (StreamWriter fileStream = new StreamWriter(path))
        {
            fileStream.Write(json);
        }

        callback?.Invoke(true);
    }

    public void Load<T>(string key, Action<T> callback)
    {
        string path = BuildPath(key);
        
        using(StreamReader fileStream = new StreamReader(path))
        {
            var json = fileStream.ReadToEnd();
            var data = JsonConvert.DeserializeObject<T>(json);
            
            callback.Invoke(data);
        }
    }

    public string BuildPath(string key)
    {
        return Path.Combine(Application.persistentDataPath, key) ;
    }
}