using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FactoryEvolved
{
    public class SelectionManager : MonoBehaviour
    {
        public static SelectionManager Instance { get; private set; }

        private BoxSelectionScript _boxSelectionScript;
        private ObjectSelection _objectSelection;

        //Singleton
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        //Assign scripts
        private void Start()
        {
            _objectSelection = GetComponent<ObjectSelection>();
            _boxSelectionScript = GetComponent<BoxSelectionScript>();

            InputManager.Instance.OnMouseOneClick.AddListener(HandleLeftClick);
            InputManager.Instance.OnMouseOneHeld.AddListener(HandleLeftHeld);
            InputManager.Instance.OnMouseTwoClick.AddListener(HandleRightClick);
            InputManager.Instance.OnControlClick.AddListener(HandleControlClick);
        }

        private void HandleLeftClick()
        {
            if (Camera.main == null) return;

            //Create a screen to point ray given the mouse position
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.mousePosition);
            bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
            RaycastHit hit;

            //Did we hit a collider?
            if (Physics.Raycast(ray, out hit) && !isOverUI)
            {
                GameObject hitObject = hit.transform.gameObject;

                switch (hitObject.gameObject.tag)
                {
                    case "Floor":
                        //If we have no units then deselect any objects
                        if (UnitManager.Instance.selectedUnits.Count == 0)
                        {
                            _objectSelection.Deselect();
                            return;
                        }

                        //Move units to this point.
                        foreach (var unit in UnitManager.Instance.selectedUnits)
                        {
                            UnitMovement script = unit.GetComponent<UnitMovement>();
                            script.ClearQueue();
                            script.MoveToPoint(hit.point);
                        }

                        //To do, take this outside and into a unit controller 
                        break;
                    
                    case "Enemy":
                        print("Hit an enemy");
                        if (!UnitManager.Instance.selectedUnits.Any()) return;
                        //Set the enemy
                        UnitManager.Instance.AttackEnemy(hitObject);
                        break;
                    case "Node":
                        //Attempt to assign selected units to the node
                        UnitManager.Instance.AssignUnitsToNode(hitObject);
                        _objectSelection.SelectObject(hitObject);
                        break;
                    case "Storage":
                        _objectSelection.SelectObject(hitObject);
                        break;
                    case "Processor":
                        _objectSelection.SelectObject(hitObject);
                        break;
                    case "Unit":
                        _objectSelection.SelectObject(hitObject);
                        break;
                    default:
                        print("This wasn't assigned!");
                        break;
                }
            }
        }

        private void HandleLeftHeld()
        {
            if (!_boxSelectionScript.enabled) _boxSelectionScript.enabled = true;
        }

        private void HandleRightClick()
        {
            UnitManager.Instance.DeselectAll();
            _objectSelection.Deselect();
        }

        private void HandleControlClick()
        {
            //This is going to be ugly

            if (Camera.main == null) return;

            //Create a screen to point ray given the mouse position
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.mousePosition);
            bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
            RaycastHit hit;

            //Did we hit a collider?
            if (Physics.Raycast(ray, out hit) && !isOverUI)
            {
                GameObject hitObject = hit.transform.gameObject;

                switch (hitObject.gameObject.tag)
                {
                    case "Floor":
                        //If we have no units then do nothing
                        if (UnitManager.Instance.selectedUnits.Count == 0) return;

                        //Queue units to this point.
                        foreach (var unit in UnitManager.Instance.selectedUnits)
                        {
                            unit.GetComponent<UnitMovement>().QueueUpPoint(hit.point);
                        }

                        //To do, take this outside and into a unit controller 
                        break;

                }
            }

        }
    }
}
