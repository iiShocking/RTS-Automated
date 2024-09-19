using UnityEngine;
using UnityEngine.Events;

namespace FactoryEvolved
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        [SerializeField] public Vector3 mousePosition;

        [SerializeField] public float holdDelay;

        [SerializeField] public bool leftClickIsBeingHeld;

        [SerializeField] private float clickTime;
        

        //List all input events
        public UnityEvent OnMouseOneClick;
        public UnityEvent OnMouseOneHeld;
        public UnityEvent OnMouseTwoClick;

        public UnityEvent OnControlClick;

        public UnityEvent<string> OnMovementKeyPressed = new UnityEvent<string>();
        public UnityEvent OnRightKeyDown;
        public UnityEvent OnLeftKeyDown;
        public UnityEvent OnUpKeyDown;
        public UnityEvent OnDownKeyDown;
        
        public UnityEvent OnScrollDown;
        public UnityEvent OnScrollUp;

        private void Awake()
        {
            // Ensure there is only one instance of InputManager
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Update()
        {
            //Movement Keys
            //if (Input.GetKey(KeyCode.W)) OnUpKeyDown?.Invoke(); // UP KEY PRESS OR HELD, IS CALLED EVERY FRAME IT'S HELD
            //if (Input.GetKey(KeyCode.S)) OnDownKeyDown?.Invoke(); // DOWN KEY PRESS OR HELD, IS CALLED EVERY FRAME IT'S HELD
            //if (Input.GetKey(KeyCode.A)) OnLeftKeyDown?.Invoke(); // LEFT KEY PRESS OR HELD, IS CALLED EVERY FRAME IT'S HELD
            //if (Input.GetKey(KeyCode.D)) OnRightKeyDown?.Invoke(); // RIGHT KEY PRESS OR HELD, IS CALLED EVERY FRAME IT'S HELD
            HandleKeyInputs();
            
            //Scroll Input
            HandleScrollInputs();
            
            //Mouse Inputs
            HandleMouseInput();
        }

        private void HandleMouseInput()
        {
            //Update Mouse position
            mousePosition = Input.mousePosition;
            
            
            //Right Click
            if (Input.GetMouseButtonDown(1)) OnMouseTwoClick?.Invoke();
            
            // Left Click
            if (Input.GetMouseButtonDown(0))
            {
                clickTime = Time.time;
                leftClickIsBeingHeld = false; // Reset the holding flag
            }

            // Mouse button held
            if (Input.GetMouseButton(0))
            {
                if (!leftClickIsBeingHeld && (Time.time - clickTime) >= holdDelay)
                {
                    leftClickIsBeingHeld = true;
                    OnMouseOneHeld?.Invoke();
                    print("Left Hold Detected");
                }
            }

            // Mouse button up
            if (Input.GetMouseButtonUp(0))
            {
                if (!leftClickIsBeingHeld && (Time.time - clickTime) < holdDelay)
                {
                    //Handle CTRL Click Here
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        OnControlClick.Invoke();
                        print("CTRL Click Detected");
                    }
                    else
                    {
                        OnMouseOneClick?.Invoke();
                        print("Left Click Detected");
                    }
                }

                leftClickIsBeingHeld = false; // Reset the holding flag
            }
        }

        private void HandleKeyInputs()
        {
            if (Input.GetKey(KeyCode.W)) OnMovementKeyPressed?.Invoke("Up"); // UP KEY PRESS OR HELD, IS CALLED EVERY FRAME IT'S HELD
            if (Input.GetKey(KeyCode.S)) OnMovementKeyPressed?.Invoke("Down"); // DOWN KEY PRESS OR HELD, IS CALLED EVERY FRAME IT'S HELD
            if (Input.GetKey(KeyCode.A)) OnMovementKeyPressed?.Invoke("Left"); // LEFT KEY PRESS OR HELD, IS CALLED EVERY FRAME IT'S HELD
            if (Input.GetKey(KeyCode.D)) OnMovementKeyPressed?.Invoke("Right"); // RIGHT KEY PRESS OR HELD, IS CALLED EVERY FRAME IT'S HELD

        }

        private void HandleScrollInputs()
        {
            if (Input.mouseScrollDelta.y > 0) OnScrollUp?.Invoke(); // SCROLL UP, EACH CLICK IS A CALL
            if (Input.mouseScrollDelta.y < 0) OnScrollDown?.Invoke(); // SCROLL DOWN, EACH CLICK IS A CALL
        }
    }
}