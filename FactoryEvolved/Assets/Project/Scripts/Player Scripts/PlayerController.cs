using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    public class PlayerController : MonoBehaviour
    {
        private ZoomHandler _zoomHandler;
        private MovementHandler _movementHandler;

        private void Awake()
        {
            _zoomHandler = GetComponent<ZoomHandler>();
            _movementHandler = GetComponent<MovementHandler>();
            
            //Scroll Inputs
            InputManager.Instance.OnScrollUp.AddListener(HandleScrollInput);
            InputManager.Instance.OnScrollDown.AddListener(HandleScrollInput);

            //Movement Inputs
            InputManager.Instance.OnMovementKeyPressed.AddListener(HandleMovementInput);
        }

        private void HandleMovementInput(string direction)
        {
            _movementHandler.HandleMovementInput(direction);
        }
        private void HandleScrollInput()
        {
            _zoomHandler.HandleZoomInput(Input.mouseScrollDelta.y);
        }
    }
}
