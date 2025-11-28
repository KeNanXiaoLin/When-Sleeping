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

    public PlayerData playerData => GameManager.Instance.player.playerData;
    public override void HideMe()
    {
        
    }

    public override void ShowMe()
    {
        
    }

    void Start()
    {
        btnClose.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ChangePlayerDataPanel>();
        });
        btnSave.onClick.AddListener(() =>
        {
            JsonMgr.Instance.SaveData(playerData, "PlayerData");
        });
        sanSlider.value = (float)playerData.maxSan / maxSan;
        sanTextValue.text = playerData.maxSan.ToString();
        sanSlider.onValueChanged.AddListener((v) =>
        {
            playerData.maxSan = (int)(v*maxSan);
            sanTextValue.text = playerData.maxSan.ToString();
        });
        moveSpeedSlider.value = (float)playerData.moveSpeed / maxMoveSpeed;
        moveSpeedTextValue.text = playerData.moveSpeed.ToString("0.00");
        moveSpeedSlider.onValueChanged.AddListener((v) =>
        {
            playerData.moveSpeed = (float)(v*maxMoveSpeed);
            moveSpeedTextValue.text = playerData.moveSpeed.ToString("0.00");
        });
        initYSpeedSlider.value = (float)playerData.initYSpeed / maxInitYSpeed;
        initYSpeedTextValue.text = playerData.initYSpeed.ToString("0.00");
        initYSpeedSlider.onValueChanged.AddListener((v) =>
        {
            playerData.initYSpeed = (float)(v*maxInitYSpeed);
            initYSpeedTextValue.text = playerData.initYSpeed.ToString("0.00");
        });
        GSlider.value = (float)playerData.G / maxG;
        GTextValue.text = playerData.G.ToString("0.00");
        GSlider.onValueChanged.AddListener((v) =>
        {
            playerData.G = (float)(v*maxG);
            GTextValue.text = playerData.G.ToString("0.00");
        });
        maxJumpTimesSlider.value = (float)playerData.maxJumpTimes / maxMaxJumpTimes;
        maxJumpTimesTextValue.text = playerData.maxJumpTimes.ToString();
        maxJumpTimesSlider.onValueChanged.AddListener((v) =>
        {
            playerData.maxJumpTimes = (int)(v*maxMaxJumpTimes);
            maxJumpTimesTextValue.text = playerData.maxJumpTimes.ToString();
        });
        atkSizeSlider.value = (float)playerData.atkSize / maxAtkSize;
        atkSizeTextValue.text = playerData.atkSize.ToString();
        atkSizeSlider.onValueChanged.AddListener((v) =>
        {
            playerData.atkSize = (int)(v*maxAtkSize);
            atkSizeTextValue.text = playerData.atkSize.ToString();
        });
        atkRangeSlider.value = (float)playerData.atkRange / maxAtkRange;
        atkRangeTextValue.text = playerData.atkRange.ToString("0.00");
        atkRangeSlider.onValueChanged.AddListener((v) =>
        {
            playerData.atkRange = (float)(v*maxAtkRange);
            atkRangeTextValue.text = playerData.atkRange.ToString("0.00");
        });
        atkIntervalSlider.value = (float)playerData.atkInterval / maxAtkInterval;
        atkIntervalTextValue.text = playerData.atkInterval.ToString("0.00");
        atkIntervalSlider.onValueChanged.AddListener((v) =>
        {
            playerData.atkInterval = (float)(v*maxAtkInterval);
            atkIntervalTextValue.text = playerData.atkInterval.ToString("0.00");
        });
        maxHpSlider.value = (float)playerData.maxHp / maxMaxHp;
        maxHpTextValue.text = playerData.maxHp.ToString();
        maxHpSlider.onValueChanged.AddListener((v) =>
        {
            playerData.maxHp = (int)(v*maxMaxHp);
            maxHpTextValue.text = playerData.maxHp.ToString();
        });
        debugToggle.isOn = playerData.isDebug;
        debugToggle.onValueChanged.AddListener((v) =>
        {
            playerData.isDebug = v;
        });
    }
}
