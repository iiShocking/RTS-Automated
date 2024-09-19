using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FactoryEvolved
{
    public class StateController : MonoBehaviour
    {
        private StateMachine _stateMachine;
        [SerializeField] private string currentStateName;

        private void Start()
        {
            _stateMachine = new StateMachine();
            _stateMachine.ChangeState(new WanderState(gameObject.GetComponent<NavMeshAgent>(), gameObject.transform, 3));
            currentStateName = _stateMachine.GetStateName();
        }

        private void FixedUpdate()
        {
            if (_stateMachine.isTransitioning) return;
            
            _stateMachine.GetCurrentState()?.Process();
            currentStateName = _stateMachine.GetStateName();
        }

        [ContextMenu("Switch to idle")]
        public void SwitchToIdle()
        {
            StartCoroutine(StartTransition());
            _stateMachine.ChangeState(new IdleState());
        }

        [ContextMenu("Switch to wander")]
        public void SwitchToWander()
        {
            StartCoroutine(StartTransition());
            _stateMachine.ChangeState(new WanderState(gameObject.GetComponent<NavMeshAgent>(), gameObject.transform, 3));
        }

        private IEnumerator StartTransition()
        {
            if (_stateMachine.isTransitioning) yield break;
            
            _stateMachine.isTransitioning = true;
            yield return new WaitForSeconds(2);
            _stateMachine.isTransitioning = false;
        }
    }
}
