using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FactoryEvolved
{
    public class WanderState : State
    {
        private Vector3 _destination;
        
        private bool _onCooldown;
        private float _wanderCooldown;
        private int _wanderRadius;

        private NavMeshAgent _navMeshAgent;
        private Transform _transform;

        private float _timer;
        
        public WanderState(NavMeshAgent agent, Transform transform, int wanderRadius)
        {
            _navMeshAgent = agent;
            _transform = transform;
            _wanderRadius = wanderRadius;
        }

        public override string ToString()
        {
            return "WANDER";
        }

        public override void Enter()
        {
            FindNewLocation();
        }

        public override void Process()
        {
            _timer += Time.deltaTime;

            if(_onCooldown)
            {
                if (_timer >= _wanderCooldown)
                {
                    FindNewLocation();
                    _onCooldown = false;
                    _timer = 0;
                }
            }
            else
            {
                FindNewLocation();
            }
        }

        public override void Exit()
        {
            _navMeshAgent.ResetPath();
            _onCooldown = false;
            _timer = 0;
            _wanderCooldown = 0;
        }

        private void FindNewLocation()
        {
            var seed = System.DateTime.Now.Second;

            System.Random random = new System.Random(seed);

            var localPosition = _transform.position;
            
            float randomX = (float)(random.NextDouble() * (_wanderRadius * 2) - _wanderRadius) + localPosition.x;
            float randomZ = (float)(random.NextDouble() * (_wanderRadius * 2) - _wanderRadius) + localPosition.z;

            Vector3 randomPosition = new Vector3(randomX, localPosition.y, randomZ);
            
            _destination = randomPosition;
            MoveToDestination();
        }

        private void MoveToDestination()
        {
            _navMeshAgent.SetDestination(_destination);
            StartCooldown();
        }

        private void StartCooldown()
        {
            _wanderCooldown = Random.Range(0f, 7.25f);
            _onCooldown = true;
        }
    }
}
