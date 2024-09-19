using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace FactoryEvolved
{
    public class PrefabManager : MonoBehaviour
    {
        public static PrefabManager Instance { get; private set; }

        [SerializeField] public GameObject defaultModel;
        
        [SerializeField] private GameObject woodNodeModel1;
        [SerializeField] private GameObject woodNodeModel2;
        [SerializeField] private GameObject woodNodeModel3;
        [SerializeField] private GameObject woodNodeModel4;
        [SerializeField] private GameObject woodNodeModel5;
        
        [SerializeField] private GameObject waterNodeModel;
        [SerializeField] private GameObject stoneNodeModel;

        [SerializeField] private GameObject constructionModel;

        [SerializeField] private GameObject bush1;
        [SerializeField] private GameObject bush2;
        [SerializeField] private GameObject bush3;
        
        [SerializeField] private List<GameObject> allModels;

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

        private void Start()
        {
            allModels = new List<GameObject>();
            
            allModels.Add(defaultModel);
            
            allModels.Add(woodNodeModel1);
            allModels.Add(woodNodeModel2);
            allModels.Add(woodNodeModel3);
            allModels.Add(woodNodeModel4);
            allModels.Add(woodNodeModel5);
            
            allModels.Add(waterNodeModel);
            allModels.Add(stoneNodeModel);
            
            allModels.Add(constructionModel);
            
            allModels.Add(bush1);
            allModels.Add(bush2);
            allModels.Add(bush3);
        }

        public GameObject GetModel(string modelName)
        {
            print(modelName);
            if (modelName.StartsWith("Wood"))
            {
                print("Choosing Tree Model");
                var rand = new Random();
                int numb = rand.Next(0, 4);

                switch (numb)
                {
                    case 0:
                        return woodNodeModel2;
                    case 1:
                        return woodNodeModel3;
                    case 2:
                        return woodNodeModel4;
                    case 3:
                        return woodNodeModel5;
                    case 4:
                        return woodNodeModel1;
                    default:
                        return defaultModel;
                }
            }
            if (modelName.StartsWith("Berries"))
            {
                print("Choosing Bush Model");
                var rand = new Random();
                int numb = rand.Next(0, 2);

                switch (numb)
                {
                    case 0:
                        return bush1;
                    case 1:
                        return bush2;
                    case 2:
                        return bush3;
                    default:
                        return defaultModel;
                }
            }
            
            foreach (var model in allModels)
            {
                if (model.gameObject.name == modelName + "Model")
                {
                    return model;
                }
            }

            Debug.LogError("NO MODEL FOUND FOR REQUESTED: " + modelName);
            return null;
        }
    }
}
