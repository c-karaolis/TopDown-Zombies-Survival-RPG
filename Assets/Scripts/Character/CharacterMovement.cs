using UnityEngine;
using Foxlair.PlayerInput;
using Foxlair.Character.Targeting;

namespace Foxlair.Character.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMovement : MonoBehaviour
    {
        public PlayerCharacter PlayerCharacter;

        private InputHandler input;
        [SerializeField]
        private CharacterController characterController;
        private CharacterTargetingHandler characterTargetingHandler;
        private double gravity;

        [SerializeField]
        private float MovementSpeed = 5f;

        [SerializeField]
        private float RotationSmoothTime=0.01f;

        [SerializeField]
        private float RotationSmoothVelocity;

        [SerializeField]
        private Camera _camera;

        public Vector3 Direction;

        private void Start()
        {
            characterTargetingHandler = PlayerCharacter.CharacterTargetingHandler;
            input = InputHandler.Instance;

        }


        public void UpdateCharacterMovement()
        {
            HandleGravity();
            Direction = new Vector3(input.InputVector.x, (float)gravity, input.InputVector.y).normalized;
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
            if (characterController.isGrounded)
            {
                gravity = 0;
            }
        }

        public void HandleHarvestingAutoRotation()
        {
            if (characterTargetingHandler.HarvestResourceTarget != null && input.IsInteractionButtonDown)
            {
                Vector3 targetDirection = characterTargetingHandler.HarvestResourceTarget.transform.position - transform.position;
                RotateTowards(targetDirection);
            }

        }

        public void HandleAutoTargetingRotation()
        {
            if(PlayerCharacter.PlayerTargetEnemy != null)
            {
                Vector3 targetDirection = PlayerCharacter.PlayerTargetEnemy.transform.position - transform.position;
                RotateTowards(targetDirection);
            }

        }

        public void HandleAutoMoveToAttack(Transform targetToAttack) 
        {
            Vector3 targetDirection = targetToAttack.position - transform.position;
            RotateTowards(targetDirection);
            MoveTowards(targetDirection);
        }

        public void HandleAutoMoveToHarvest(Transform resourceToHarvest) 
        {
            Vector3 targetDirection = resourceToHarvest.position - transform.position;
            RotateTowards(targetDirection);
            MoveTowards(targetDirection);

        }

        private void HandleMovement()
        {

            if (Direction.magnitude >= 0.1f)
            {
                RotateTowardsMovementDirection();
                MoveTowards(Direction);
            }
        }

        private void MoveTowards(Vector3 movementDirection)
        {
            characterController.Move(movementDirection.normalized * MovementSpeed * Time.deltaTime);
        }

        private void RotateTowardsMovementDirection()
        {
            RotateTowards(Direction);
        }

        private void RotateTowards(Vector3 rotationDirection)
        {
            float targetAngle = Mathf.Atan2(rotationDirection.x, rotationDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref RotationSmoothVelocity, RotationSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

    }
}