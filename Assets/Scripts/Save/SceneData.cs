using System.Collections;
using System.Collections.Generic;
using KNXL.DialogSystem;
using UnityEngine;

/// <summary>
/// 场景数据中需要存储的NPC类型
/// </summary>
public enum E_NPCType
{
    Mom,
    Bob,
}

[System.Serializable]
/// <summary>
/// 场景数据，存储每个场景的相关数据
/// </summary>
public class SceneData
{
    /// <summary>
    /// 通过字典来管理所有场景数据
    /// </summary>
    public Dictionary<string,SingleSceneData> sceneData = new();

    /// <summary>
    /// 添加场景数据
    /// </summary>
    /// <param name="sceneName">场景名称</param>
    /// <param name="singleSceneData">场景数据</param>
    public void AddSceneData(string sceneName,SingleSceneData singleSceneData)
    {
        if (sceneData.ContainsKey(sceneName))
        {
            return;
        }
        sceneData.Add(sceneName,singleSceneData);
    }
    /// <summary>
    /// 获取场景数据
    /// </summary>
    /// <param name="sceneName">场景名称</param>
    /// <returns>场景数据</returns>
    public SingleSceneData GetSceneData(string sceneName)
    {
        if (!sceneData.ContainsKey(sceneName))
        {
            return null;
        }
        return sceneData[sceneName];
    }
    /// <summary>
    /// 删除场景数据
    /// </summary>
    /// <param name="sceneName">场景名称</param>
    public void RemoveSceneData(string sceneName)
    {
        if (!sceneData.ContainsKey(sceneName))
        {
            Debug.LogError($"场景数据中不存在{sceneName}的场景数据");
            return;
        }
        sceneData.Remove(sceneName);
    }

    public void UpdateSceneData(string sceneName,SingleSceneData singleSceneData)
    {
        sceneData[sceneName] = singleSceneData;
    }
}

[System.Serializable]
/// <summary>
/// 单场景数据，存储单个场景的相关数据
/// </summary>
public class SingleSceneData
{
    public string sceneName;
    public Dictionary<E_NPCType,NPCData> npcData = new();

    /// <summary>
    /// 添加NPC数据
    /// </summary>
    /// <param name="npcType">NPC类型</param>
    /// <param name="npcData">NPC数据</param>
    public void AddNPCData(E_NPCType npcType,NPCData npcData)
    {
        if (this.npcData.ContainsKey(npcType))
        {
            Debug.LogError($"场景数据中已经存在{sceneName}的{npcType}的NPC数据");
            return;
        }
        this.npcData.Add(npcType,npcData);
    }
    /// <summary>
    /// 获取NPC数据
    /// </summary>
    /// <param name="npcType">NPC类型</param>
    /// <returns>NPC数据</returns>
    public NPCData GetNPCData(E_NPCType npcType)
    {
        if (!this.npcData.ContainsKey(npcType))
        {
            return new NPCData(){npcType = npcType};
        }
        return this.npcData[npcType];
    }

    /// <summary>
    /// 删除NPC数据
    /// </summary>
    /// <param name="npcType">NPC类型</param>
    public void RemoveNPCData(E_NPCType npcType)
    {
        if (!this.npcData.ContainsKey(npcType))
        {
            return;
        }
        this.npcData.Remove(npcType);
    }
}
