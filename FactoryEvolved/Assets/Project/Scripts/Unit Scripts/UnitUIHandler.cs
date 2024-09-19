using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FactoryEvolved
{
    public class UnitUIHandler : MonoBehaviour
    {
        [SerializeField] private GameObject ui;
        [SerializeField] private TMP_Text unitNo;
        [SerializeField] private TMP_Text resourceQuantity;
        [SerializeField] private TMP_Text resourceName;
        
        [SerializeField] private List<GameObject> queuePoints;
        [SerializeField] private GameObject queuePointGameObject;

        [SerializeField] private Slider hungerSlider;
        [SerializeField] private Slider thirstSlider;
        
        private UnitResourceScript _unitResourceScript;
        private UnitMovement _unitMovement;
        private UnitStatsScript _unitStatsScript;

        private void Start()
        {
            _unitResourceScript = GetComponent<UnitResourceScript>();
            _unitMovement = GetComponent<UnitMovement>();
            _unitStatsScript = GetComponent<UnitStatsScript>();
            queuePoints = new List<GameObject>();
            DefaultSetup();
        }

        public void UpdateUnitRadials()
        {
            thirstSlider.value = _unitStatsScript.GetThirst() / 100;
            hungerSlider.value = _unitStatsScript.GetHunger() / 100;
        }
        public void DisplayUI()
        {
            UpdateAll();
            ui.SetActive(true);
            DisplayQueuePoints();
        }

        public void HideUI()
        {
            ui.SetActive(false);
            HideQueuePoints();
        }

        private void DefaultSetup()
        {
            resourceQuantity.text = "";
            resourceName.text = "";
            HideUI();
        }
        public void UpdateAll()
        {
            UpdateName();
            UpdateQuantity();
            //UpdateUnitNo();
        }

        private void UpdateUnitNo()
        {
            unitNo.text = UnitManager.Instance.IndexOfUnit(gameObject).ToString();
        }

        private void UpdateQuantity()
        {
            if (_unitResourceScript.heldResource == null)
            {
                resourceQuantity.text = "";
                return;
            }
            resourceQuantity.text = _unitResourceScript.heldResource.Amount.ToString();
        }

        private void UpdateName()
        {
            if (_unitResourceScript.heldResource == null)
            {
                resourceName.text = "";
                return;
            }
            resourceName.text = _unitResourceScript.heldResource.Name;
        }

        private void DisplayQueuePoints()
        {
            foreach (var point in queuePoints)
            {
                point.SetActive(true);
            }
        }

        private void HideQueuePoints()
        {
            foreach (var point in queuePoints)
            {
                point.SetActive(false);
            }
        }


        public void AddQueuePoint(Vector3 position)
        {
            var newObject = Instantiate(queuePointGameObject, position, Quaternion.identity);
            queuePoints.Add(newObject);
        }

        public void RemoveQueuePoint()
        {
            var removal = queuePoints[0];
            queuePoints.RemoveAt(0);
            Destroy(removal);
        }

        public void ClearQueuePoints()
        {
            foreach (var point in queuePoints)
            {
                Destroy(point);
            }
            queuePoints.Clear();
        }
    }
}
