using System.Collections;
using System.Collections.Generic;
using GJ;
using UnityEngine;

namespace GJ
{

    public class PlayerStateMescine : MonoBehaviour
    {
        public PlayerState CurrentState { get; private set; }

        public void Initialize(PlayerState _StartState)
        {
            CurrentState = _StartState;
            CurrentState.Enter();
        }

        public void ChangeState(PlayerState _AimState)
        {
            CurrentState.Exit();
            CurrentState = _AimState;
            CurrentState.Enter();
        }
    }
}