using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LA_ItemWorkBench : MonoBehaviour
{
    //TODO 将制作UI的初始化制作为由代码生成
    [Header("用于生成的工作台游戏物体")]
    public GameObject WorkBenchCube;




    //检测道具是否能被制作
    public bool CheckCanMakeItem_WorkBench(LA_Item _Item)
    {
        int AimCheckNum = _Item.repiteItem.Count;
        int check = 0;

        for (int i = 0; i < AimCheckNum; i++)
        {
            if (LA_Backpack.instence.ReadItemNum_Backpack(_Item.repiteItem[i]) >= _Item.repiteItemNum[i])
            {
                check++;
            }
        }

        if (check == AimCheckNum) return true;
        else return false;
    }

    //用于Button: 检测并制作道具
    public void MakeItem_WorkBench(LA_Item _AimItem)
    {
        if (CheckCanMakeItem_WorkBench(_AimItem) == false) return;

        for (int i = 0; i < _AimItem.repiteItem.Count; i++)
        {
            if (_AimItem.repiteItem[i].itemType != LA_Item.ItemType.Block)
                LA_Backpack.instence.DeceleItem_Backpack(_AimItem.repiteItem[i], _AimItem.repiteItemNum[i]);
        }

        if (_AimItem.itemID == 21) Instantiate(WorkBenchCube);
        else LA_Backpack.instence.AddItm_Backpack(_AimItem);
    }

    
}
