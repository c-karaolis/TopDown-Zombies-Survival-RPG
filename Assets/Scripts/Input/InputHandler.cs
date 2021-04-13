using UnityEngine;
using Foxlair.Tools;

namespace Foxlair.PlayerInput
{
    public class InputHandler : SingletonMonoBehaviour<InputHandler>
    {
        public Vector2 InputVector { get; private set; }
        public Vector3 MousePosition { get; private set; }

        public bool IsFiringButtonDown;
        public bool IsInteractionButtonDown;

        public bool IsMovementButtonsDown;

        void Update()
        {
            HandleMovementInput();
            HandleWeaponInput();
            HandleGeneralCharacterInput();

            MousePosition = Input.mousePosition;
        }

        private void HandleGeneralCharacterInput()
        {
            //Harvesting stuff here??
            IsInteractionButtonDown = Input.GetButton("Jump");
        }

        private void HandleWeaponInput()
        {
            IsFiringButtonDown = Input.GetButton("Fire1");
        }

        private void HandleMovementInput()
        {
             
            // in the future maybe allow half left of the screen to act as joy stick when you touch anywhere
            var h = Input.GetAxisRaw("Horizontal");
            var v = Input.GetAxisRaw("Vertical");


            InputVector = new Vector2(h, v);
            IsMovementButtonsDown = (InputVector != Vector2.zero);
            //Debug.Log(inputVector);
        }
    }
}
