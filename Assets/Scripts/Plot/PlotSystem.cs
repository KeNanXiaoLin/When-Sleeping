using System.Collections;
using System.Collections.Generic;
using KNXL.DialogSystem;
using UnityEngine;

public class PlotSystem : SingletonAutoMono<PlotSystem>
{
    void Awake()
    {
        // gameStartDialogData = Resources.Load<RoleDialogData>("PlotData/GameStartDialog");
    }
    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener<int>(E_EventType.E_DialogEnd, CheckPlotCanPlayByDialogID);
        EventCenter.Instance.AddEventListener<string>(E_EventType.E_SceneLoad, CheckPlotCanPlayByChangeScene);
    }

    void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<int>(E_EventType.E_DialogEnd, CheckPlotCanPlayByDialogID);
        EventCenter.Instance.RemoveEventListener<string>(E_EventType.E_SceneLoad, CheckPlotCanPlayByChangeScene);
    }

    public void PlayGameStartDialog()
    {
        DialogSystemMgr.Instance.StartPlayPlotDialog(10001, () =>
        {
            GameManager.Instance.player.EnablePlayerInput();
        });
    }

    /// <summary>
    /// 检测因为对话播放而解锁的剧情事件
    /// </summary>
    /// <param name="dialogId"></param>
    private void CheckPlotCanPlayByDialogID(int dialogId)
    {
        switch (dialogId)
        {
            //这里是玩家和电视剧交互完毕后解锁的剧情
            case 10004:
                //播放敲门声
                MusicManager.Instance.PlaySound("按门铃音效6");
                //播放提示内容
                DialogSystemMgr.Instance.StartPlayPlotDialog(10005, () =>
                {
                    GameManager.Instance.player.EnablePlayerInput();
                });
                break;
        }
    }

    private void CheckPlotCanPlayByItemID(int itemID)
    {
        switch (itemID)
        {
            case 1:
                break;
        }
    }

    private void CheckPlotCanPlayByChangeScene(string sceneName)
    {
        switch (sceneName)
        {
            case "GameScene":
                //玩家在切换到场景1的时候，可以播放和Bob对话的剧情，前提是剧情没有被触发过，并且剧情的前置已经解锁
                RoleDialogData plotData = DialogSystemMgr.Instance.GetPlotByID(10006);
                if (!plotData.isTrigger &&
                    DialogSystemMgr.Instance.GetPlotByID(plotData.preRoleDialogs).isTrigger)
                {
                    //实例化一个Bob出来和玩家模拟对话
                    GameObject BobPrefab = Resources.Load<GameObject>("NPC/Bob");
                    GameObject.Instantiate(BobPrefab);
                    DialogSystemMgr.Instance.StartPlayPlotDialog(10006);
                }
                break;
        }
    }
}
