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

    public bool IsHaveSaveFile { get => isHaveSaveFile; }
    /// <summary>
    /// 存档文件名
    /// </summary>
    private string saveFileName = "saveData";
    private string saveFilePath;
    private string defaultSaveFilePath;
    /// <summary>
    /// 存档数据
    /// </summary>
    public SaveData saveData;

    private SaveSystemMgr()
    {

    }

    /// <summary>
    /// 进入游戏，查看本地是否有存档，来进行开始界面UI的显示布局
    /// </summary>
    public void Init()
    {
        saveFilePath = Application.persistentDataPath + "/" + saveFileName + ".json";
        defaultSaveFilePath = Application.streamingAssetsPath + "/" + saveFileName + ".json";
        if (File.Exists(saveFilePath))
        {
            isHaveSaveFile = true;
        }
        else
        {
            isHaveSaveFile = false;
        }
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    public void Save()
    {
        JsonMgr.Instance.SaveDataToFilePath(saveData, saveFilePath);
    }

    /// <summary>
    /// 读取本地存档文件
    /// </summary>
    public void Load()
    {
        // 这是存档文件的路径

        saveData = JsonMgr.Instance.LoadDataFromFilePath<SaveData>(saveFilePath);

    }

    public void DefaultLoad()
    {
        saveData = new SaveData();
        saveData.LoadDefaultData();
    }
}
