using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DamageState : BaseState
{
    public Enemy_DamageState(AIBase npc) : base(npc)
    {
    }

    public override void Enter()
    {
        owner.Animator.Play(Setting.AnimationName_Enemy_Damage);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}
