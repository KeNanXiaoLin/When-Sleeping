using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSystem :BaseManager<DialogSystem>
{
    private Dictionary<int, DialogData> dialogDic;
    private DialogData curData;

    /// <summary>
    /// 私有构造
    /// </summary>
    private DialogSystem()
    {
        Init();
        
    }

    /// <summary>
    /// 初始化所有对话数据
    /// </summary>
    public void Init()
    {
        dialogDic = new();
        DialogData[] dialogs = Resources.LoadAll<DialogData>("DialogData");
        foreach (DialogData data in dialogs)
        {
            if (!dialogDic.ContainsKey(data.id))
            {
                dialogDic.Add(data.id, data);
            }
            else
            {
                Debug.Log("存在重复id属性，请检查是否存在配置错误");
            }
        }
    }

    /// <summary>
    /// 测试脚本，用于初始化单例
    /// </summary>
    public void Test()
    {

    }
    /// <summary>
    /// 触发开始对话
    /// </summary>
    /// <param name="data"></param>
    public void TriggerStartDialog(DialogData data)
    {
        if (!dialogDic.ContainsKey(data.id)) return;
        curData = data;

        if (data.isStart)
        {
            //显示对话面板，播放对话
            UIManager.Instance.ShowPanel<DialogPanel>((panel) =>
            {
                panel.UpdateInfo(data);
                //设置对话已经触发，防止重复触发
                data.isTrigger = true;

            });
        }
        else
        {
            Debug.Log("当前没有办法触发对话，请检查数据是否存在问题");
        }
    }

    public void TriggerDialog(DialogData data)
    {
        if (!dialogDic.ContainsKey(data.id)) return;
        curData = data;
        if (data.type == E_DialogType.God)
        {
            UIManager.Instance.ShowPanel<TipPanel>((panel) =>
            {
                panel.UpdateInfo(data.item.des);
                BagManager.Instance.AddItem(data.item.itemID);
            });
        }
        else
        {
            //显示对话面板，播放对话
            UIManager.Instance.ShowPanel<DialogPanel>((panel) =>
            {
                panel.UpdateInfo(data);
            });
        }
        
    }

    /// <summary>
    /// 触发下一句对话
    /// </summary>
    public void TriggerNextDialog()
    {
        if (curData == null)
        {
            Debug.LogError("当前还没有触发过对话，无法触发下一句对话");
            return;
        }
        if (curData.isEnd)
        {
            Debug.Log("对话已经触发完毕");
            //关闭对话面板，做一些其他事情
            UIManager.Instance.HidePanel<DialogPanel>();
            //分发对话结束事件
            EventCenter.Instance.EventTrigger(E_EventType.E_DialogEnd);
        }
        else
        {
            curData = curData.nextData;
            TriggerDialog(curData);
        }
    }
}
