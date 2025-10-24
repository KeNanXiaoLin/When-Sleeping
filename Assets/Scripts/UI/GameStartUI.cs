using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStartUI : UIPanelBase
{
    private Button btnStart;
    private Button btnSetting;
    private Button btnQuit;

    public override void HideMe()
    {
    }

    public override void ShowMe()
    {
    }

    protected override void ClickBtn(string btnName)
    {
        switch (btnName)
        {
            case "Start":
                //切换场景到游戏场景
                SceneLoadManager.Instance.LoadScene("GameScene3", EnterScene, sceneFaderBefore: InitNewSceneObj, sceneAfterLoad: PlayGameStartPlot);
                break;
            case "Setting":
                //打开设置面板
                UIManager.Instance.ShowPanel<SettingPanelUI>();
                break;
            case "Quit":
                Application.Quit();
                break;
        }
    }

    public IEnumerator EnterScene()
    {
        yield return CGManager.Instance.PlayKaiTouCG();
    }

    public void InitNewSceneObj()
    {
        // DialogSystem.Instance.Test();
        Vector3 spawnPos = GameManager.Instance.playerPos;
        GameObject playerObj = Instantiate(Resources.Load<GameObject>("Player/Player"), spawnPos, Quaternion.identity);
        GameObject playerCamera = Instantiate(Resources.Load<GameObject>("Player/PlayerCamera"));
        DontDestroyOnLoad(playerObj);
        DontDestroyOnLoad(playerCamera);
        Player player = playerObj.GetComponent<Player>();
        CinemachineVirtualCamera camera = playerCamera.GetComponent<CinemachineVirtualCamera>();
        //记录Player,playerCamera，方便访问
        GameManager.Instance.player = player;
        GameManager.Instance.playerCamera = camera;
        GameManager.Instance.InitCameraValues();
        //禁用玩家输入
        player.DisablePlayerInput();
        UIManager.Instance.ShowPanel<GameUI>();
        UIManager.Instance.HidePanel<GameStartUI>();
        //启动时间流逝
        TimeSystem.Instance.RecoverTime();
    }

    public void PlayGameStartPlot()
    {
        //禁用玩家的输入
        GameManager.Instance.player.DisablePlayerInput();
        PlotSystem.Instance.PlayGameStartDialog();
    }



}
