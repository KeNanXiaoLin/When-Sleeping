using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GJ
{

    public class PlayerState
    {
        public PlayerStateMescine stateMachine;
        public Player player;
        public string AnimatonName;

        public PlayerState(PlayerStateMescine _stateMachine, Player _player, string _animatonName)
        {
            this.stateMachine = _stateMachine;
            this.player = _player;
            this.AnimatonName = _animatonName;
        }

        public virtual void Enter()
        {
            player.anim.SetBool(AnimatonName, true);
        }

        public virtual void Update()
        {

        }
        public virtual void Exit()
        {
            player.anim.SetBool(AnimatonName, false);
        }
    }
}
