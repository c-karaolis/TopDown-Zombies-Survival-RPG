using UnityEngine;
using Foxlair.Tools;

namespace Foxlair.PlayerInput
{
    public class InputHandler : SingletonMonoBehaviour<InputHandler>
    {
        public Vector2 inputVector { get; private set; }
        public Vector3 mousePosition { get; private set; }

        public bool isFiringButtonDown;
        public bool isInteractionButtonDown;

        public bool isMovementButtonsDown;

        void Update()
        {
            HandleMovementInput();
            HandleWeaponInput();
            HandleGeneralCharacterInput();

            mousePosition = Input.mousePosition;
        }

        private void HandleGeneralCharacterInput()
        {
            //Harvesting stuff here??
            isInteractionButtonDown = Input.GetButton("Jump");
        }

        private void HandleWeaponInput()
        {
            isFiringButtonDown = Input.GetButton("Fire1");
        }

        private void HandleMovementInput()
        {
             
            // in the future maybe allow half left of the screen to act as joy stick when you touch anywhere
            var h = Input.GetAxisRaw("Horizontal");
            var v = Input.GetAxisRaw("Vertical");


            inputVector = new Vector2(h, v);
            isMovementButtonsDown = (inputVector != Vector2.zero);
            //Debug.Log(inputVector);
        }
    }
}
