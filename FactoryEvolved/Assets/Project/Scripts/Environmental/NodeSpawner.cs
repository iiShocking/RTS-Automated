using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    public class NodeSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject nodePrefab;
        [SerializeField] private GameObject deer;
        [SerializeField] private LayerMask mask;
        
        private const int MinX = -24;
        private const int MinZ = -16;
        private const int MaxX = 100;
        private const int MaxZ = 100;

        private void Start()
        {
            var pos = ChooseRandomPositionToSpawn();
            
            var status = CheckIfPositionIsObstructed(pos);
            
            print("Spawning Trees");
            SpawnNode(200, Items.Tier1.Wood);
            print("Spawning Stone");
            SpawnNode(80, Items.Tier1.Stone);
            print("Spawning Berries");
            SpawnNode(230, Items.Tier1.Berries);
            print("Spawning Water");
            SpawnNode(55, Items.Tier1.Water);
            print("Spawning Deer");
            SpawnObject(70);
        }

        private void SpawnNode(int loops, Items.Tier1 resource)
        {
            for (int i = 0; i < loops; i++)
            {
                bool status = false;
                Vector3 position = new Vector3(0, 0, 0);
                while (!status)
                { 
                    position = ChooseRandomPositionToSpawn();
                    status = CheckIfPositionIsObstructed(position);
                }

                var node = Instantiate(nodePrefab, position, Quaternion.identity);
                node.GetComponent<ResourceNode>().Init(resource.ToString());
            }
        }

        private void SpawnObject(int loops)
        {
            for (int i = 0; i < loops; i++)
            {
                var position = ChooseRandomPositionToSpawn();
                Instantiate(deer, position, Quaternion.identity);
            }
        }

        private bool CheckIfPositionIsObstructed(Vector3 position)
        {
            var nearby = Physics.OverlapSphere(position, 2, mask);
            
            if (nearby.Length != 0) return false;

            return true;
        }
        private Vector3 ChooseRandomPositionToSpawn()
        {
            var random = new System.Random();
            return new Vector3(random.Next(MinX, MaxX), 0, random.Next(MinZ, MaxZ));
        }
    }
}
