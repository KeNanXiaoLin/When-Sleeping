using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJ
{

    public class PlayerStopState : PlayerState
    {
        public PlayerStopState(PlayerStateMescine _stateMachine, Player _player, string _animatonName) : base(_stateMachine, _player, _animatonName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            if (player.GetCCPlayerHealth_Player() <= 0) EventListener.GameLose();
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