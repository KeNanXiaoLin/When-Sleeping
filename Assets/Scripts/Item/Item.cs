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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //如果这个物品可以交互
            if (this.itemData.canInteractive)
            {
                GameManager.Instance.player.ShowHeadTip(this);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.player.HideHeadTip();
        }
    }
}
