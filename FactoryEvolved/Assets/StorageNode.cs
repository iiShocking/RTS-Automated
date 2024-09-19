using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
namespace FactoryEvolved
{
    public class StorageNode : MonoBehaviour, ISelectable
    {
        private object _item;
        [SerializeField] private Items.Tier1 Determine;
        [SerializeField] private Items.Tier1Craftable Predetermine;
        
        [SerializeField] public Resource storedItem;
        [SerializeField] private int maxAmount;
    
        //UI
        [SerializeField] private GameObject ui;
        [SerializeField] private TMP_Text resourceQuantity;
        [SerializeField] private TMP_Text resourceName;

        private void Start()
        {
            InitUI();
        }
        
        [ContextMenu("Set1st")]
        private void SetDetermine() => storedItem = new Resource(Determine.ToString());

        [ContextMenu("Set2nd")]
        private void SetPredetermine() => storedItem = new Resource(Predetermine.ToString());

        public void TransferInto(Resource transfer)
        {
            print("Transferring Into Storage");
            if (storedItem.Name == "")
            {
                storedItem = new Resource(transfer.Name);
                UpdateUI();
            }
            
            if (storedItem.Name == transfer.Name)
            {
                HandleTransfer(transfer, GetMaxInput(transfer.Amount));
                UpdateQuantity();
            }
        }

        public void Consume(int amount)
        {
            print("Consuming item out of storage");
            storedItem.Amount -= amount;
            UpdateQuantity();
        }

        private int GetMaxInput(int amountToCheck)
        {
            int remaining = maxAmount - storedItem.Amount;
            int amountToTake = Math.Min(remaining, amountToCheck);

            return amountToTake;
        }

        private void HandleTransfer(Resource incoming, int amountToTake)
        {
            incoming.Amount -= amountToTake;
            storedItem.Amount += amountToTake;
        }

        private void InitUI()
        {
            resourceQuantity.text = "";
            resourceName.text = "Nothing";
            ui.SetActive(false);
        }

        private void UpdateUI()
        {
            resourceQuantity.text = storedItem.Amount.ToString();
            resourceName.text = storedItem.Name;
        }

        public Resource GetResource() => storedItem;
        
        private void UpdateQuantity() => resourceQuantity.text = storedItem.Amount.ToString();
        
        public void OnSelect()
        {
            DisplayUI();
        }

        public void OnDeselect()
        {
            HideUI();
        }

        public void OnHighlight()
        {
            throw new NotImplementedException();
        }

        public void OnUnhighlight()
        {
            throw new NotImplementedException();
        }

        public void DisplayUI()
        {
            ui.SetActive(true);
        }

        public void HideUI()
        {
            ui.SetActive(false);
        }
    }
}
