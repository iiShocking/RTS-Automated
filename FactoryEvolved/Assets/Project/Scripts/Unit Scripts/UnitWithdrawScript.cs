using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    public class UnitWithdrawScript : MonoBehaviour
    {
        [SerializeField] private List<GameObject> nearbyWithdrawObjects;
        [SerializeField] private float nearbyRadius;

        private UnitResourceScript _unitResourceScript;
        private UnitUIHandler _unitUIHandler;

        private void Start()
        {
            _unitResourceScript = GetComponent<UnitResourceScript>();
            _unitUIHandler = GetComponent<UnitUIHandler>();
        }

        [ContextMenu("Withdraw")]
        public void Withdraw()
        {
            CheckForNearbyWithdraws();
            WithdrawFromNearby();
            _unitUIHandler.UpdateAll();
        }
        
        private void WithdrawFromNearby()
        {
            foreach (var node in nearbyWithdrawObjects)
            {
                if (node.CompareTag("Storage"))
                {
                    StorageNode nodeScript = node.GetComponent<StorageNode>();
                    if (nodeScript.storedItem.Amount == 0) return;
                    if (CheckIfHeld(nodeScript.storedItem.Name))
                    {
                        _unitResourceScript.IncreaseAmount(nodeScript.storedItem);
                    }
                }
                
                if (node.CompareTag("Processor"))
                {
                    ProcessingNode nodeScript = node.GetComponent<ProcessingNode>();
                    if (nodeScript.resourceOutput.Amount == 0) return;
                    if (CheckIfHeld(nodeScript.resourceOutput.Name))
                    {
                        _unitResourceScript.IncreaseAmount(nodeScript.resourceOutput);
                    }
                }

                if (node.CompareTag("Node"))
                {
                    ResourceNode nodeScript = node.GetComponent<ResourceNode>();
                    if (nodeScript.GetResource().Amount == 0) return;
                    if (CheckIfHeld(nodeScript.GetResource().Name))
                    {
                        _unitResourceScript.IncreaseAmount(nodeScript.GetResource());
                    }
                }
            }
        }

        private bool CheckIfHeld(string check)
        {
            if (_unitResourceScript.heldResource.Name == "" || _unitResourceScript.heldResource == null)
            {
                _unitResourceScript.SetResource(check);
            }
            
            if (_unitResourceScript.heldResource.Name == check)
            {
                return true;
            }

            return false;
        }
        
        private void CheckForNearbyWithdraws()
        {
            nearbyWithdrawObjects.Clear();
            var nearbyObjects = Physics.OverlapSphere(transform.position, nearbyRadius);

            foreach (var nearby in nearbyObjects)
            {
                if (nearby.gameObject.CompareTag("Node"))
                {
                    nearbyWithdrawObjects.Add(nearby.gameObject);
                }
                
                if (nearby.gameObject.CompareTag("Processor"))
                {
                    nearbyWithdrawObjects.Add(nearby.gameObject);
                }

                if (nearby.gameObject.CompareTag("Storage"))
                {
                    nearbyWithdrawObjects.Add(nearby.gameObject);
                }
            }
        }
    }
}
