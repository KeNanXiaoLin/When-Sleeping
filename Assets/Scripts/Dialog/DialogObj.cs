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
    public int dialogId;

    /// <summary>
    /// 返回当前能够触发的对话
    /// </summary>
    public RoleDialogData GetDialogData()
    {
        return DialogSystemMgr.Instance.GetDialogDataByType(dialogId);
    }
}
