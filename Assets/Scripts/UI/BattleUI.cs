using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : UIPanelBase
{
    [SerializeField] private Image PlayerHealthUI;
    public override void HideMe()
    {
        EventCenter.Instance.RemoveEventListener<float>(E_EventType.E_UpdateHp, UpdateInfo);
    }

    public override void ShowMe()
    {
        EventCenter.Instance.AddEventListener<float>(E_EventType.E_UpdateHp, UpdateInfo);
    }

    public void UpdateInfo(float value)
    {
        PlayerHealthUI.fillAmount = value;
    }

    protected override void ClickBtn(string btnName)
    {
        switch (btnName)
        {
            case "Quit":
                Application.Quit();
                break;
        }
    }
}
