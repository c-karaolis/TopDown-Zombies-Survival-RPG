using UnityEngine;
using Foxlair.PlayerInput;
using Foxlair.Character;
using System;
using Foxlair.Character.Targeting;

namespace Foxlair.Character.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMovement : MonoBehaviour
    {
        private InputHandler _input;
        private CharacterController _characterController;
        private CharacterTargetingHandler _characterTargetingHandler;
        private bool enemyTargetFound;
        private bool resourceNodeTargetFound;

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
            _input = InputHandler.Instance;
            _characterTargetingHandler = CharacterTargetingHandler.Instance;
            _characterController = GetComponent<CharacterController>();
        }


        // Update is called once per frame
        void Update()
        {
            direction = new Vector3(_input.inputVector.x, 0, _input.inputVector.y).normalized;
            HandleAutoTargetingRotation();
            HandleHarvestingAutoRotation();
            HandleMovement();
           
        }

        private void HandleHarvestingAutoRotation()
        {
            if (_characterTargetingHandler.HarvestResourceTarget != null)
            {
                //TODO: figure out how harvest resource will be handled(e.g. rotate to harvest node only if player presses harvest)
                // || _characterTargetingHandler.HarvestResourceTarget != null
                resourceNodeTargetFound = true;
                Vector3 targetDirection = _characterTargetingHandler.HarvestResourceTarget.transform.position - transform.position;
                RotateTowards(targetDirection);
            }
            else
            {
                resourceNodeTargetFound = false;
            }
        }

        private void HandleAutoTargetingRotation()
        {
            if(_characterTargetingHandler.EnemyTarget != null)
            {
                //TODO: figure out how harvest resource will be handled(e.g. rotate to harvest node only if player presses harvest)
                // || _characterTargetingHandler.HarvestResourceTarget != null
                enemyTargetFound = true;
                Vector3 targetDirection = _characterTargetingHandler.EnemyTarget.transform.position - transform.position;
                RotateTowards(targetDirection);
            }
            else
            {
                enemyTargetFound = false;
            }

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
            if (enemyTargetFound) {return;}
            RotateTowards(direction);
        }

        private void RotateTowards(Vector3 rotationDirection)
        {
            float targetAngle = Mathf.Atan2(rotationDirection.x, rotationDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSmoothVelocity, rotationSmoothTime);

            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        }

    }
}