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

        public PlayerState(PlayerStateMescine stateMachine, Player player, string animatonName)
        {
            this.stateMachine = stateMachine;
            this.player = player;
            this.AnimatonName = animatonName;
        }

        public virtual void Enter()
        {
            
        }

        public virtual void Update()
        {

        }
        public virtual void Exit()
        {

        }
    }
}
