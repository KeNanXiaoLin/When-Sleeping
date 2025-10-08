using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_DialogType
{
    MainRole,
    Mom,
    God,
    Bob,
}
public enum E_DialogTriggerType
{
    /// <summary>
    /// 在第一天的上午触发
    /// </summary>
    OneDayMorning,
    /// <summary>
    /// 在第一天晚上触发
    /// </summary>
    OneDayNight,
    /// <summary>
    /// 第二天早上触发
    /// </summary>
    SecondDayMorning,
    /// <summary>
    /// 第二天下午触发
    /// </summary>
    SecondDayAfternoon,
    /// <summary>
    /// 不需要特殊处理，由玩家主动触发
    /// </summary>
    None,
}

[CreateAssetMenu(fileName = "DialogData", menuName = "MyAssets/DialogData", order = 0)]
public class DialogData : ScriptableObject
{
    /// <summary>
    /// 对话的id
    /// </summary>
    public int id;
    //当前说话的对象
    public E_DialogType type;
    /// <summary>
    /// 对话的内容
    /// </summary>
    public string content;
    /// <summary>
    /// 关联的下一句对话
    /// </summary>
    public DialogData nextData;
    /// <summary>
    /// 是否是最后一句对话
    /// </summary>
    public bool isEnd;
    /// <summary>
    /// 是否是开始的对话
    /// </summary>
    public bool isStart;
    /// <summary>
    /// 对话的触发时机
    /// </summary>
    public E_DialogTriggerType triggerType;
    /// <summary>
    /// 对话是否已经触发过
    /// </summary>
    public bool isTrigger;
    /// <summary>
    /// 这句话赠送物品，需要显示提示框
    /// </summary>
    public BagItem item;
}