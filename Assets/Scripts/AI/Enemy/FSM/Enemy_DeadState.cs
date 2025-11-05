using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeadState : BaseState
{
    public Enemy_DeadState(AIBase npc) : base(npc)
    {
    }

    public override void Enter()
    {
        owner.Animator.Play(Setting.AnimationName_Enemy_Death);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}
