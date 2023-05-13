using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LoaderRecords : MonoBehaviour
{
    [SerializeField] Transform contentContainer;

    private const string FADER_PATH = "RecordLine";

    public void Start()
    {
        var prefab = Resources.Load<GameObject>(FADER_PATH);

        JsonToFileStorageService storageService = new JsonToFileStorageService();
        StorageItemFreeMode e = new StorageItemFreeMode();
        storageService.Load<StorageItemFreeMode>("FreeModeKey", data => { Debug.Log($"Loaded. int : {e.Round}"); });

        int countResults = 10; // plug

        for ( int i = 0; i < countResults; i++)
        {
            var item = Instantiate(prefab);
            item.GetComponentInChildren<Text>().text = "plug";
            item.transform.SetParent(contentContainer);
            item.transform.localScale = Vector2.one;

        }

    }
}
