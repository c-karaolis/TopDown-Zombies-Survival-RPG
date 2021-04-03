using UnityEngine;
using Foxlair.Tools;

namespace Foxlair.PlayerInput
{
    public class InputHandler : SingletonMonoBehaviour<InputHandler>
    {
        public Vector2 inputVector { get; private set; }
        public Vector3 mousePosition { get; private set; }

        public bool isFiringButtonDown;

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
        }

        private void HandleWeaponInput()
        {
            isFiringButtonDown = Input.GetButtonDown("Fire1");
        }

        private void HandleMovementInput()
        {
            // in the future maybe allow half left of the screen to act as joy stick when you touch anywhere
            var h = Input.GetAxisRaw("Horizontal");
            var v = Input.GetAxisRaw("Vertical");
            inputVector = new Vector2(h, v);
            //Debug.Log(inputVector);
        }
    }
}
