using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    public class UnitResourceScript : MonoBehaviour
    {
        [SerializeField] public Resource heldResource;
        [SerializeField] private int maxCarryLimit = 50;
        
        public void SetResource(string n) => heldResource = new Resource(n);

        public void IncreaseAmount(Resource resource)
        {
            if (heldResource.Amount >= maxCarryLimit) return;
            
            int remaining = maxCarryLimit - heldResource.Amount;
            int amountToTake = Math.Min(remaining, resource.Amount);
            
            heldResource.Amount += amountToTake;
            resource.Amount -= amountToTake;
        }

        public void TryResetHeldResource()
        {
            if (heldResource.Amount == 0)
            {
                heldResource.Name = "";
            }
        }
        //To Do, Transfer Method

    }
}
