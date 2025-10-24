using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KNXL.DialogSystem
{
    public enum E_DialogType
    {
        /// <summary>
        /// 就是普通对话，不用做特殊处理
        /// </summary>
        Normal,
        /// <summary>
        /// 任务对话
        /// </summary>
        Task,
        /// <summary>
        /// 有关物品的对话
        /// </summary>
        Item,
        /// <summary>
        /// 有关选择的对话，主要是主角选择不同对话对应不同的剧情走向
        /// </summary>
        Choose,
        /// <summary>
        /// 这个对话是要触发一个什么效果
        /// </summary>
        Effect,
        /// <summary>
        /// 信息提示，一般是上帝视角，引导玩家干什么
        /// </summary>
        Info,
    }
}
