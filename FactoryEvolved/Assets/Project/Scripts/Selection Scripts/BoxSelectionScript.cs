using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    public class BoxSelectionScript : MonoBehaviour
    {
        [SerializeField] private RectTransform box;
        
        private Vector3 startPosition;
        private Vector3 endPosition;
        private bool started;
        private void Update()
        {
            HandleBoxSelection();
        }

        public void HandleBoxSelection()
        {
            //If we just started box selection then assign start position
            if (!started)
            {
                startPosition = InputManager.Instance.mousePosition;
                box.gameObject.SetActive(true);
                started = true;
            }
            
            CreateBox();
            CheckForUnitsUnderBox();
            
            //Once we let go, reset
            if (Input.GetMouseButtonUp(0))
            {
                started = false;
                box.gameObject.SetActive(false);
                enabled = false;
            }
        }
        
        private void CreateBox()
        {
            endPosition = InputManager.Instance.mousePosition;
            float width = InputManager.Instance.mousePosition.x - startPosition.x;
            float height = InputManager.Instance.mousePosition.y - startPosition.y;

            box.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
            box.anchoredPosition = (startPosition + InputManager.Instance.mousePosition) / 2;
        }
        
        private void CheckForUnitsUnderBox()
        {
            UnitController[] selectableUnits = FindObjectsOfType<UnitController>();

            foreach (var unit in selectableUnits)
            {
                Vector2 unitScreenPos = Camera.main.WorldToScreenPoint(unit.transform.position);
                if(IsWithinBox(unitScreenPos))
                {
                    if (!UnitManager.Instance.selectedUnits.Contains(unit.gameObject))
                    {
                        unit.gameObject.GetComponent<UnitSelectScript>().OnSelect();
                    }
                }
                else
                {
                    if (UnitManager.Instance.selectedUnits.Contains(unit.gameObject))
                    {
                        unit.gameObject.GetComponent<UnitSelectScript>().OnDeselect();
                    }
                }
            }
        }
        
        private bool IsWithinBox(Vector2 unitPos)
        {
            float left = box.anchoredPosition.x - (box.sizeDelta.x) / 2;
            float right = box.anchoredPosition.x + (box.sizeDelta.x) / 2;
            float top = box.anchoredPosition.y + (box.sizeDelta.x) / 2;
            float bottom = box.anchoredPosition.y - (box.sizeDelta.x) / 2;

            if (unitPos.x > left && unitPos.x < right && unitPos.y > bottom && unitPos.y < top)
            {
                return true;
            }

            return false;
        }
    }
}
