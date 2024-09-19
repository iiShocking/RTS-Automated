using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    public abstract class Node : MonoBehaviour
    {
        public string NodeName { get; set; }
        public Vector3 NodePosition { get; set; }

        public abstract void Process();

    }
}
