using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJ
{

    public class V_PlayerMoveState : PlayerState
    {
        public V_PlayerMoveState(PlayerStateMescine _stateMachine, Player _player, string _animatonName) : base(_stateMachine, _player, _animatonName)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }

}