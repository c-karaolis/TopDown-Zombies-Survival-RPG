using UnityEngine;
using Foxlair.PlayerInput;

namespace Foxlair.Movement
{
    [RequireComponent(typeof(InputHandler), typeof(CharacterController))]
    public class CharacterMovement : MonoBehaviour
    {
        private InputHandler _input;
        private CharacterController _characterController;

        [SerializeField]
        private float movementSpeed = 5f;

        [SerializeField]
        private float rotateSpeed = 3f;

        [SerializeField]
        private float rotationSmoothTime=0.1f;

        [SerializeField]
        private float rotationSmoothVelocity;

        [SerializeField]
        private Camera _camera;

        private Vector3 direction;
        private void Awake()
        {
            _input = GetComponent<InputHandler>();
            _characterController = GetComponent<CharacterController>();
        }


        // Update is called once per frame
        void Update()
        {
            direction = new Vector3(_input.inputVector.x, 0, _input.inputVector.y).normalized;
            HandleMovement();
        }

        private void HandleMovement()
        {

            if (direction.magnitude >= 0.1f)
            {
                RotateTowardsMovementDirection();

                _characterController.Move(direction * movementSpeed * Time.deltaTime);
            }
        }

        private void RotateTowardsMovementDirection()
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSmoothVelocity, rotationSmoothTime);

            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        }
    }
}