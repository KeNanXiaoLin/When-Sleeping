using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UsePanel : UIPanelBase
{
    private UnityAction OkAction;
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
                UIManager.Instance.HidePanel<UsePanel>();
                OkAction?.Invoke();
                //调用之后置空，避免占用内存
                OkAction = null;
                break;
            case "Cancel":
                UIManager.Instance.HidePanel<UsePanel>();
                break;
        }
    }

    public void RegisterOKAction(UnityAction action)
    {
        if (action != null)
        {
            OkAction += action;
        }
    }

    public void UpdateInfo(string info)
    {
        tmpInfo.text = info;
    }

    
}
