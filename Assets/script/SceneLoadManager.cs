using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private static SceneLoadManager _instance;
    public static SceneLoadManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new();
            }
            return _instance;
        }
    }

    [SerializeField] public GameObject WinScene;
    [SerializeField] public GameObject LoseScene;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        EventListener.OnVilliageSceneChange += LoadScene;
        EventListener.OnBattleSceneChange += LoadScene;
    }


    public void LoadScene(string name)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
    }

    private IEnumerator LoadSceneCoroutine(string name)
    {
        yield return SceneManager.LoadSceneAsync(name);
    }
}
