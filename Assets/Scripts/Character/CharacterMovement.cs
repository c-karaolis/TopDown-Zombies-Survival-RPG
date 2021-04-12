using UnityEngine;
using Foxlair.PlayerInput;
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
        private double gravity;

        [SerializeField]
        private float movementSpeed = 5f;

        [SerializeField]
        private float rotationSmoothTime=0.01f;

        [SerializeField]
        private float rotationSmoothVelocity;

        [SerializeField]
        private Camera _camera;

        public Vector3 direction;
        private void Awake()
        {
        }
        private void Start()
        {
            _characterTargetingHandler = CharacterTargetingHandler.Instance;
            _characterController = GetComponent<CharacterController>();

            _input = InputHandler.Instance;

        }


        public void UpdateCharacterMovement()
        {
            HandleGravity();
            direction = new Vector3(_input.inputVector.x, (float)gravity, _input.inputVector.y).normalized;
            //HandleAutoTargetingRotation();
            HandleMovement();
        }

        // Update is called once per frame
        //void Update()
        //{
        //    HandleGravity();
        //    direction = new Vector3(_input.inputVector.x, (float)gravity, _input.inputVector.y).normalized;
        //    HandleAutoTargetingRotation();
        //    HandleHarvestingAutoRotation();
        //    HandleMovement();
        //}

        private void HandleGravity()
        {
            gravity -= 9.81 * Time.deltaTime;
            if (_characterController.isGrounded)
            {
                gravity = 0;
            }
        }

        public void HandleHarvestingAutoRotation()
        {
            if (_characterTargetingHandler.HarvestResourceTarget != null && _input.isInteractionButtonDown)
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

        public void HandleAutoTargetingRotation()
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

        private void HandleAutoMoveToAttack(Transform targetToAttack) 
        {
            Vector3 targetDirection = targetToAttack.position - transform.position;
            RotateTowards(targetDirection);
            MoveTowards(targetDirection);
        }

        private void HandleAutoMoveToHarvest(Transform resourceToHarvest) 
        {
            Vector3 targetDirection = resourceToHarvest.position - transform.position;
            RotateTowards(targetDirection);
            MoveTowards(targetDirection);

        }

        private void HandleMovement()
        {

            if (direction.magnitude >= 0.1f)
            {
                RotateTowardsMovementDirection();
                MoveTowards(direction);
            }
        }

        private void MoveTowards(Vector3 movementDirection)
        {
            _characterController.Move(movementDirection * movementSpeed * Time.deltaTime);
        }

        private void RotateTowardsMovementDirection()
        {
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