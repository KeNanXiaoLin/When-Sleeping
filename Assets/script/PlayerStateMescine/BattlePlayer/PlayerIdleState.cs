using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJ
{

    public class PlayerIdleState : PlayerState
    {
        public PlayerIdleState(PlayerStateMescine stateMachine, Player player, string animatonName) : base(stateMachine, player, animatonName)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();

            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                stateMachine.ChangeState(player.MoveState);
            }

            //攻击操作
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.J) && player.isAttacking == false)
            {
                stateMachine.ChangeState(player.AttackState);
                //TODO 为攻击添加冷却时间
            }

            //空格点击操作
            if (Input.GetKeyDown(KeyCode.Space) && player.isJumping == false)
            {
                stateMachine.ChangeState(player.JumpState);
                
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

    }
}
