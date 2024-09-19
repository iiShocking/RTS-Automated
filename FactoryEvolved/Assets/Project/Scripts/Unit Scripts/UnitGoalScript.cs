using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    public class UnitGoalScript : MonoBehaviour
    {
        private List<string> FoodStrings;
        [SerializeField] private float radius;
        [SerializeField] private GameObject destinationObject;
        [SerializeField] public string goal;
        private UnitMovement _unitMovement;
        private UnitStatsScript _unitStatsScript;
        private UnitController _unitController;
        private Vector3 storedPosition;

        public GameObject enemyToAttack;
        private bool isStalled;
        private void Start()
        {
            //Do stuff
            _unitMovement = GetComponent<UnitMovement>();
            _unitStatsScript = GetComponent<UnitStatsScript>();
            _unitController = GetComponent<UnitController>();
            FoodStrings = new List<string>();
            AssignFoods();
            goal = "Nothing";
        }

        public void SetGoal(string settingGoal) => goal = settingGoal;

        private void AssignFoods()
        {
            FoodStrings.Add("Berries");
        }

        public void AttackEnemy(GameObject enemy)
        {
            if (isStalled) return;
            //Refactor
            goal = "Attack";
            enemyToAttack = enemy;
            //Go to enemy
            destinationObject = enemy.GetComponent<AIAnchorScript>().GetRandomAnchorPosition();
            GoToDestination();
            //Attack enemy
            _unitMovement.unitArrived.AddListener(PerformGoal);
        }
    
        [ContextMenu("Find Water")]
        public bool FindWater()
        {
            goal = "Water";
            var nearbyStorages = FindNearbyStorages();

            foreach (var storage in nearbyStorages)
            {
                if (storage.storedItem.Name == Items.Tier1.Water.ToString() && storage.storedItem.Amount >= 10)
                {
                    destinationObject = storage.gameObject;
                    storedPosition = transform.position;
                    GoToDestination();
                    _unitMovement.unitArrived.AddListener(PerformGoal);
                    return true;
                }
                print("No water found in this storage");
            }
            return false;
        }

        public bool FindFood()
        {
            goal = "Food";
            var nearbyStorages = FindNearbyStorages();

            foreach (var storage in nearbyStorages)
            {
                if (FoodStrings.Contains(storage.storedItem.Name) && storage.storedItem.Amount >= 10)
                {
                    destinationObject = storage.gameObject;
                    storedPosition = transform.position;
                    GoToDestination();
                    _unitMovement.unitArrived.AddListener(PerformGoal);
                    return true;
                }
                print("No food found in this node");
            }

            return false;
        }

        public void GoToDestination()
        {
            _unitMovement.MoveToPoint(destinationObject.transform.position);
        }

        public void ReturnToOriginal()
        {
            _unitMovement.MoveToPoint(storedPosition);
            _unitController.SetHasTask(false);
        }

        public void PerformGoal()
        {
            _unitMovement.unitArrived.RemoveListener(PerformGoal);
            print("Performing Goal");
            switch (goal)
            {
                case "Water":
                    if (!FindWater()) return;
                    
                    _unitStatsScript.IncreaseThirst(100);
                    destinationObject.GetComponent<StorageNode>().Consume(10);
                    goal = "Returning";
                    ReturnToOriginal();
                    break;
                case "Food":
                    _unitStatsScript.IncreaseHunger(100);
                    destinationObject.GetComponent<StorageNode>().Consume(10);
                    goal = "Returning";
                    ReturnToOriginal();
                    break;
                case "Attack":
                    if (isStalled) return;
                    print("Attacking");
                    //Handle attack logic
                    HealthScript enemy = enemyToAttack.GetComponent<HealthScript>();
                    if (enemy.isDead)
                    {
                        goal = "Nothing";
                        _unitMovement.target = null;
                        _unitMovement.ResetPath();    
                        _unitController._unitAnimationHandler.PlayAnimation("Idle");
                        StopAllCoroutines();
                        _unitStatsScript.IncreaseHunger(65);
                        return;
                    }
                    
                    GoToDestination();
                    _unitMovement.target = enemyToAttack;
                    _unitMovement.RotateTowardsTarget();
                    _unitController._unitAnimationHandler.TriggerAnimation("Attack");
                    isStalled = true;
                    StartCoroutine(DamageAfterSeconds(1f, enemy, 20));
                    StartCoroutine(RepeatGoal(2.6f));
                    break;
                default:
                    print("No goal");
                    break;
            }
        }
        private List<StorageNode> FindNearbyStorages()
        {
            var nearbyObjectsColliders = Physics.OverlapSphere(transform.position, radius);
            List<StorageNode> nearbyStorages = new List<StorageNode>();
            foreach (var item in nearbyObjectsColliders)
            {
                if (item.gameObject.CompareTag("Storage"))
                {
                    nearbyStorages.Add(item.GetComponent<StorageNode>());
                }
            }

            return nearbyStorages;
        }

        private IEnumerator RepeatGoal(float duration)
        {
            print("Repeating goal");
            yield return new WaitForSeconds(duration);
            PerformGoal();
        }

        private IEnumerator DamageAfterSeconds(float seconds, HealthScript enemy, float damage)
        {
            yield return new WaitForSeconds(seconds);
            enemy.TakeDamage(damage);
            isStalled = false;

        }
    }
}
