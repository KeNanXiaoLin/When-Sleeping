using System.Collections;
using System.Collections.Generic;
using KNXL.DialogSystem;
using UnityEngine;

public class DialogObj : MonoBehaviour
{
    // <summary>
    /// 这个对象的对话类型
    /// </summary>
    public E_DialogRoleType dialogType;
    /// <summary>
    /// 这个对象可以触发的所有对话
    /// </summary>
    private List<RoleDialogData> m_allDialogDatas;
    /// <summary>
    /// 当前能够触发的对话，如果没有，返回null
    /// </summary>
    // public int dialogId;

    void Awake()
    {
        //初始化所有身上的数据
        GetAllDialogData();
    }

    /// <summary>
    /// 返回当前能够触发的对话
    /// </summary>
    // public RoleDialogData GetDialogData()
    // {
    //     return DialogSystemMgr.Instance.GetDialogDataByID(dialogId);
    // }

    /// <summary>
    /// 从对话管理器中找到所有属于自己的对话
    /// </summary>
    private void GetAllDialogData()
    {
        m_allDialogDatas = DialogSystemMgr.Instance.GetDialogDataByType(dialogType);
    }

    /// <summary>
    /// 从自己的所有对话中找到一个可以触发的对话进行播放
    /// 重复触发的对话的处理，再解锁下一个对话前都使用这个它，解锁了下一个对话禁用他的canTriggerRepeat即可
    /// </summary>
    /// <returns></returns>
    public RoleDialogData GetFirstCanTriggerDialogData()
    {
        foreach (var item in m_allDialogDatas)
        {
            //对话已经解锁，并且没有触发过或者触发过但是可以重复触发
            if (!item.isLocked && (!item.isTrigger || item.isTrigger && item.canTriggerRepeat))
            {
                return item;
            }
        }
        return null;
    }

    /// <summary>
    /// 当前是否有对话可以播放，主要是用来做头顶的提示
    /// </summary>
    /// <returns></returns>
    public bool IsHaveDialogCanPlay()
    {
        RoleDialogData data = GetFirstCanTriggerDialogData();
        if (data != null)
            return true;
        return false;
    }
}
