using System.Collections;
using System.Collections.Generic;
using KNXL.DialogSystem;
using UnityEngine;

public class CheatPanel : UIPanelBase
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
            case "1":
                //击杀场景中所有的敌人
                for (int i = 0; i < 6; i++)
                {
                    GameManager.Instance.MinusEnemyCount();
                }
                UIManager.Instance.HidePanel<CheatPanel>();
                break;
            case "2":
                //直接切换到第一天晚上剧情
                DialogSystemMgr.Instance.UnLockedID(10015);
                DialogSystemMgr.Instance.StartPlayDialog(10015,E_DialogPlayType.Plot);
                UIManager.Instance.HidePanel<CheatPanel>();
                break;
            case "3":
                //直接切换到第二天晚上剧情
                DialogSystemMgr.Instance.UnLockedID(10029);
                DialogSystemMgr.Instance.StartPlayDialog(10029,E_DialogPlayType.Plot);
                UIManager.Instance.HidePanel<CheatPanel>();
                break;
            case "4":
                //玩家减少10点血量
                GameManager.Instance.player.Damage(10);
                UIManager.Instance.HidePanel<CheatPanel>();
                break;
            case "5":
                BagManager.Instance.AddItem(3);
                UIManager.Instance.HidePanel<CheatPanel>();
                break;
            case "6":
                BagManager.Instance.AddItem(1);
                UIManager.Instance.HidePanel<CheatPanel>();
                break;
            case "Close":
                UIManager.Instance.HidePanel<CheatPanel>();
                break;
        }
    }
}
