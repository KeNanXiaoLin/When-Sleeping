using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagManager : BaseManager<BagManager>
{
    private List<BagData> allDatas = new List<BagData>();
    private Dictionary<int, BagItem> allIteminfos = new();

    /// <summary>
    /// 私有构造函数
    /// </summary>
    private BagManager()
    {
        Init();
    }

    /// <summary>
    /// 初始化，记录所有的物品
    /// </summary>
    private void Init()
    {
        BagItem[] items = Resources.LoadAll<BagItem>("ItemData");
        foreach (var item in items)
        {
            allIteminfos.Add(item.itemID, item);
        }
    }

    /// <summary>
    /// 根据背包的内容的索引得到内容
    /// </summary>
    /// <param name="index"></param>
    public BagData GetItemByIndex(int index)
    {
        if (index > allDatas.Count)
        {
            UIManager.Instance.ShowPanel<TipPanel>((panel) =>
            {
                panel.UpdateInfo("there not have things");
            });
            return null;
        }
        return allDatas[index - 1];
    }

    /// <summary>
    /// 添加物品，默认添加一个
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="count"></param>
    public void AddItem(int itemID, int count = 1)
    {
        if (!allIteminfos.ContainsKey(itemID))
        {
            Debug.LogError("不存在这种物品，请检查传入参数");
            return;
        }
        BagItem addItem = allIteminfos[itemID];
        bool find = false;
        for (int i = 0; i < allDatas.Count; i++)
        {
            if (allDatas[i].id == addItem.itemID)
            {
                find = true;
                allDatas[i].count += count;
                break;
            }
        }
        if (!find)
        {
            allDatas.Add(new BagData(addItem, count));
        }
        EventCenter.Instance.EventTrigger<List<BagData>>(E_EventType.E_UpdateBag, allDatas);
    }

    public void RemoveItem(int itemID, int count = 1)
    {
        if (!allIteminfos.ContainsKey(itemID))
        {
            Debug.LogError("不存在这种物品，请检查传入参数");
            return;
        }
        BagItem removeItem = allIteminfos[itemID];
        bool find = false;
        for (int i = 0; i < allDatas.Count; i++)
        {
            if (allDatas[i].id == removeItem.itemID)
            {
                find = true;
                allDatas[i].count -= count;
                if (allDatas[i].count == 0)
                {
                    allDatas.Remove(allDatas[i]);
                }
                break;
            }
        }
        if (!find)
        {
            Debug.LogError("尝试移除你背包中不存在的物品，请检查传入参数");
        }
        EventCenter.Instance.EventTrigger<List<BagData>>(E_EventType.E_UpdateBag, allDatas);
    }

    /// <summary>
    /// 清空背包
    /// </summary>
    public void ClearBag()
    {
        allDatas.Clear();
    }

    /// <summary>
    /// 调试代码，用于显示背包的内容
    /// </summary>
    public void ShowBagInfo()
    {
        Debug.Log("我现在有" + allDatas.Count + "种物品");
        foreach (var item in allDatas)
        {
            Debug.LogFormat("物品名称{0},物品数量{1}", item.Name, item.count);
        }
    }

}
