using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : UIPanelBase
{
    public ScrollRect sv;
    public float spacing = 10f;
    public int rowCount = 4;

    public override void HideMe()
    {

    }

    public override void ShowMe()
    {

    }

    public void UpdateBagInfo(List<BagData> datas)
    {
        if (datas == null || datas.Count == 0)
            return;
        for (int i = 0; i < datas.Count; i++)
        {
            //先把背包图片实例化出来
            GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("UI/ItemUI"), sv.content);
            //设置位置
            obj.transform.localPosition = new Vector3(i % rowCount * (spacing + 100), i / rowCount * (spacing + 100), 0);
            //替换图片
            ItemUI ui = obj.GetComponent<ItemUI>();
            ui.UpdateInfo(datas[i].Sprite, datas[i].count);
        }
        int height = 0;
        height = datas.Count / rowCount;
        if (datas.Count % rowCount > 0)
        {
            height += 1;
        }
    }
}
