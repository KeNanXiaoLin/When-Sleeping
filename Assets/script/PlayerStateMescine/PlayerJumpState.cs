using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJ
{

    public class PlayerJumpState : PlayerMoveState
    {
        //FIXME 修复连跳问题
        private float PlayerJumpForce;
        private Rigidbody2D playerRd;
        private bool IsGroundDetectStart = false;

        public PlayerJumpState(PlayerStateMescine stateMachine, Player player, string animatonName) : base(stateMachine, player, animatonName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            PlayerJumpForce = player.GetJumpForce_Player();
            playerRd = player.rd;

            //给予向上的力
            playerRd.AddForce(new Vector2(0, PlayerJumpForce));
        }

        public override void Update()
        {
            base.Update();

            //进行地面检测
            if (player.GroundDetect() == false)
            {
                IsGroundDetectStart = true;
                Debug.Log("Check Start");

            }
            else if (player.GroundDetect() == true && IsGroundDetectStart)
            {
                stateMachine.ChangeState(player.IdleState);
                IsGroundDetectStart = false;
                Debug.Log("Exit Jump");
            }

            //允许左右移动

            
        }

        public override void Exit()
        {
            base.Exit();
        }
    }

}