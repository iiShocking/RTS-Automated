using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FactoryEvolved
{
    //This needs to be refactored badly
    public class UnitHousing : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        public int housesPlaced;

        [SerializeField] private List<Resource> nextHouseRecipe;
        private void Start()
        {
            nextHouseRecipe = new List<Resource>()
            {
                new Resource("Wood"),
                new Resource("Stone"),
                new Resource("Plank")
            };

            nextHouseRecipe[0].Amount = 10;
            nextHouseRecipe[1].Amount = 10;
            nextHouseRecipe[2].Amount = 3;
        }
    
        [ContextMenu("Check")]
        public bool CheckBuild()
        {
            var nearbyResources = GetNearbyResources();

            bool condition = CanCompleteWithResources(nearbyResources);
            
            return condition;
        }

        public void BuildPlaced()
        {
            housesPlaced++;
            ResetRecipe();
            UpgradeRecipeRequirements();
        }
        private void UpgradeRecipeRequirements()
        {
            nextHouseRecipe[0].Amount += housesPlaced * 5;
            nextHouseRecipe[1].Amount += housesPlaced * 5;
            nextHouseRecipe[2].Amount += housesPlaced * 2;
        }

        private void ResetRecipe()
        {
            nextHouseRecipe[0].Amount = 20;
            nextHouseRecipe[1].Amount = 20;
            nextHouseRecipe[2].Amount = 5;
        }
        private List<Resource> GetNearbyResources()
        {
            //Find storages within a range
            var nearbyObjects = Physics.OverlapSphere(player.transform.position, 100);
            List<Resource> nearbyResources = new List<Resource>();
            foreach (var obj in nearbyObjects)
            {
                if (obj.CompareTag("Storage"))
                {
                    print("Found storage at: " + obj.transform.position);
                    nearbyResources.Add(obj.GetComponent<StorageNode>().GetResource());
                }
            }
            //Gather a list of available resources
            return nearbyResources;
        }

        private bool CanCompleteWithResources(List<Resource> resources)
        {
            foreach (var resource in nextHouseRecipe)
            {
                foreach (var held in resources)
                {
                    if (held.Name == resource.Name)
                    {
                        if (!IsEnough(held.Amount, resource.Amount))
                        {
                            return false;
                        }
                        else
                        {
                            held.Amount -= resource.Amount;
                            resource.Amount -= resource.Amount;
                        }
                    }
                }
            }

            foreach (var r in nextHouseRecipe)
            {
                if (r.Amount != 0)
                {
                    ResetRecipe();
                    return false;
                }
            }
            return true;
        }

        private bool IsEnough(int amountGiven, int amountCompared)
        {
            return amountGiven >= amountCompared;
        }
    }
}