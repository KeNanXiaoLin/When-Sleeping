using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : UIPanelBase
{
    public TextMeshProUGUI tmpTime;
    public TextMeshProUGUI tmpSanValue;
    public List<Image> images;
    public Image timeIcon;
    private int maxSanValue;
    private int lastSanvalue;

    void Start()
    {
        if (GameManager.Instance.player != null)
        {
            maxSanValue = GameManager.Instance.player.Max_San;
            lastSanvalue = maxSanValue;
        }
        else
        {
            Debug.LogError("赋值的时机不对，这个时候玩家还没有初始化");
        }
    }

    public override void ShowMe()
    {
        EventCenter.Instance.AddEventListener<string>(E_EventType.E_UpdateTime, UpdateTime);
        EventCenter.Instance.AddEventListener<List<BagData>>(E_EventType.E_UpdateBag, UpdateBagInfo);
        EventCenter.Instance.AddEventListener<int>(E_EventType.E_SanChange, UpdateSanValue);
    }
    public override void HideMe()
    {
        EventCenter.Instance.RemoveEventListener<string>(E_EventType.E_UpdateTime, UpdateTime);
        EventCenter.Instance.RemoveEventListener<List<BagData>>(E_EventType.E_UpdateBag, UpdateBagInfo);
        EventCenter.Instance.RemoveEventListener<int>(E_EventType.E_SanChange, UpdateSanValue);
    }


    public void UpdateTime(string info)
    {
        tmpTime.text = info;
        E_TimeType curType = TimeSystem.Instance.CurTimeType;
        switch (curType)
        {
            case E_TimeType.Moring:
                timeIcon.sprite = Resources.Load<Sprite>("Sprites/Morning");
                break;
            case E_TimeType.Afternoon:
                timeIcon.sprite = Resources.Load<Sprite>("Sprites/Afternoon");
                break;
            case E_TimeType.Night:
                timeIcon.sprite = Resources.Load<Sprite>("Sprites/Night");
                break;
        }
    }

    public void UpdateBagInfo(List<BagData> datas)
    {
        if (datas == null || datas.Count == 0)
            return;
        int maxCanShow = datas.Count > images.Count ? images.Count : datas.Count;
        for (int i = 0; i < maxCanShow; i++)
        {
            images[i].sprite = datas[i].Sprite;
        }
    }

    protected override void ClickBtn(string btnName)
    {
        switch (btnName)
        {
            case "Pause":
                //弹出暂停面板
                UIManager.Instance.ShowPanel<PausePanel>();
                //停止时间流逝
                TimeSystem.Instance.PauseTime();
                //禁用玩家输入
                GameManager.Instance.player.DisablePlayerInput();
                break;
            case "Quit":
                Application.Quit();
                break;
        }
    }

    public void UpdateSanValue(int value)
    {
        Debug.Log("San值变化:" + value);
        StartCoroutine(SanChangeCoroutine(value));
    }

    private IEnumerator SanChangeCoroutine(int value)
    {
        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime;
            float val = Mathf.Lerp(lastSanvalue, value, t);
            tmpSanValue.text = $"{((int)val)} /{maxSanValue}";
            yield return null;
        }
        lastSanvalue = value;

    }
}
