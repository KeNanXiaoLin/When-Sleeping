using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 用于 里式替换原则 装载 子类的父类
/// </summary>
public abstract class EventInfoBase { }

/// <summary>
/// 用来包裹 对应观察者 函数委托的 类
/// </summary>
/// <typeparam name="T"></typeparam>
public class EventInfo<T> : EventInfoBase
{
    //真正观察者 对应的 函数信息 记录在其中
    public UnityAction<T> actions;

    public EventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}

/// <summary>
/// 主要用来记录无参无返回值委托
/// </summary>
public class EventInfo : EventInfoBase
{
    public UnityAction actions;

    public EventInfo(UnityAction action)
    {
        actions += action;
    }
}

/// <summary>
/// 新增：用于存储无参数协程回调（Func<IEnumerator>）
/// </summary>
public class CoroutineEventInfo : EventInfoBase
{
    public List<Func<IEnumerator>> actions = new List<Func<IEnumerator>>(); // 用List存储，避免多播委托返回值问题
    public CoroutineEventInfo(Func<IEnumerator> action)
    {
        actions.Add(action);
    }
}

/// <summary>
/// 新增：用于存储有参数的协程回调（Func<T,IEnumerator>）
/// </summary>
/// <typeparam name="T"></typeparam>
public class CoroutineEventInfo<T> : EventInfoBase
{
    public List<Func<T, IEnumerator>> actions = new List<Func<T, IEnumerator>>(); // 用List存储，避免多播委托返回值问题
    public CoroutineEventInfo(Func<T, IEnumerator> action)
    {
        actions.Add(action);
    }
}


/// <summary>
/// 事件中心模块 
/// </summary>
public class EventCenter : BaseManager<EventCenter>
{

    //用于记录对应事件 关联的 对应的逻辑
    private Dictionary<E_EventType, EventInfoBase> eventDic = new Dictionary<E_EventType, EventInfoBase>();

    private EventCenter() { }

    /// <summary>
    /// 触发事件 
    /// </summary>
    /// <param name="eventName">事件名字</param>
    public void EventTrigger<T>(E_EventType eventName, T info)
    {
        //存在关心我的人 才通知别人去处理逻辑
        if (eventDic.ContainsKey(eventName))
        {
            //去执行对应的逻辑
            (eventDic[eventName] as EventInfo<T>).actions?.Invoke(info);
        }
    }

    /// <summary>
    /// 触发事件 无参数
    /// </summary>
    /// <param name="eventName"></param>
    public void EventTrigger(E_EventType eventName)
    {
        //存在关心我的人 才通知别人去处理逻辑
        if (eventDic.ContainsKey(eventName))
        {
            //去执行对应的逻辑
            (eventDic[eventName] as EventInfo).actions?.Invoke();
        }
    }

    // 触发协程事件并等待所有协程完成（核心：支持yield等待）
    public IEnumerator TriggerCoroutineAndWait(E_EventType eventName)
    {
        if (eventDic.TryGetValue(eventName, out var info) && info is CoroutineEventInfo coroutineInfo)
        {
            foreach (var func in coroutineInfo.actions)
            {
                yield return func.Invoke(); // 逐个等待协程执行
            }
        }
    }

    // 触发协程事件并等待所有协程完成（核心：支持yield等待）
    public IEnumerator TriggerCoroutineAndWait<T>(E_EventType eventName, T data)
    {
        if (eventDic.TryGetValue(eventName, out var info) && info is CoroutineEventInfo<T> coroutineInfo)
        {
            foreach (var func in coroutineInfo.actions)
            {
                yield return func.Invoke(data); // 逐个等待协程执行
            }
        }
    }

    /// <summary>
    /// 添加事件监听者
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="func"></param>
    public void AddEventListener<T>(E_EventType eventName, UnityAction<T> func)
    {
        //如果已经存在关心事件的委托记录 直接添加即可
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo<T>).actions += func;
        }
        else
        {
            eventDic.Add(eventName, new EventInfo<T>(func));
        }
    }


    public void AddEventListener(E_EventType eventName, UnityAction func)
    {
        //如果已经存在关心事件的委托记录 直接添加即可
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo).actions += func;
        }
        else
        {
            eventDic.Add(eventName, new EventInfo(func));
        }
    }

    /// <summary>
    /// 注册无参数的协程回调
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="func"></param>
    public void AddCoroutineListener(E_EventType eventName, Func<IEnumerator> func)
    {
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as CoroutineEventInfo).actions.Add(func);
        }
        else
        {
            eventDic.Add(eventName, new CoroutineEventInfo(func));
        }
    }

    /// <summary>
    /// 注册有参数的协程回调
    /// </summary>
    /// <typeparam name="T">参数的泛型</typeparam>
    /// <param name="eventName">事件的枚举名</param>
    /// <param name="func">委托名字</param>
    public void AddCoroutineListener<T>(E_EventType eventName, Func<T, IEnumerator> func)
    {
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as CoroutineEventInfo<T>).actions.Add(func);
        }
        else
        {
            eventDic.Add(eventName, new CoroutineEventInfo<T>(func));
        }
    }

    /// <summary>
    /// 移除事件监听者
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="func"></param>
    public void RemoveEventListener<T>(E_EventType eventName, UnityAction<T> func)
    {
        if (eventDic.ContainsKey(eventName))
            (eventDic[eventName] as EventInfo<T>).actions -= func;
    }

    public void RemoveEventListener(E_EventType eventName, UnityAction func)
    {
        if (eventDic.ContainsKey(eventName))
            (eventDic[eventName] as EventInfo).actions -= func;
    }

    /// <summary>
    /// 移除协程回调（按需实现）
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="func"></param>
    public void RemoveCoroutineListener(E_EventType eventName, Func<IEnumerator> func)
    {
        if (eventDic.TryGetValue(eventName, out var info) && info is CoroutineEventInfo coroutineInfo)
        {
            coroutineInfo.actions.Remove(func);
        }
    }

    /// <summary>
    /// 移除带参数的协程回调（按需实现）
    /// </summary>
    /// <typeparam name="T">参数的泛型</typeparam>
    /// <param name="eventName"></param>
    /// <param name="func"></param>
    public void RemoveCoroutineListener<T>(E_EventType eventName, Func<T, IEnumerator> func)
    {
        if (eventDic.TryGetValue(eventName, out var info) && info is CoroutineEventInfo<T> coroutineInfo)
        {
            coroutineInfo.actions.Remove(func);
        }
    }

    /// <summary>
    /// 清空所有事件的监听
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }

    /// <summary>
    /// 清除指定某一个事件的所有监听
    /// </summary>
    /// <param name="eventName"></param>
    public void Clear(E_EventType eventName)
    {
        if (eventDic.ContainsKey(eventName))
            eventDic.Remove(eventName);
    }
}
