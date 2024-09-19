using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    public class ZoomHandler : MonoBehaviour
    {
        [SerializeField] private float zoomSpeed;
        [SerializeField] private float inBounds;
        [SerializeField] private float outBounds;
        public void HandleZoomInput(float zoomInput)
        {
            if(zoomInput > 0)
            {
                ZoomIn();
            }
            if(zoomInput < 0)
            {
                ZoomOut();
            }
        }
        
        private void ZoomIn()
        {
            //Check we're not below the bounds... somehow!
            if (transform.position.y <= inBounds) return;
            //If we're within the bounds then move, and enforce bounds
            transform.position += transform.forward * zoomSpeed * Time.deltaTime;
            LimitBounds();
        }

        private void ZoomOut()
        {
            //Check we're not above the bounds... somehow!
            if (transform.position.y >= outBounds) return;
            //If we're within the bounds then move, and enforce bounds
            transform.position -= transform.forward * zoomSpeed * Time.deltaTime;
            LimitBounds();
        }

        private void LimitBounds()
        {
            if (transform.position.y < inBounds)
            {
                transform.position = new Vector3(transform.position.x, inBounds, transform.position.z);
            }

            if (transform.position.y > outBounds)
            {
                transform.position = new Vector3(transform.position.x, outBounds, transform.position.z);
            }
        }
    }
}
