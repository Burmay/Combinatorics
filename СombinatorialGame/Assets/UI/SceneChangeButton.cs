using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeButton : MonoBehaviour
{
    [SerializeField] string sceneName;
    GameObject loaderTag;
    SceneLoader loader;

    private void Start()
    {
        loaderTag = GameObject.FindWithTag("SceneLoader");
        loader = loaderTag.GetComponent<SceneLoader>();
    }

    public void Click()
    {
        loader.LoadScene(sceneName);
    }
}
