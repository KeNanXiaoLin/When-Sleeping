using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveData
{
    /// <summary>
    /// 玩家状态数据
    /// </summary>
    public PlayerData playerData = new();
    /// <summary>
    /// 剧情对话数据
    /// </summary>
    public PlotDialogData plotDialogData = new();
    /// <summary>
    /// 游戏数据
    /// </summary>
    public GameData gameData = new();

    /// <summary>
    /// 如果调用构造函数，就是想要使用默认数据
    /// </summary>
    public SaveData()
    {
        
    }

    public void LoadDefaultData()
    {
        // 因为要提供给外部修改数据的功能，所以我们加载默认数据就以persistentDataPath为准
        string filePath = Application.persistentDataPath + "/PlayerData.json";
        if (File.Exists(filePath))
        {
            playerData = JsonMgr.Instance.LoadDataFromFilePath<PlayerData>(filePath);
        }
        else
        {
            filePath = Application.streamingAssetsPath + "/PlayerData.json";
            playerData = JsonMgr.Instance.LoadDataFromFilePath<PlayerData>(filePath);
        }
        plotDialogData = new PlotDialogData();
        plotDialogData.LoadDefaultData();
    }
}
