using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJ
{
    public class PlayerAttackState : PlayerMoveState    //攻击时可移动
    {
        public float AttackTime;

        public PlayerAttackState(PlayerStateMescine stateMachine, Player player, string animatonName) : base(stateMachine, player, animatonName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            AttackTime = player.GetAttackTime_Player();
            player.isAttacking = true;
            
            EventListener.PlayerDamage();
        }

        public override void Update()
        {
            base.Update();

            // AttackTime -= Time.deltaTime;
            // if (AttackTime <= 0) stateMachine.ChangeState(player.IdleState);
        }

        public override void Exit()
        {
            base.Exit();

            player.isAttacking = false;
        }

    }

}