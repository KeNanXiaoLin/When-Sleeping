using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJ
{

    public class PlayerMoveState : PlayerState
    {
        private float Horizontal;
        private float Vitural;
        private float MoveSpeed;

        public PlayerMoveState(PlayerStateMescine stateMachine, Player player, string animatonName) : base(stateMachine, player, animatonName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            MoveSpeed = player.GetMoveSpeed_Player();
        }

        public override void Update()
        {
            base.Update();
            Horizontal = Input.GetAxisRaw("Horizontal");

            player.transform.Translate(new Vector2(Horizontal, 0)* MoveSpeed * Time.deltaTime);

            //左右移动
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                stateMachine.ChangeState(player.IdleState);
            }

            //攻击操作
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.J) && stateMachine.CurrentState != player.AttackState)
            {
                stateMachine.ChangeState(player.AttackState);
                //TODO 为攻击添加冷却时间
            }

            //空格点击操作
            if (Input.GetKeyDown(KeyCode.Space) && stateMachine.CurrentState != player.JumpState)
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