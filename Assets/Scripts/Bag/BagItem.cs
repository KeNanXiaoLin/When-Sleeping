using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BagItemData", menuName = "MyAssets/BagItemData", order = 2)]
public class BagItem : ScriptableObject
{
    /// <summary>
    /// 物品id
    /// </summary>
    public int itemID;
    /// <summary>
    /// 物品精灵图片
    /// </summary>
    public Sprite sprite;
    /// <summary>
    /// 物品名字
    /// </summary>
    public string itemName;
    /// <summary>
    /// 物品描述
    /// </summary>
    public string des;
    /// <summary>
    /// 这个物品是否可以交互
    /// </summary>
    public bool canInteractive = true;
    /// <summary>
    /// 这个物品是否可以使用
    /// </summary>
    public bool canUse = true;
    public string useTipInfo;
}
