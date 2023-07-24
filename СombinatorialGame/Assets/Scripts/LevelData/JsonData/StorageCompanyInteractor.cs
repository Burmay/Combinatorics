using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening.Plugins.Core.PathCore;

public class StorageCompanyInteractor : Interactor
{
    Action<bool> _savingIsComplete;
    Action<bool> _loadIsCopmlete;
    public StorageCompanyItem _storageCompany { get; private set; }
    GameManager _gameManager;

    public static event Action LoadStartEvent;
    public static event Action LoadOnEvent;


    JsonToFileStorageService _storageService;
    const string COMPANY_KEY = "CompanyData";

    public override void OnCreate()
    {
        _storageService = new JsonToFileStorageService();
        _storageCompany = new StorageCompanyItem();
        LoadStartEvent += LoadCompanyData;
    }

    public void GetLink(GameManager manager)
    {
        this._gameManager = manager;
        LoadStartEvent.Invoke();
    }

    public void UpdateData(int value)
    {
        _storageCompany.MaxLevel = value;
        Save();
    }

    public void LoadCompanyData()
    {
        if (!File.Exists(_storageService.BuildPath(COMPANY_KEY))) { _gameManager.SetStartLevel(); }
        else { _storageService.Load<StorageCompanyItem>(COMPANY_KEY, ReturnData); }
        LoadStartEvent -= LoadCompanyData;
        LoadOnEvent.Invoke();
    }

    private void ReturnData(StorageCompanyItem data)
    {
        _storageCompany.MaxLevel = data.MaxLevel;
        if (!_gameManager._isFree) _gameManager.SetLevel(_storageCompany);
    }

    private void Save()
    {
        _storageService.Save(COMPANY_KEY, _storageCompany, _savingIsComplete);
    }

}