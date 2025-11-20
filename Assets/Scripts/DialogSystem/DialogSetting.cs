using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DialogSetting", menuName = "MyAssets/DialogSetting")]
public class DialogSetting : ScriptableObject
{
    [Header("是否开启对话系统播放对话的动画")]
    public bool isOpenDialogPlayAnim = true;
    [Header("中文中每个字播放的间隔时间")]
    public float wordIntervalTime = 0.1f;
}
