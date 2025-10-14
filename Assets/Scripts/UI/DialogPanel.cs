using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanel : UIPanelBase
{
    public TextMeshProUGUI contentTMP;
    public TextMeshProUGUI nameTMP;
    public Image headIcon;

    public override void HideMe()
    {

    }

    public override void ShowMe()
    {

    }

    public void UpdateDialogText(string content)
    {
        contentTMP.text = content;
    }

    public void UpdateDialogOtherInfo(DialogData data)
    {
        switch (data.type)
        {
            case E_DialogType.MainRole:
                headIcon.sprite = Resources.Load<Sprite>("Sprites/MainRole");
                nameTMP.text = "Mike";
                break;
            case E_DialogType.Mom:
                headIcon.sprite = Resources.Load<Sprite>("Sprites/Mom");
                nameTMP.text = "Mom";
                break;
            case E_DialogType.Bob:
                headIcon.sprite = Resources.Load<Sprite>("Sprites/Bob");
                nameTMP.text = "Bob";
                break;
            case E_DialogType.God:
                headIcon.sprite = Resources.Load<Sprite>("Sprites/God");
                nameTMP.text = "God";
                break;
        }
    }

    public void UpdateInfo(DialogData data)
    {
        switch (data.type)
        {
            case E_DialogType.MainRole:
                headIcon.sprite = Resources.Load<Sprite>("Sprites/MainRole");
                nameTMP.text = "Mike";
                break;
            case E_DialogType.Mom:
                headIcon.sprite = Resources.Load<Sprite>("Sprites/Mom");
                nameTMP.text = "Mom";
                break;
            case E_DialogType.Bob:
                headIcon.sprite = Resources.Load<Sprite>("Sprites/Bob");
                nameTMP.text = "Bob";
                break;
            case E_DialogType.God:
                headIcon.sprite = Resources.Load<Sprite>("Sprites/God");
                nameTMP.text = "God";
                break;
        }
        UpdateDialogText(data.content);
    }

    public void ClickDialogPanel()
    {
        DialogSystem.Instance.TryPlayNextDialog();
    }
}
