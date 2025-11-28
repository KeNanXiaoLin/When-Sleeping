using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using KNXL.DialogSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStartUI : UIPanelBase
{
    private Button btnStart;
    private Button btnSetting;
    private Button btnQuit;

    private Player player;

    public override void HideMe()
    {
    }

    public override void ShowMe()
    {
        Button btnContinue = GetControl<Button>("Continue");
        btnContinue.gameObject.SetActive(SaveSystemMgr.Instance.IsHaveSaveFile);
    }

    protected override void ClickBtn(string btnName)
    {
        switch (btnName)
        {
            //继续游戏，恢复玩家的数据
            case "Continue":
                SaveSystemMgr.Instance.Load();
                GameManager.Instance.ResetGameData();
                DialogSystemMgr.Instance.Init();
                PlotSystem.Instance.Init();
                //恢复上次游戏数据
                SceneLoadManager.Instance.LoadScene(GameManager.Instance.currentSceneName, sceneFaderBefore: InitNewSceneObj,sceneAfterLoad:ContinueGame);
                break;
            case "Start":
                SaveSystemMgr.Instance.DefaultLoad();
                DialogSystemMgr.Instance.Init();
                //切换场景到游戏场景
                SceneLoadManager.Instance.LoadScene(GameManager.Instance.currentSceneName, CGManager.Instance.PlayKaiTouCG, sceneFaderBefore: InitNewSceneObj, sceneAfterLoad: PlayGameStartPlot);
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

    public void InitNewSceneObj()
    {
        
        Vector3 spawnPos = GameManager.Instance.initPos;
        GameObject playerObj = Instantiate(Resources.Load<GameObject>("Player/Player"), spawnPos, Quaternion.identity);
        GameObject playerCamera = Instantiate(Resources.Load<GameObject>("Player/PlayerCamera"));
        DontDestroyOnLoad(playerObj);
        DontDestroyOnLoad(playerCamera);
        Player player = playerObj.GetComponent<Player>();
        CinemachineVirtualCamera camera = playerCamera.GetComponent<CinemachineVirtualCamera>();
        //记录Player,playerCamera，方便访问
        this.player = player;
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

    private void ContinueGame()
    {
        player.EnablePlayerInput();
    }



}
