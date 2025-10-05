using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJ
{

    public class PlayerJumpState : PlayerMoveState
    {
        private float PlayerJumpForce;
        private Rigidbody2D playerRd;
        private bool IsGroundDetectStart = false;

        private float JumpDetectTime = 1f;

        public PlayerJumpState(PlayerStateMescine stateMachine, Player player, string animatonName) : base(stateMachine, player, animatonName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            PlayerJumpForce = player.GetJumpForce_Player();
            playerRd = player.rd;
            player.isJumping = true;

            JumpDetectTime = 1f;

            //给予向上的力
            playerRd.AddForce(new Vector2(0, PlayerJumpForce));
        }

        public override void Update()
        {
            base.Update();

            JumpDetectTime -= Time.deltaTime;


            //进行地面检测
            if (JumpDetectTime <= 0)
            {
                IsGroundDetectStart = true;
            }

            if (player.GroundDetect() == true && IsGroundDetectStart)
            {
                stateMachine.ChangeState(player.IdleState);
                IsGroundDetectStart = false;
            }

            
        }

        public override void Exit()
        {
            base.Exit();

            player.isJumping = false;
        }
    }

}