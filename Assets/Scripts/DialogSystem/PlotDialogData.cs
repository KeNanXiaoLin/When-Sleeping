using System.Collections;
using System.Collections.Generic;
using System.IO;
using KNXL.DialogSystem;
using UnityEngine;

[System.Serializable]
public class PlotDialogData
{
    /// <summary>
    /// 存储所有的单条对话数据
    /// </summary>
    public Dictionary<int, DialogData> allDialogDataDic = new();
    /// <summary>
    /// 存储每一段对话的角色数据
    /// </summary>
    public Dictionary<int, RoleDialogData> allRoleDialogDataDic = new();

    /// <summary>
    /// 加载默认数据
    /// </summary>
    public void LoadDefaultData()
    {
        string path1 = Application.streamingAssetsPath + "/DialogDatas.json";
        List<DialogData> datas = JsonMgr.Instance.LoadDataFromFilePath<List<DialogData>>(path1);
        string path2 = Application.streamingAssetsPath + "/RoleDialogDatas.json";
        List<RoleDialogData> datas1 = JsonMgr.Instance.LoadDataFromFilePath<List<RoleDialogData>>(path2);

        foreach (var item in datas)
        {
            if (!allDialogDataDic.ContainsKey(item.id))
            {
                allDialogDataDic.Add(item.id, item);
            }
            else
            {
                Debug.LogWarning("存在id相同的数据，请检查配置文件" + item.id);
            }
        }

        foreach (var item in datas1)
        {
            if (!allRoleDialogDataDic.ContainsKey(item.id))
            {
                allRoleDialogDataDic.Add(item.id, item);
            }
            else
            {
                Debug.LogWarning("存在id相同的数据，请检查配置文件");
            }
        }
    }
}
