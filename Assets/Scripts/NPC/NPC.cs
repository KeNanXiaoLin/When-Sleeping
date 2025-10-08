using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPC : MonoBehaviour
{
    public NPCData data;
    public float moveSpeed = 3f;
    public SpriteRenderer sr;

    public void Init()
    {
        this.sr.sprite = data.sprite;
    }

    /// <summary>
    /// 走向主角
    /// </summary>
    /// <param name="target"></param>
    /// <param name="action"></param>
    public void MoveToMainRole(Transform target, UnityAction action)
    {
        StartCoroutine(MoveToMainRoleCoroutine(target, action));
    }

    private IEnumerator MoveToMainRoleCoroutine(Transform target, UnityAction action)
    {
        while (Vector2.Distance(target.position, transform.position) > 1f)
        {
            Vector3 dir = (target.position -transform.position).normalized;
            transform.Translate(dir * moveSpeed * Time.deltaTime);
            yield return null;
        }
        //在走向主角后，触发剧情
        action?.Invoke();
    }

    public DialogData FindDialogData(E_DialogTriggerType type)
    {
        DialogData data = null;
        List<DialogData> allDialogs = this.data.allDialogs;
        //先从玩家的对话数据里找到要触发的对话
        for (int i = 0; i < allDialogs.Count; i++)
        {
            //对话没有触发过，并且对话的触发时机正确
            if (!allDialogs[i].isTrigger && allDialogs[i].triggerType == type)
            {
                data = allDialogs[i];
                break;
            }
        }
        return data;
    }
}
