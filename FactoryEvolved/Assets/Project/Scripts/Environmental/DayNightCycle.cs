using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    public class DayNightCycle : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private float rotationSpeed;
        void Start()
        {
            TickManager.Instance.Subscribe(AdjustLighting, 1);
        }

        private void AdjustLighting()
        {
            transform.Rotate(rotationSpeed, 0, 0);
        }
    }
}
