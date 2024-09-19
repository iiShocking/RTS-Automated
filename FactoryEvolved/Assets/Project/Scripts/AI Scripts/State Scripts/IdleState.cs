using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    [Serializable]
    public class IdleState : State
    {
        [SerializeField] protected string name;
        [SerializeField] protected GameObject owner;
        [SerializeField] protected MonoBehaviour monoBehaviour;
        
        private bool _isIdle;
        public override void Enter()
        {
            Debug.Log("Entering Idle State");
        }

        public override void Process()
        {
            Debug.Log("Processing Idle State");
        }

        public override void Exit()
        {
            Debug.Log("Exiting Idle State");
        }

        public override string ToString()
        {
            return "IDLE";
        }

        private void DoNothing()
        {
            if (_isIdle)
            {
                return;
            }
            _isIdle = true;
        }
    }
}
