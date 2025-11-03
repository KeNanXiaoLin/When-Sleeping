using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TipPanel : UIPanelBase
{
    public TextMeshProUGUI tmpInfo;
    public override void ShowMe()
    {

    }

    public override void HideMe()
    {

    }

    protected override void ClickBtn(string btnName)
    {
        switch (btnName)
        {
            case "OK":
                UIManager.Instance.HidePanel<TipPanel>();
                break;
        }
    }

    public void UpdateInfo(string info)
    {
        this.tmpInfo.text = info;
    }

    public void AddOKEvent(UnityAction action)
    {
        Button ok = GetControl<Button>("OK");
        ok.onClick.AddListener(action);
    }


}
