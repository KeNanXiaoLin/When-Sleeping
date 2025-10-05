using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GJ
{

    public class PlayerOpenBackpackState : PlayerState
    {
        private List<Button> UISlot = new List<Button>();

        public PlayerOpenBackpackState(PlayerStateMescine stateMachine, Player player, string animatonName) : base(stateMachine, player, animatonName)
        {

        }

        public override void Enter()
        {
            base.Enter();
            LA_Backpack.instence.OpenBackpack();

            for (int i = 0; i < LA_Backpack.instence.Backpack.Count; i++)
            {
                UISlot.Add(LA_Backpack.instence.UISlot_Backpack[i].GetComponent<Button>());
                //TODO 完成背包系统
            }
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