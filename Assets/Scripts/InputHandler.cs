using UnityEngine;

namespace Foxlair.PlayerInput
{
    public class InputHandler : SingletonMonoBehaviour<InputHandler>
    {
        public Vector2 inputVector { get; private set; }
        public Vector3 mousePosition { get; private set; }

        // Update is called once per frame
        void Update()
        {
            HandleMovementInput();
            HandleWeaponInput();

            mousePosition = Input.mousePosition;
        }

        private void HandleWeaponInput()
        {
            throw new System.NotImplementedException();
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
