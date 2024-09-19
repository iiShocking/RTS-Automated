using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FactoryEvolved
{
    public class ProcessingNode : Node, ISelectable
    {
        [SerializeField] private Items.Tier1 input;
        [SerializeField] private Items.Tier1Craftable output;

        [SerializeField] public Resource resourceInput;
        [SerializeField] public Resource resourceOutput;

        [SerializeField] private int maxInput;
        [SerializeField] private int maxOutput;
        
        //UI
        [SerializeField] private GameObject ui;
        [SerializeField] private TMP_Text inputUI;
        [SerializeField] private TMP_Text outputUI;

        //Use start when manually placed
        void Start()
        {
            resourceInput = new Resource(input.ToString());
            resourceOutput = new Resource(output.ToString());
            
            TickManager.Instance.Subscribe(Process, 8);
            InitUI();
        }
        

        public override void Process()
        {
            if (resourceInput.Amount == 0) return;
            if (resourceOutput.Amount >= maxInput) return;
            
            resourceInput.Amount -= 1;
            resourceOutput.Amount += 1;
            
            UpdateUI();
        }
        
        public void TransferInto(Resource transfer)
        {
            print("Transferring Into Storage");
            if (resourceInput.Name == transfer.Name)
            {
                HandleTransfer(transfer, GetMaxInput(transfer.Amount));
                UpdateUI();
            }
        }

        private int GetMaxInput(int amountToCheck)
        {
            int remaining = maxInput - resourceInput.Amount;
            int amountToTake = Mathf.Min(remaining, amountToCheck);

            return amountToTake;
        }

        private void HandleTransfer(Resource incoming, int amountToTake)
        {
            incoming.Amount -= amountToTake;
            resourceInput.Amount += amountToTake;
        }

        private void InitUI()
        {
            ui.SetActive(false);
            inputUI.text = resourceInput.Name + ": " + resourceInput.Amount;
            outputUI.text = resourceOutput.Name + ": " + resourceOutput.Amount;
        }
        private void UpdateUI()
        {
            inputUI.text = resourceInput.Name + ": " + resourceInput.Amount;
            outputUI.text = resourceOutput.Name + ": " + resourceOutput.Amount;
        }

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
            throw new System.NotImplementedException();
        }

        public void OnUnhighlight()
        {
            throw new System.NotImplementedException();
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
