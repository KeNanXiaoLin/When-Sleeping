using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagData
{
    public BagItem item;
    public int count;
    public int id => item.itemID;
    public string Name => item.itemName;
    public string Des => item.des;
    public Sprite Sprite => item.sprite;
    public bool CanUse => item.canUse;
    public string UseInfo => item.useTipInfo;

    public BagData(BagItem item, int count)
    {
        this.item = item;
        this.count = count;
    }
}
