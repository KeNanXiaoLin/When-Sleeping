using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadManager : MonoBase<SceneLoadManager>
{

    public float DayIndex = 0;
    public bool MilkDrinked = false;

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

    protected override void Awake()
    {
        base.Awake();
    }

    void OnEnable()
    {
        EventListener.OnVilliageSceneChange += LoadScene;
        EventListener.OnBattleSceneChange += LoadScene;
    }

    void OnDisable()
    {
        EventListener.OnVilliageSceneChange -= LoadScene;
        EventListener.OnBattleSceneChange -= LoadScene;
    }


    public void LoadScene(string name)
    {
        FadeAndLoadScene(name);
    }

    public void ReLoadScene(string name)
    {
        FadeAndLoadScene(name);
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
    private IEnumerator FadeAndSwitchScenes(string sceneName)
    {
        // 开始淡入到黑屏，并等待淡入完成
        yield return StartCoroutine(Fade(1f));

        // 开始加载指定场景，并等待加载完成
        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));

        // 开始淡出黑屏，并等待淡出完成
        yield return StartCoroutine(Fade(0f));

        //销毁淡入面板
        UIManager.Instance.HidePanel<FaderPanel>();

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
    public void FadeAndLoadScene(string sceneName)
    {
        // 如果当前没有进行淡入淡出操作，则开始淡入淡出并切换场景
        if (!isFading)
        {
            UIManager.Instance.ShowPanel<FaderPanel>((panel)=>
            {
                faderCanvasGroup = panel.gameObject.GetComponent<CanvasGroup>();
                faderCanvasGroup.alpha = 0;
                faderCanvasGroup.blocksRaycasts = true;
            });
            StartCoroutine(FadeAndSwitchScenes(sceneName));
        }
    }
}
