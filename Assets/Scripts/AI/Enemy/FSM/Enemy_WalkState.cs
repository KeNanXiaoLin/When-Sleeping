using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_WalkState : BaseState
{
    public Enemy_WalkState(AIBase npc) : base(npc)
    {
    }

    public override void Enter()
    {
        owner.Animator.SetBool(Setting.PlayerAnimationParameter_Enemy_IsMove, true);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}
