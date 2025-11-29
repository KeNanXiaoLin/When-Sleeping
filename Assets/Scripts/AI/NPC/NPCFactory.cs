using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFactory : BaseManager<NPCFactory>
{
    private NPCFactory()
    {
        
    }

    /// <summary>
    /// 创建NPC
    /// </summary>
    /// <param name="npcType">NPC类型</param>
    /// <returns>NPC控制器</returns>
    // public NPCController CreateNPC(E_NPCType npcType,Vector3 spawnPos=default,Vector3 homePos=default)
    // {
    //     Debug.Log($"创建NPC:{npcType.ToString()}");
    //     GameObject prefab = Resources.Load<GameObject>($"NPC/{npcType.ToString()}");
    //     NPCController npcController = GameObject.Instantiate(prefab,spawnPos,Quaternion.identity).GetComponent<NPCController>();
    //     npcController.Init(npcType,spawnPos,homePos);
    //     return npcController;
    // }

    /// <summary>
    /// 创建NPC
    /// </summary>
    /// <param name="npcType">NPC类型</param>
    /// <returns>NPC控制器</returns>
    public NPCController CreateNPC(E_NPCType npcType,NPCData npcData)
    {
        GameObject prefab = Resources.Load<GameObject>($"NPC/{npcType.ToString()}");
        NPCController npcController = GameObject.Instantiate(prefab,npcData.pos,Quaternion.identity).GetComponent<NPCController>();
        npcController.Init(npcData);
        return npcController;
    }
}
