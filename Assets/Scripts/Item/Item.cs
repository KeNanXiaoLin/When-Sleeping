using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    /// <summary>
    /// 物品的精灵图片渲染器
    /// </summary>
    public SpriteRenderer sr;
    public BagItem itemData;

    void Start()
    {
        sr.sprite = itemData.sprite;
    }


}
