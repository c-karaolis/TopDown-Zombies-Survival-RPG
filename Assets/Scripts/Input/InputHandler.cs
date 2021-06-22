using UnityEngine;
using Foxlair.Tools;

namespace Foxlair.PlayerInput
{
    public class InputHandler : PersistentSingletonMonoBehaviour<InputHandler>
    {
        public Vector2 InputVector { get; private set; }
        public Vector3 MousePosition { get; private set; }

        public Joystick Joystick;

        public bool IsFiringButtonDown;
        public bool IsInteractionButtonDown;

        public bool IsMovementButtonsDown;

        private void Start()
        {
            Joystick = FindObjectOfType<Joystick>();
        }

        void Update()
        {
            HandleMovementInput();
            HandleGeneralCharacterInput();

            MousePosition = Input.mousePosition;
        }

        private void HandleGeneralCharacterInput()
        {

        }

        private void HandleMovementInput()
        {

            // in the future maybe allow half left of the screen to act as joy stick when you touch anywhere
            //var h = Input.GetAxisRaw("Horizontal");
            //var v = Input.GetAxisRaw("Vertical");


            // InputVector = new Vector2(h, v);
            InputVector = new Vector2(Joystick.Horizontal, Joystick.Vertical);
          
            IsMovementButtonsDown = (InputVector != Vector2.zero);
            //Debug.Log(inputVector);
        }
    }
}
