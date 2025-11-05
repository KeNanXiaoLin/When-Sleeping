using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBase : MonoBehaviour
{
    /// <summary>
    /// 持有的动画控制器
    /// </summary>
    protected Animator m_animator;
    /// <summary>
    /// 持有的状态机
    /// </summary>
    protected StateMachine m_stateMachine;
    public Animator Animator => m_animator;
    /// <summary>
    /// 当前的面朝向，主要用于控制动画
    /// </summary>
    public E_Direction Facing = E_Direction.Right;

    /// <summary>
    /// 根据传入的点，计算当前的面向
    /// </summary>
    /// <param name="targetPos"></param>
    protected void CalDirection(Vector3 targetPos)
    {
        Vector3 v = (targetPos - this.transform.position).normalized;
        //左右
        if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
        {
            if (v.x > 0)
            {
                Facing = E_Direction.Right;
            }
            else
            {
                Facing = E_Direction.Left;
            }
        }
        //上下
        else
        {
            if (v.y > 0)
            {
                Facing = E_Direction.Up;
            }
            else
            {
                Facing = E_Direction.Down;
            }
        }
    }
}
