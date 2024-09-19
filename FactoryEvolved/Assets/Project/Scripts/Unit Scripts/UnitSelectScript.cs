using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    public class UnitSelectScript : MonoBehaviour, ISelectable
    {
        [SerializeField] private GameObject floorSprite;
        private UnitUIHandler _unitUIHandler;

        private void Start()
        {
            _unitUIHandler = GetComponent<UnitUIHandler>();
        }

        public void OnSelect()
        {
            floorSprite.SetActive(true);
            DisplayUI();
            UnitManager.Instance.SelectUnit(gameObject);
        }

        public void OnDeselect()
        {
            floorSprite.SetActive(false);
            HideUI();
            UnitManager.Instance.DeselectUnit(gameObject);
        }

        public void OnHighlight()
        {
            throw new System.NotImplementedException();
        }

        public void OnUnhighlight()
        {
            throw new System.NotImplementedException();
        }

        public void DisplayUI()
        {
            _unitUIHandler.DisplayUI();
        }

        public void HideUI()
        {
            _unitUIHandler.HideUI();
        }
    }
}
