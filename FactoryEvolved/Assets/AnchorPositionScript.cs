using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    public class AnchorPositionScript : MonoBehaviour
    {
        [SerializeField]
        private ResourceNode assignedNode;
        [SerializeField] private GameObject assignedUnit;
        [SerializeField] private GameObject nodeModel;
        
        [SerializeField] public bool isInUse;
        [SerializeField] public bool troopEnRoute;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Unit") && !isInUse && troopEnRoute)
            {
                print("Anchor Triggered");
                isInUse = true;
                assignedUnit = other.gameObject;
                assignedNode.IncreaseAssigned();
                other.GetComponent<UnitMovement>().target = nodeModel;  
                other.GetComponent<UnitAnimationHandler>().PlayAnimation("Mining");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == assignedUnit)
            {
                isInUse = false;
                assignedUnit = null;
                assignedNode.DecreaseAssigned();
                other.GetComponent<UnitMovement>().target = null;  
            }
        }

        public void UnitIsOnTheWay()
        {
            StartCoroutine(UnitOnTheWay());
        }
        
        private IEnumerator UnitOnTheWay()
        {
            troopEnRoute = true;
            yield return new WaitForSeconds(6);
            troopEnRoute = false;
        }
    }
}
