using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 事件类型 枚举
/// </summary>
public enum E_EventType
{
    /// <summary>
    /// 时间更新事件
    /// </summary>
    E_UpdateTime,
    E_SceneLoadAfter,
    /// <summary>
    /// 场景切换后，淡出销毁前的事情,主要是播动画
    /// </summary>
    // E_SceneLoadFaderBeforeCoroutine,
    /// <summary>
    /// 背包更新事件
    /// </summary>
    E_UpdateBag,
    /// <summary>
    /// 早上会触发的事件
    /// </summary>
    E_Morning,
    /// <summary>
    /// 下午会触发的事件
    /// </summary>
    E_Afternoon,
    /// <summary>
    /// 晚上会触发的事件
    /// </summary>
    E_Night,
    /// <summary>
    /// 对话结束触发事件
    /// </summary>
    E_DialogEnd,
}
