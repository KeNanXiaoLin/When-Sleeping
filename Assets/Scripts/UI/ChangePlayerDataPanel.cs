using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangePlayerDataPanel : UIPanelBase
{
    [Header("按钮相关")]
    public Button btnClose;
    public Button btnSave;
    [Header("San值相关")]
    public Slider sanSlider;
    public TextMeshProUGUI sanTextValue;
    public int maxSan = 1000;
    [Header("moveSpeed值相关")]
    public Slider moveSpeedSlider;
    public TextMeshProUGUI moveSpeedTextValue;
    public float maxMoveSpeed = 100;
    [Header("initYSpeed值相关")]
    public Slider initYSpeedSlider;
    public TextMeshProUGUI initYSpeedTextValue;
    public float maxInitYSpeed = 30;
    [Header("G值相关")]
    public Slider GSlider;
    public TextMeshProUGUI GTextValue;
    public float maxG = 100;
    [Header("maxJumpTimes值相关")]
    public Slider maxJumpTimesSlider;
    public TextMeshProUGUI maxJumpTimesTextValue;
    public int maxMaxJumpTimes = 10;
    [Header("atkSize值相关")]
    public Slider atkSizeSlider;
    public TextMeshProUGUI atkSizeTextValue;
    public int maxAtkSize = 1000;
    [Header("atkRange值相关")]
    public Slider atkRangeSlider;
    public TextMeshProUGUI atkRangeTextValue;
    public float maxAtkRange = 20;
    [Header("atkInterval值相关")]
    public Slider atkIntervalSlider;
    public TextMeshProUGUI atkIntervalTextValue;
    public float maxAtkInterval = 20;
    [Header("maxHp值相关")]
    public Slider maxHpSlider;
    public TextMeshProUGUI maxHpTextValue;
    public int maxMaxHp = 1000;
    [Header("Debug相关")]
    public Toggle debugToggle;
    public override void HideMe()
    {
        
    }

    public override void ShowMe()
    {
        
    }
}
