using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : UIPanelBase
{
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
            case "Continue":
                //关闭暂停面板
                UIManager.Instance.HidePanel<PausePanel>();
                //恢复时间流逝
                TimeSystem.Instance.RecoverTime();
                //恢复玩家移动
                GameManager.Instance.player.EnablePlayerInput();
                break;
            case "Save":
                //保存游戏数据
                //只允许玩家在生活场景中保存游戏
                if(GameManager.Instance.currentSceneName == Setting.BattleScene)
                {
                    UIManager.Instance.ShowPanel<TipPanel>((panel)=>
                    {
                        panel.UpdateInfo("只能在生活场景中保存游戏");
                    });
                }
                else
                {
                    SaveSystemMgr.Instance.Save();
                    UIManager.Instance.ShowPanel<TipPanel>((panel)=>
                    {
                        panel.UpdateInfo("游戏已保存");
                    });
                }
                break;
            case "Quit":
                //给玩家一个提示，保存游戏
                UIManager.Instance.ShowPanel<UsePanel>((panel) =>
                {
                    panel.UpdateInfo("退出游戏会丢失所有进度，请先保存游戏，是否确认回到主菜单?");
                    panel.RegisterOKAction(() =>
                    {
                        //玩家确认退出游戏
                        Application.Quit();
                    });
                    panel.RegisterCancelAction(() =>
                    {
                        //玩家确认取消退出游戏
                        UIManager.Instance.HidePanel<UsePanel>();
                    });
                });
                break;
            case "BackToMenu":
                //给玩家一个提示，保存游戏
                UIManager.Instance.ShowPanel<UsePanel>((panel) =>
                {
                    panel.UpdateInfo("退出游戏会丢失所有进度，请先保存游戏，是否确认回到主菜单?");
                    panel.RegisterOKAction(() =>
                    {
                        //玩家确认回到主菜单
                        GameManager.Instance.DestroyObj();
                        UIManager.Instance.HidePanel<PausePanel>();
                        UIManager.Instance.HidePanel<GameUI>();
                        AStarMgr.Instance.Clear();
                        SceneLoadManager.Instance.LoadScene("StartScene");

                    });
                    panel.RegisterCancelAction(() =>
                    {
                        //玩家确认取消回到主菜单
                        UIManager.Instance.HidePanel<UsePanel>();
                    });
                });
                break;
        }
    }
}
