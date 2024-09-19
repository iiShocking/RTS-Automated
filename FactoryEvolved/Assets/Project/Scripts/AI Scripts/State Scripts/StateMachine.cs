using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    public class StateMachine
    {
        [SerializeField] private State currentState;
        public bool isTransitioning;

        public void ChangeState(State newState)
        {
            currentState?.Exit();

            currentState = newState;
            currentState.Enter();
        }

        public State GetCurrentState() => currentState;

        public string GetStateName() => currentState.ToString();
    }
}
