using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : UIPanelBase
{
    [SerializeField] private Image PlayerHealthUI;
    public override void HideMe()
    {

    }

    public override void ShowMe()
    {

    }

    public void UpdateInfo(float value)
    {
        PlayerHealthUI.fillAmount = value;
    }
}
