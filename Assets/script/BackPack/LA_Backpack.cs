using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 物品槽类，用于存储物品及其数量
/// </summary>
public class ItemSlot
{
    public LA_Item itemOS;
    public int ItemNum;

    public ItemSlot(LA_Item _itemOS)
    {
        itemOS = _itemOS;
        ItemNum = 1;
    }
}

public class LA_Backpack : MonoBehaviour
{
    //单例模式
    public static LA_Backpack instence;

    void Awake()
    {
        if (instence != null) Destroy(instence);
        else instence = this;
    }

    public List<ItemSlot> Backpack = new List<ItemSlot>();

 
    [Header("背包系统UI：物品槽")]
    public List<Image> UISlot_Backpack = new List<Image>();

    /// <summary>
    /// 向背包中添加物品
    /// </summary>
    /// <param name="_item">要添加的物品</param>
    public void AddItm_Backpack(LA_Item _item)
    {
        bool HadAddItem = false;

        foreach (ItemSlot i in Backpack)
        {
            if (i.itemOS.itemID == _item.itemID)
            {
                i.ItemNum++;
                HadAddItem = true;
            }
        }

        if (HadAddItem == false)
        {
            ItemSlot newItem = new ItemSlot(_item);
            Backpack.Add(newItem);
        }

        InitUI_Backpack();
    }

    /// <summary>
    /// 从背包中减少物品数量
    /// </summary>
    /// <param name="_Item">要减少的物品</param>
    /// <param name="_deceleNum">减少的数量</param>
    public void DeceleItem_Backpack(LA_Item _Item, int _deceleNum)
    {
        foreach (ItemSlot item in Backpack)
        {
            if (item.itemOS.itemID == _Item.itemID)
            {
                item.ItemNum -= _deceleNum;
            }
        }
    }

    /// <summary>
    /// 读取背包中指定物品的数量
    /// </summary>
    /// <param name="_Item">要查询的物品</param>
    /// <returns>物品数量</returns>
    public int ReadItemNum_Backpack(LA_Item _Item)
    {
        foreach (ItemSlot i in Backpack)
        {
            if (i.itemOS.itemID == _Item.itemID)
            {
                return i.ItemNum;
            }
        }

        return 0;
    }

    /// <summary>
    /// 检查背包中指定物品的数量是否满足目标数量
    /// </summary>
    /// <param name="_Item">要检查的物品</param>
    /// <param name="_AimNum">目标数量</param>
    /// <returns>是否满足</returns>
    public bool CheckItemNum_Backpack(LA_Item _Item, int _AimNum)
    {
        int A = ReadItemNum_Backpack(_Item);
        
        if (A == _AimNum) return true;
        else return false;
    }

    /// <summary>
    /// 初始化背包UI
    /// </summary>
    private void InitUI_Backpack()
    {
        for (int i = 0; i < Backpack.Count; i++)
        {
            UISlot_Backpack[i].sprite = Backpack[i].itemOS.ItemPicture;

            Text UISlotText = UISlot_Backpack[i].GetComponentInChildren<Text>();
            UISlotText.text = Backpack[i].ItemNum.ToString();
        }

    }
    

}
