using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    public class MovementHandler : MonoBehaviour
    {
        [SerializeField] private float speed;

        public void HandleMovementInput(string direction)
        {
            switch (direction)
            {
                case "Up":
                    MoveUp();
                    break;
                case "Down":
                    MoveDown();
                    break;
                case "Left":
                    MoveLeft();
                    break;
                case "Right":
                    MoveRight();
                    break;
            }
        }
        //Don't ask me why these are inverted...
        private void MoveRight()
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        private void MoveLeft()
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        private void MoveUp()
        {
            transform.position += Vector3.forward * speed * Time.deltaTime;
        }

        private void MoveDown()
        {
            transform.position += Vector3.back * speed * Time.deltaTime;
        }
    }
}
