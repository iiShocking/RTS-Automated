using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    public class UnitCargoScript : MonoBehaviour
    {
        private ObjectSelection _objectSelection;
        private UnitMovement _unitMovement;
        private UnitWithdrawScript _unitWithdrawScript;
        private UnitDepositScript _unitDepositScript;
        
        [SerializeField] private GameObject fromNode;
        [SerializeField] private GameObject toNode;

        [SerializeField] private int status;

        private void Start()
        {
            _objectSelection = FindObjectOfType<ObjectSelection>();
            _unitMovement = GetComponent<UnitMovement>();
            _unitWithdrawScript = GetComponent<UnitWithdrawScript>();
            _unitDepositScript = GetComponent<UnitDepositScript>();
        }

        public void BeginAssign()
        {
            _objectSelection.objectSelected.AddListener(WaitForFirstAssign);
        }

        private void WaitForFirstAssign()
        {
            fromNode = _objectSelection.GetSelectedObject();
            _objectSelection.objectSelected.RemoveListener(WaitForFirstAssign);
            _objectSelection.objectSelected.AddListener(WaitForSecondAssign);
        }

        private void WaitForSecondAssign()
        {
            toNode = _objectSelection.GetSelectedObject();
            _objectSelection.objectSelected.RemoveListener(WaitForSecondAssign);
            PerformCargoRoute();
        }

        public void PerformCargoRoute()
        {
            _unitMovement.MoveToPoint(fromNode.transform.position);
            status = 2;
            StartCoroutine(CheckForArrival());
        }

        private void MoveToFromNode()
        {
            _unitMovement.MoveToPoint(fromNode.transform.position);
            status = 2;
            StartCoroutine(CheckForArrival());
        }

        private void MoveToToNode()
        {
            _unitMovement.MoveToPoint(toNode.transform.position);
            status = 1;
            StartCoroutine(CheckForArrival());
        }

        private IEnumerator CheckForArrival()
        {
            if (!_unitMovement.hasArrived)
            {
                yield return new WaitForSeconds(3);
                StartCoroutine(CheckForArrival());
            }
            else
            {
                if (status == 1)
                {
                    _unitDepositScript.Deposit();
                    MoveToFromNode();
                }
                else if (status == 2)
                {
                    _unitWithdrawScript.Withdraw();
                    MoveToToNode();
                }
                else
                {
                    print("Undefined status");
                }
            }
            
        }
    }
}
