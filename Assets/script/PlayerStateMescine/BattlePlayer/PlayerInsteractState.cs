using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJ
{
    public class PlayerInsteractState : PlayerState
    {
        public PlayerInsteractState(PlayerStateMescine stateMachine, Player player, string animatonName) : base(stateMachine, player, animatonName)
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