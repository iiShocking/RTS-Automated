using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    public class UnitManager : MonoBehaviour
    {
        
        public static UnitManager Instance { get; private set; }
        
        [SerializeField] public List<GameObject> selectedUnits;
        
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
        
        public void SelectUnit(GameObject unit)
        {
            if (selectedUnits.Contains(unit)) return;
            selectedUnits.Add(unit);
        }

        public void DeselectUnit(GameObject unit)
        {
            if (!selectedUnits.Contains(unit)) return;
            selectedUnits.Remove(unit);
        }

        public void DeselectAll()
        {
            var unitsToDeselect = new List<GameObject>(selectedUnits);
    
            foreach (var unit in unitsToDeselect)
            {
                unit.GetComponent<UnitSelectScript>().OnDeselect();
            }
        }

        public void AssignUnitsToNode(GameObject node)
        {
            print("Assigning units to node");
            int maxUnits = node.GetComponent<ResourceNode>().maxUnitsAllowed;
            List<GameObject> unitsToGo = new List<GameObject>();
            
            foreach (var unit in selectedUnits)
            {
                if (maxUnits == 0) return;
                unitsToGo.Add(unit);
                maxUnits--;
            }
            
            print(unitsToGo.Count);
            
            foreach (var unit in unitsToGo)
            {
                print(unit);
                node.GetComponent<ResourceNode>().AssignToAnchorPoint(unit);
            } ;
        }

        public void AttackEnemy(GameObject enemy)
        {
            print("Attack!!");
            foreach (var unit in selectedUnits)
            {
                UnitGoalScript script = unit.GetComponent<UnitGoalScript>();
                script.AttackEnemy(enemy);
            }
        }

        public int IndexOfUnit(GameObject unit)
        {
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                if (selectedUnits[i] == unit)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
