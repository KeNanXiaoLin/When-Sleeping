using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DamageState : BaseState
{
    private EnemyObj enemyObj;
    public Enemy_DamageState(AIBase npc) : base(npc)
    {
        enemyObj = npc as EnemyObj;
    }

    public override void Enter()
    {
        enemyObj.CanMove = false;
        owner.Animator.SetTrigger(Setting.PlayerAnimationParameter_Enemy_Damage);
    }

    public override void Exit()
    {
        enemyObj.CanMove = true;
    }

    public override void Update()
    {
        // 1. 获取当前动画状态信息（0=默认层，多层需改层索引）
        AnimatorStateInfo damageStateInfo = owner.Animator.GetCurrentAnimatorStateInfo(0);

        // 2. 判断：是否正在播放攻击动画 + 动画已播放完成（normalizedTime >= 1）
        if (damageStateInfo.IsName(Setting.AnimationName_Enemy_Attack) && damageStateInfo.normalizedTime >= 1.0f)
        {
            // 3. 动画播放完成，退出攻击状态，切换到目标状态（移动/待机）
            ExitDamageState();
        }
    }

    private void ExitDamageState()
    {
        // 示例1：如果敌人有目标（如玩家），切换到移动状态（追击）
        if (enemyObj.CanAttackTarget()) // 假设AIBase中有判断是否存在目标的方法
        {
            enemyObj.SwitchState(E_StateType.Battle_Idle);
        }
        // 示例2：如果没有目标，切换到待机动画
        else
        {
            enemyObj.SwitchState(E_StateType.Battle_Move);
        }
    }
}
