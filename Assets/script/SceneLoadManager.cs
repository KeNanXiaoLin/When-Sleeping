using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 动画转场类型
/// </summary>
public enum E_SceneLoadType
{
    Clock,
    None,
}

public class SceneLoadManager : MonoBase<SceneLoadManager>
{
    public float DayIndex = 0;
    public bool DayOneDia = false;
    public bool DayTwoDia = false;
    public bool MilkDrinked = false;
    public int LoadScene3Index = 0;

    /// <summary>
    /// 标识是否正在进行淡入淡出操作
    /// </summary>
    private bool isFading;
    /// <summary>
    /// 淡入淡出的持续时间
    /// </summary>
    [SerializeField] private float fadeDuration = 1f;
    /// <summary>
    /// 用于淡入淡出效果的 CanvasGroup
    /// </summary>
    private CanvasGroup faderCanvasGroup = null;

    [HideInInspector]public string CurrentScene;
    [HideInInspector]public string CurrentMusic;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        PlayeStartMusic();
    }

    void OnEnable()
    {
        EventListener.OnVilliageSceneChange += LoadScene;
        EventListener.OnVilliageSceneChange += VilliageBackGroundMusic;

        EventListener.OnBattleSceneChange += LoadScene;
        EventListener.OnBattleSceneChange += DayUp;
        EventListener.OnBattleSceneChange += BattleBackGroundMusic;

        EventListener.OnSceneReload += ReLoadScene;
    }

    private void DayUp(string name, E_SceneLoadType type)
    {
        DayIndex++;
    }

    #region music
    private void VilliageBackGroundMusic(string name, E_SceneLoadType type)
    {
        if (CurrentScene == "GameScene3")
        {
            if (CurrentMusic == "轻松小曲2")  return;       //检测当前是否正在播放重复歌曲

            MusicManager.Instance.PlayBKMusic("轻松小曲2");
            CurrentMusic = "轻松小曲2";
        }
        else
        {
            if (CurrentMusic == "轻松小曲1")  return;

            MusicManager.Instance.PlayBKMusic("轻松小曲1");
            CurrentMusic = "轻松小曲1";
        }
    }

    private void BattleBackGroundMusic(string name, E_SceneLoadType type)
    {
        MusicManager.Instance.PlayBKMusic("阴森的小曲1");

        CurrentMusic = "阴森的小曲1";
    }

    private void PlayeStartMusic()
    {
        MusicManager.Instance.PlayBKMusic("阴森的小曲1");
        CurrentMusic = "阴森的小曲1";
    }
    #endregion

    void OnDisable()
    {
        EventListener.OnVilliageSceneChange -= LoadScene;
        EventListener.OnBattleSceneChange -= LoadScene;
    }


    public void LoadScene(string name, E_SceneLoadType type = E_SceneLoadType.None)
    {
        FadeAndLoadScene(name, type);
        MusicManager.Instance.PlaySound("时钟的音效2", false, false);
        CurrentScene = name;

        if (name == "GameScene3") LoadScene3Index++;
    }

    public void ReLoadScene(string name, E_SceneLoadType type = E_SceneLoadType.None)
    {
        FadeAndLoadScene(name, type);
        CurrentScene = name;
        
    }

    /// <summary>
    /// 执行淡入淡出动画的协程
    /// </summary>
    /// <param name="finalAlpha">淡入淡出的最终透明度</param>
    /// <returns>协程迭代器</returns>
    private IEnumerator Fade(float finalAlpha)
    {
        // 设置正在淡入淡出的标志，防止重复调用 FadeAndSwitchScenes 协程
        isFading = true;

        // 确保 CanvasGroup 阻止射线穿透到场景中，避免在淡入淡出时接受输入
        faderCanvasGroup.blocksRaycasts = true;

        // 根据当前透明度、最终透明度和淡入淡出持续时间，计算 CanvasGroup 的淡入淡出速度
        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / fadeDuration;

        // 当 CanvasGroup 的透明度还未达到最终透明度时
        while (!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpha))
        {
            // 逐步将透明度调整到目标值
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, finalAlpha,
                fadeSpeed * Time.deltaTime);

            // 等待一帧后继续执行
            yield return null;
        }

        // 淡入淡出完成，设置标志为 false
        isFading = false;

        // 取消 CanvasGroup 对射线的阻挡，恢复输入响应
        faderCanvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// 执行淡入淡出并切换场景的协程
    /// </summary>
    /// <param name="sceneName">要切换到的场景名称</param>
    /// <param name="spawnPosition">玩家在新场景中的生成位置</param>
    /// <returns>协程迭代器</returns>
    private IEnumerator FadeAndSwitchScenes(string sceneName,E_SceneLoadType type)
    {
        // 开始淡入到黑屏，并等待淡入完成
        yield return StartCoroutine(Fade(1f));

        //在场景加载完成后，做一些额外处理，时钟动画
        yield return DoSomethingAfterLoadScene(type);

        // 开始加载指定场景，并等待加载完成
        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));


        // 开始淡出黑屏，并等待淡出完成
        yield return StartCoroutine(Fade(0f));

        //销毁淡入面板
        UIManager.Instance.HidePanel<FaderPanel>();

    }

    /// <summary>
    /// 在场景加载完成后，做一些额外处理，时钟动画
    /// </summary>
    private IEnumerator DoSomethingAfterLoadScene(E_SceneLoadType type)
    {
        switch (type)
        {
            case E_SceneLoadType.Clock:
                yield return PlayClockAnim(0.3f);
                break;
            case E_SceneLoadType.None:
                yield return null;
                break;
        }
    }

    /// <summary>
    /// 播放时钟动画
    /// </summary>
    /// <param name="time">播放每张图片的时间间隔</param>
    /// <returns></returns>
    private IEnumerator PlayClockAnim(float time)
    {

        List<Sprite> clockSprites = new();
        for (int i = 5; i >= 1; i--)
        {
            clockSprites.Add(Resources.Load<Sprite>("Sprites/Clock" + i));
        }
        UIManager.Instance.ShowPanel<CGPanel>();
        CGPanel gPanel = UIManager.Instance.GetPanel<CGPanel>();
        //从后往前移除列表
        for (int i = clockSprites.Count - 1; i >= 0; i--)
        {
            gPanel.UpdateImage(clockSprites[i]);
            clockSprites.Remove(clockSprites[i]);
            yield return new WaitForSeconds(time);
        }
        UIManager.Instance.HidePanel<CGPanel>();

    }

    /// <summary>
    /// 加载指定场景并将其设置为激活场景的协程
    /// </summary>
    /// <param name="sceneName">要加载的场景名称</param>
    /// <returns>协程迭代器</returns>
    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {

        // 允许指定场景分多帧加载，并将其添加到已加载的场景中（此时只有持久化场景）
        yield return SceneManager.LoadSceneAsync(sceneName);
    }

    /// <summary>
    /// 外部调用的主要接口，用于触发场景切换和淡入淡出效果
    /// </summary>
    /// <param name="sceneName">要切换到的场景名称</param>
    /// <param name="spawnPosition">玩家在新场景中的生成位置</param>
    public void FadeAndLoadScene(string sceneName,E_SceneLoadType type)
    {
        // 如果当前没有进行淡入淡出操作，则开始淡入淡出并切换场景
        if (!isFading)
        {
            UIManager.Instance.ShowPanel<FaderPanel>((panel) =>
            {
                faderCanvasGroup = panel.gameObject.GetComponent<CanvasGroup>();
                faderCanvasGroup.alpha = 0;
                faderCanvasGroup.blocksRaycasts = true;
            });
            StartCoroutine(FadeAndSwitchScenes(sceneName,type));
        }
    }
}
