using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FactoryEvolved
{
    public class BuildManager : MonoBehaviour
    {
        private bool isBuilding;
        
        private GameObject _placement;
        
        public GameObject _storagePrefab;
        public GameObject _housingPrefab;

        public Material validMat;
        public Material invalidMat;

        private Camera main;
        private Ray ray;
        private RaycastHit hit;

        public Vector3 hitPosition;

        private int _obstacles;

        public LayerMask layerMask;

        private UnitHousing _unitHousing;

        [SerializeField] private GameObject storageBTN;
        [SerializeField] private GameObject housingBTN;

        [SerializeField] private string buildChoice;
        // Start is called before the first frame update
        void Start()
        {
            _unitHousing = GetComponent<UnitHousing>();
            main = Camera.main;
            _placement = Instantiate(PrefabManager.Instance.GetModel("Construction"));
            _placement.SetActive(false);
            buildChoice = "Housing";
        }

        public void ToggleOtherButtons()
        {
            storageBTN.SetActive(!storageBTN.activeSelf);
            housingBTN.SetActive(!housingBTN.activeSelf);
        }

        public void ChangeBuild(string model)
        {
            switch (model)
            {
                case "Storage":
                    _placement.GetComponent<MeshFilter>().mesh = _storagePrefab.GetComponentInChildren<MeshFilter>().sharedMesh;
                    buildChoice = "Storage";
                    break;
                case "Housing":
                    _placement.GetComponent<MeshFilter>().mesh = _housingPrefab.GetComponent<MeshFilter>().sharedMesh;
                    buildChoice = "Housing";
                    break;
            }
        }

        public void ToggleBuildMode()
        { 
            isBuilding = !isBuilding;

            if (isBuilding == false)
            {
                _placement.SetActive(false);
                _placement.GetComponent<ValidityCheck>().obstacles = 0;
            }
        }
        
        // Update is called once per frame
        void Update()
        {
            //If we aren't building then quit
            if (!isBuilding) return;

            _placement.SetActive(true);
            //Get mouse to screen position
            ray = main.ScreenPointToRay(InputManager.Instance.mousePosition);

            if (Physics.Raycast(ray, out hit, 100f, layerMask) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (hit.collider.gameObject.CompareTag("Floor"))
                {
                    hitPosition = hit.point;
                }
            }

            _placement.transform.position = hitPosition;
            //Place prefab at location of mouse
            //Check for valid placement
            if (CheckForValidPlacement())
            {
                ChangeMaterial("Valid");
                //On click, place object
                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    PlaceBuilding();
                }
            }
            else
            {
                ChangeMaterial("Invalid");
            }
        }

        private void PlaceBuilding()
        {
            if (buildChoice == "Housing")
            {
                if (!_unitHousing.CheckBuild()) return;
                var newBuilding = Instantiate(_housingPrefab, hitPosition, Quaternion.identity);
                newBuilding.transform.rotation = Quaternion.Euler(0, -180, 0);
                _unitHousing.BuildPlaced();
            }
            else
            {
                var newBuilding = Instantiate(_storagePrefab, hitPosition, Quaternion.identity);
                newBuilding.transform.rotation = Quaternion.Euler(0, -180, 0);
            }
        }

        private bool CheckForValidPlacement()
        {
            if (_placement.GetComponent<ValidityCheck>().obstacles == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ChangeMaterial(string state)
        {
            switch (state)
            {
                case "Valid":
                    try
                    {
                        _placement.GetComponent<MeshRenderer>().material = new Material(validMat);
                        break;
                    }
                    catch (MissingComponentException e)
                    {
                        _placement.GetComponentInChildren<MeshRenderer>().material = new Material(validMat);
                        break;
                    }
                case "Invalid":
                    try
                    {
                        _placement.GetComponent<MeshRenderer>().material = new Material(invalidMat);
                        break;
                    }
                    catch (MissingComponentException e)
                    {
                        _placement.GetComponentInChildren<MeshRenderer>().material = new Material(invalidMat);
                        break;
                    }
            }
        }
    }
}
