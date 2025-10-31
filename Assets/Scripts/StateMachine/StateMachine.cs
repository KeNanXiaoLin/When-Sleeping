using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    // 当前状态
    private BaseState _currentState;

    // 状态字典，用于存储和快速查找状态
    private Dictionary<E_StateType, BaseState> _states = new Dictionary<E_StateType, BaseState>();

    public E_StateType CurrentStateType = E_StateType.None;

    // 切换到指定状态
    public void ChangeState(E_StateType stateName)
    {
        if (!_states.ContainsKey(stateName))
        {
            Debug.LogError($"状态机中不存在名为 {stateName} 的状态");
            return;
        }
        if (CurrentStateType == stateName)
        {
            Debug.Log("当前正处于这个状态，不需要切换");
            return;
        }
        CurrentStateType = stateName;

        // 退出当前状态
        _currentState?.Exit();

        // 切换到新状态并进入
        _currentState = _states[stateName];
        _currentState.Enter();
    }

    // 添加状态到状态机
    public void AddState(E_StateType stateName, BaseState state)
    {
        if (!_states.ContainsKey(stateName))
        {
            _states.Add(stateName, state);
        }
        else
        {
            Debug.LogWarning($"状态机中已存在名为 {stateName} 的状态，将被覆盖");
            _states[stateName] = state;
        }
    }

    // 更新当前状态
    public void Update()
    {
        _currentState?.Update();
    }
}
