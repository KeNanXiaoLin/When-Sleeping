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
            Vitural = Input.GetAxisRaw("Vertical");

            player.transform.Translate(new Vector2(Horizontal, Vitural)* MoveSpeed * Time.deltaTime);

            if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            {
                stateMachine.ChangeState(player.IdleState);
            }

            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.J))
            {
                EventListener.PlayerDamage();
                //TODO 为攻击添加冷却时间
            }

            if (Input.GetKeyDown(KeyCode.Space))
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