using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace FactoryEvolved
{
    public class UnitMovement : MonoBehaviour
    {
        public Queue<Vector3> _destinationQueue;
        private NavMeshAgent _navMeshAgent;
        private UnitUIHandler _unitUIHandler;
        private UnitAnimationHandler _animationHandler;
        public bool hasArrived = true;
        public UnityEvent unitArrived;
        public GameObject target;
        public bool moving;
        private bool flag;
        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _unitUIHandler = GetComponent<UnitUIHandler>();
            _animationHandler = GetComponent<UnitAnimationHandler>();
            _destinationQueue = new Queue<Vector3>();
        }

        private void FixedUpdate()
        {
            if(!_navMeshAgent.pathPending && !_navMeshAgent.hasPath)
            {
                if (!hasArrived)
                {
                    hasArrived = true;
                    EventInvoker();
                }
            }
            else
            {
                hasArrived = false;
                flag = false;
            }

            if (target != null)
            {
                RotateTowardsTarget();
            }
        }

        public void MoveToPoint(Vector3 point)
        {
            hasArrived = false;
            moving = true;
            _animationHandler.PlayAnimation("Walking");
            _navMeshAgent.destination = point;
        }

        public void ResetPath() => _navMeshAgent.ResetPath();

        public void QueueUpPoint(Vector3 point)
        {
            _destinationQueue.Enqueue(point);
            _unitUIHandler.AddQueuePoint(point);
        }

        private void EventInvoker()
        {
            if (!flag)
            {
                unitArrived.Invoke();
                print(gameObject.name + " HAS ARRIVED");
                moving = false;
                if (_animationHandler.currentAnimation == "Walking")
                {
                    _animationHandler.PlayAnimation("Idle");
                }
                //On arrival let's dequeue the position from the queue of points
                Pop();
                flag = true;
            }
        }

        public void ClearQueue()
        {
            _destinationQueue.Clear();
            _unitUIHandler.ClearQueuePoints();
        }
        
        private void Pop()
        {
            //If there isn't a queue, leave
            if (_destinationQueue.Count == 0) return;

            _destinationQueue.Dequeue();
            _unitUIHandler.RemoveQueuePoint();
        }
        
        public void RotateTowardsTarget()
        {
            print("Rotating");
            Vector3 targetDirection = target.transform.position - transform.position;
            targetDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            print(targetRotation + gameObject.name);
            transform.rotation = targetRotation; // Just snap to the target
        }
    }
}
