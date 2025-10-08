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
            case "Quit":
                Application.Quit();
                break;
        }
    }
}
