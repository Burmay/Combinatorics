using UnityEngine;
using UnityEngine.SceneManagement;

public class EntryPoint : MonoBehaviour
{
    const string _MENUSCENE = "MainMenu";
    private void Awake()
    {
        SceneManager.LoadScene(_MENUSCENE);
    }
}
