using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KNXL.DialogSystem
{
    public class RoleDialogData
    {
        public int id;
        /// <summary>
        /// 拥有这段对话的主人
        /// </summary>
        public E_DialogRoleType owner;
        /// <summary>
        /// 这段对话的所有对话数据
        /// </summary>
        public int startDialog;
        /// <summary>
        /// 这段对话是否可以重复触发
        /// 比如场景中一些npc的对话就可以一直触发，关于剧情的内容只能触发一次
        /// </summary>
        public bool canTriggerRepeat = false;
        /// <summary>
        /// 要解锁这个对话所需要的前置对话，如果是null，不需要前置可以直接解锁
        /// 为了方便扩展，这里设计成为一个列表
        /// </summary>
        public int preRoleDialogs = -1;
        /// <summary>
        /// 是否是剧情对话
        /// </summary>
        public bool isPlotDialog = true;
        /// <summary>
        /// 为对话添加的描述，只是为了配置方便查看的，没有其他作用
        /// </summary>
        public string dialogDes;
        /// <summary>
        /// 这个对话是否已经触发过
        /// </summary>
        public bool isTrigger = false;
    }
}
