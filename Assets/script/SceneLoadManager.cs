using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBase<SceneLoadManager>
{
   
    protected override void Awake()
    {
        base.Awake();

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
