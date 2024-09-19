using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FactoryEvolved
{
    public class AIAnchorScript : MonoBehaviour
    {
        [SerializeField] private List<GameObject> availableAnchorPositions;

        private void Start()
        {
            availableAnchorPositions = new List<GameObject>();

            foreach (var anchor in gameObject.GetComponentsInChildren<Anchor>())
            {
                availableAnchorPositions.Add(anchor.gameObject);
            }
        }

        public GameObject GetRandomAnchorPosition()
        {
            int randomNumber = Random.Range(0, availableAnchorPositions.Count);

            return availableAnchorPositions[randomNumber];
        }
    }
}
