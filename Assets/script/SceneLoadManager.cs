using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBase<SceneLoadManager>
{
    public float DayIndex = 0;
    public bool DayOneDia = false;
    public bool DayTwoDia = false;
    public bool MilkDrinked = false;

    public string CurrentScene;

    protected override void Awake()
    {
        base.Awake();

        EventListener.OnVilliageSceneChange += LoadScene;

        EventListener.OnBattleSceneChange += LoadScene;
        EventListener.OnBattleSceneChange += DayUp;

        EventListener.OnSceneReload += ReLoadScene;
    }

    private void DayUp(string name)
    {
        DayIndex++;
    }


    public void LoadScene(string name)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);

        CurrentScene = name;
    }

    public void ReLoadScene(string name)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        CurrentScene = name;
    }

    private IEnumerator LoadSceneCoroutine(string name)
    {
        yield return SceneManager.LoadSceneAsync(name);
    }
}
