using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]
public class LA_Item : ScriptableObject
{
    public enum ItemType
    {
        Material,   //材料
        Weapon,     //武器
        armor,      //防具
        medcine,    //药瓶
        Mission,    //任务物品
    }

    //基本属性
    public int itemID;
    public string ItemName;
    public ItemType itemType;
    public Sprite ItemPicture;
    [TextArea]
    public string ItemInto;

    //道具合成配方
    public List<LA_Item> repiteItem;
    public List<int> repiteItemNum;

    //武器：攻击力
    public int AttackRate;

    //防具：防御力
    public int DefenceRate;

    //药品：生命值增加量
    public int HealthUpRate;

}
