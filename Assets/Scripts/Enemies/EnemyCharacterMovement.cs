using Foxlair.Enemies.Targeting;
using Foxlair.PlayerInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Foxlair.Enemies.Movement
{
    public class EnemyCharacterMovement : MonoBehaviour
    {
        public EnemyCharacter enemyCharacter;

        [SerializeField]
        private EnemyCharacterTargetingHandler enemyCharacterTargetingHandler;
        private NavMeshAgent navMeshAgent;

        [SerializeField]
        private float movementSpeed = 5f;
        [SerializeField]
        private float rotationSmoothTime = 0.01f;
        [SerializeField]
        private float rotationSmoothVelocity;

        private void Awake()
        {
            navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            enemyCharacterTargetingHandler = gameObject.GetComponent<EnemyCharacterTargetingHandler>(); ;
        }
 
        public void MoveTo(Vector3 position)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(position);
        }

        public void StopMoving()
        {
            navMeshAgent.isStopped = true;
        }

        public void ContinuePreviouslyStoppedMovement()
        {
            navMeshAgent.isStopped = false;
        }

        public void HandleAutoTargetingRotation()
        {
            if (enemyCharacter.playerTarget != null)
            {
                Vector3 targetDirection = enemyCharacter.playerTarget.transform.position - transform.position;
                RotateTowards(targetDirection);
            }
        }

        private void RotateTowards(Vector3 rotationDirection)
        {
            float targetAngle = Mathf.Atan2(rotationDirection.x, rotationDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSmoothVelocity, rotationSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

    }
}