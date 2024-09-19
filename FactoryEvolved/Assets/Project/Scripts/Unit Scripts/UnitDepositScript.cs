using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    public class UnitDepositScript : MonoBehaviour
    {
        [SerializeField] private List<GameObject> nearbyDepositObjects;
        [SerializeField] private float nearbyRadius;

        private UnitResourceScript _unitResourceScript;
        private UnitUIHandler _unitUIHandler;
        private void Start()
        {
            _unitResourceScript = GetComponent<UnitResourceScript>();
            _unitUIHandler = GetComponent<UnitUIHandler>();
        }
        
        [ContextMenu("Deposit")]
        public void Deposit()
        {
            CheckForNearbyDeposits();
            DepositIntoNearby();
            _unitUIHandler.UpdateAll();
        }
        
        private void DepositIntoNearby()
        {
            foreach (var node in nearbyDepositObjects)
            {
                if (node.CompareTag("Storage"))
                {
                    print("Found a storage to deposit into");
                    StorageNode nodeScript = node.GetComponent<StorageNode>();
                    
                    if (CheckIfHeld(nodeScript.storedItem.Name) || nodeScript.storedItem.Name == "")
                    {
                        nodeScript.TransferInto(_unitResourceScript.heldResource);
                        _unitResourceScript.TryResetHeldResource();
                    }
                }

                if (node.CompareTag("Processor"))
                {
                    print("Found a processor to deposit into");
                    ProcessingNode nodeScript = node.GetComponent<ProcessingNode>();
                    if (CheckIfHeld(nodeScript.resourceInput.Name))
                    {
                        nodeScript.TransferInto(_unitResourceScript.heldResource);
                        _unitResourceScript.TryResetHeldResource();
                    }
                }
            }
        }

        private bool CheckIfHeld(string check)
        {
            if (_unitResourceScript.heldResource.Name == check)
            {
                print("Can deposit into it!");
                return true;
            }
            return false;
        }

        private void CheckForNearbyDeposits()
        {
            nearbyDepositObjects.Clear();
            
            var nearbyObjects = Physics.OverlapSphere(transform.position, nearbyRadius);

            foreach (var nearby in nearbyObjects)
            {
                if (nearby.gameObject.CompareTag("Storage"))
                {
                    nearbyDepositObjects.Add(nearby.gameObject);
                }
                if (nearby.gameObject.CompareTag("Processor"))
                {
                    nearbyDepositObjects.Add(nearby.gameObject);
                }
            }
        }
    }
}
