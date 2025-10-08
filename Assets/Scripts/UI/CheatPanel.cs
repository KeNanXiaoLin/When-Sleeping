using System.Collections;
using System.Collections.Generic;
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
                TimeSystem.Instance.SpeedUpOneHour();
                UIManager.Instance.HidePanel<CheatPanel>();
                break;
            case "2":
                TimeSystem.Instance.SpeedUpThreeHour();
                UIManager.Instance.HidePanel<CheatPanel>();
                break;
            case "3":
                TimeSystem.Instance.SpeedUpOneDay();
                UIManager.Instance.HidePanel<CheatPanel>();
                break;
            case "4":
                BagManager.Instance.AddItem(2);
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
