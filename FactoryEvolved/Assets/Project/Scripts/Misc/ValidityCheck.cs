using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    public class ValidityCheck : MonoBehaviour
    {
        public int obstacles;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Floor")) return;

            obstacles++;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Floor")) return;
            obstacles--;
        }
    }
}
