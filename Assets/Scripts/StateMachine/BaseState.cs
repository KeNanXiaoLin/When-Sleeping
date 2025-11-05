using UnityEngine;

public abstract class BaseState
{
    protected AIBase owner;
    public BaseState(AIBase npc)
    {
        owner = npc;
    }

    // 当状态被进入时调用
    public abstract void Enter();

    // 每帧更新时调用
    public abstract void Update();

    // 当状态被退出时调用
    public abstract void Exit();
}
