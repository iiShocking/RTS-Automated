using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace FactoryEvolved
{
    public class ResourceNode : Node, ISelectable
    {
        [SerializeField] private Items.Tier1 resource;
        [SerializeField] private Resource harvestedResource;
        [SerializeField] public int assignedUnits;
        [SerializeField] public int maxUnitsAllowed = 4;
        [SerializeField] private int maxAmount;
        [SerializeField] public GameObject[] anchorPositions;


        [SerializeField] private GameObject inspectorTemplate;
        
        //UI
        [SerializeField] private GameObject ui;
        [SerializeField] private TMP_Text nodeName;
        [SerializeField] private TMP_Text resourceQuantity;
        [SerializeField] private TMP_Text resourceName;
        

        
        //Init should be used when instantiated
        public void Init(string name)
        {
            harvestedResource = new Resource(name);
            TickManager.Instance.Subscribe(Process, 6);
            gameObject.name = name + "Node";
            var model = Instantiate(PrefabManager.Instance.GetModel(gameObject.name), transform.position, Quaternion.identity, transform);
            inspectorTemplate.SetActive(false);
            InitUI();
        }
        
        //Start should be used when manually placed
        // private void Start()
        // {
        //     harvestedResource = new Resource(resource.ToString());
        //     TickManager.Instance.Subscribe(Process, 6);
        //     gameObject.name = resource + "Node";
        //     try
        //     {
        //         Instantiate(PrefabManager.Instance.GetModel(gameObject.name), transform.position, Quaternion.identity, transform);
        //     }
        //     catch (ArgumentException)
        //     {
        //         Console.WriteLine("Please fix this unallocated model at " + transform.position);
        //         Instantiate(PrefabManager.Instance.defaultModel, transform.position, Quaternion.identity, transform);
        //     }
        //     inspectorTemplate.SetActive(false);
        //     InitUI();
        // }
        
        public override void Process()
        {
            harvestedResource.Amount += assignedUnits;
            ValidateVariables();
            UpdateQuantity();
        }

        public Resource GetResource() => harvestedResource;

        public void AssignToAnchorPoint(GameObject unit)
        {
            print("Assigning to anchor point");
            for(int i = 0; i < anchorPositions.Length; i++)
            {
                var anchorScript = anchorPositions[i].GetComponent<AnchorPositionScript>();
                if (!anchorScript.isInUse && !anchorScript.troopEnRoute)
                {
                    GameObject anchorPoint = anchorPositions[i];
                    unit.GetComponent<UnitMovement>().MoveToPoint(anchorPoint.transform.position);
                    anchorScript.UnitIsOnTheWay();
                    print("Assigning troop to : " + anchorPoint.name);
                    return;
                }
            }
            print("No anchor point found!");
        }

        public void IncreaseAssigned()
        {
            assignedUnits++;
            maxUnitsAllowed--;
        }

        public void DecreaseAssigned()
        {
            assignedUnits--;
            maxUnitsAllowed++;
        }
        
        private void ValidateVariables()
        {
            if (harvestedResource.Amount > maxAmount) harvestedResource.Amount = maxAmount;
        }

        private void InitUI()
        {
            ui.SetActive(false);
            nodeName.text = harvestedResource.Name + " Node";
            resourceName.text = harvestedResource.Name;
            resourceQuantity.text = harvestedResource.Amount.ToString();
        }
        
        private void UpdateQuantity() => resourceQuantity.text = harvestedResource.Amount.ToString();
    
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
