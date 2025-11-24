using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystemMgr : BaseManager<SaveSystemMgr>
{
    /// <summary>
    /// 本地是否有存档文件
    /// </summary>
    private bool isHaveSaveFile = false;

    public bool IsHaveSaveFile { get => isHaveSaveFile;}
    private string saveFileName = "saveData";

    private SaveSystemMgr()
    {
        string saveFilePath = Application.persistentDataPath + "/" + saveFileName + ".json";
        if(File.Exists(saveFilePath))
        {
            isHaveSaveFile = true;
        }
        else
        {
            isHaveSaveFile = false;
        }
    }

    public void SaveData()
    {
        
    }
}
