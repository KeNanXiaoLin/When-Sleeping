using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "MyAssets/NPCData", order = 1)]
public class NPCData : ScriptableObject
{
    /// <summary>
    /// npc的Id
    /// </summary>
    public int id;
    /// <summary>
    /// npc的所有对话信息
    /// </summary>
    public List<DialogData> allDialogs;
    /// <summary>
    /// 生成的位置
    /// </summary>
    public Vector3 spawnPos;
    /// <summary>
    /// 精灵图片
    /// </summary>
    public Sprite sprite;
}
