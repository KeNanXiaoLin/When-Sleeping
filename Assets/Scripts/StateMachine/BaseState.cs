using UnityEngine;

public abstract class BaseState
{
    protected NPCController owner;
    public BaseState(NPCController npc)
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
