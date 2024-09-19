using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FactoryEvolved
{
    public class ObjectSelection : MonoBehaviour
    {
        [SerializeField] private GameObject currentlySelectedObject;
        public UnityEvent objectSelected;

        public void SelectObject(GameObject clickedObject)
        {
            Deselect();
            currentlySelectedObject = clickedObject;
            currentlySelectedObject.GetComponent<ISelectable>().OnSelect();
            objectSelected.Invoke();
        }

        public void Deselect()
        {
            if (currentlySelectedObject == null) return;
            currentlySelectedObject.GetComponent<ISelectable>().OnDeselect();
            currentlySelectedObject = null;
        }

        public GameObject GetSelectedObject() => currentlySelectedObject;
    }
}
