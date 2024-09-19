using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    public class UnitController : MonoBehaviour
    {
        private UnitMovement _unitMovementScript;
        private UnitDepositScript _unitDepositScript;
        private UnitWithdrawScript _unitWithdrawScript;
        private UnitStatsScript _unitStatsScript;
        private UnitGoalScript _unitGoalScript;
        public UnitSelectScript unitSelectScript;
        public UnitAnimationHandler _unitAnimationHandler;

        [SerializeField] private bool hasTask;

        private void Awake()
        {
            _unitMovementScript = GetComponent<UnitMovement>();
            _unitDepositScript = GetComponent<UnitDepositScript>();
            _unitWithdrawScript = GetComponent<UnitWithdrawScript>();
            _unitStatsScript = GetComponent<UnitStatsScript>();
            _unitGoalScript = GetComponent<UnitGoalScript>();
            _unitAnimationHandler = GetComponent<UnitAnimationHandler>();
            unitSelectScript = GetComponent<UnitSelectScript>();
        }

        private void Start()
        {
            TickManager.Instance.Subscribe(HandleAll, 1);
        }

        private void HandleAll()
        {
            HandleStats();
            HandleMovement();
            //HandleGoal();
        }

        private void HandleMovement()
        {
            if (_unitMovementScript._destinationQueue.Count != 0)
            {
                _unitMovementScript.MoveToPoint(_unitMovementScript._destinationQueue.Peek());
            }
        }

        private void HandleGoal()
        {
            _unitGoalScript.PerformGoal();
        }

        private void HandleStats()
        {
            _unitStatsScript.DecreaseStats();
            
            if (_unitStatsScript.GetHunger() <= 1 || _unitStatsScript.GetThirst() <= 1)
            {
                _unitStatsScript.Die();
            }
            
            if (_unitStatsScript.GetThirst() < 20)
            {
                if (hasTask) return;
                
                if (_unitGoalScript.FindWater())
                {
                    hasTask = true;
                }
                else
                {
                    BeginTaskCooldown();
                }
            }
            
            if (_unitStatsScript.GetHunger() < 20)
            {
                if (hasTask) return;
                
                if (_unitGoalScript.FindFood())
                {
                    hasTask = true;
                }
                else
                {
                    BeginTaskCooldown();
                }
            }
        }
        
        public void SetHasTask(bool value) => hasTask = value;

        private void BeginTaskCooldown() => StartCoroutine(TaskCooldown());

        private IEnumerator TaskCooldown()
        {
            hasTask = true;
            _unitGoalScript.goal = "On Cooldown";
            yield return new WaitForSeconds(6);
            hasTask = false;
            _unitGoalScript.goal = "No Current Task";
        }
    }
}
