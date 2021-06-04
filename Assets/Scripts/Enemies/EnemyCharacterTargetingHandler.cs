using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foxlair.Tools;
using Foxlair.Enemies;
using Foxlair.Harvesting;
using Foxlair.Tools.Events;
using Foxlair.Character;

namespace Foxlair.Enemies.Targeting
{

    public class EnemyCharacterTargetingHandler : MonoBehaviour
    {
        protected Vector3 _aimDirection;
        protected Collider[] _hit;
        protected Vector3 _raycastDirection;
        protected Collider _potentialPlayerHit;
        protected Collider _potentialInteractableHit;

        public LayerMask targetsMask;
        public LayerMask harvestResourceMask;
        public LayerMask obstacleMask;

        protected Vector3 _raycastOrigin;

        public float scanRadius = 15f;

        public float durationBetweenScans = 0.2f;
        protected float _lastScanTimestamp = 0f;

        public EnemyCharacter enemyCharacter;
        public PlayerCharacter playerTarget;
        public PlayerCharacter previousPlayerTarget;

        public bool drawDebugRadius = true;


        private void Awake()
        {
            enemyCharacter = gameObject.GetComponent<EnemyCharacter>();
        }

        void Update()
        {
            DetermineRaycastOrigin();
            ScanIfNeeded();
        }

        /// <summary>
        /// Scans for targets by performing an overlap detection, then verifying line of fire with a boxcast
        /// </summary>
        /// <returns></returns>
        protected bool ScanForPlayerTarget()
        {
            previousPlayerTarget = playerTarget;
            playerTarget = null;

            float nearestDistance = float.MaxValue;
            float distance;

            //_hit = Physics.OverlapSphere(currentlyEquippedWeapon.transform.position, ScanRadius, TargetsMask);
            _hit = Physics.OverlapSphere(transform.position, scanRadius, targetsMask);

            if (_hit.Length > 0)
            {
                foreach (Collider collider in _hit)
                {
                    distance = (_raycastOrigin - collider.transform.position).sqrMagnitude;
                    if (distance < nearestDistance)
                    {
                        _potentialPlayerHit = collider;
                        nearestDistance = distance;
                    }
                }

                // we cast a ray to make sure there's no obstacle
                _raycastDirection = _potentialPlayerHit.transform.position - _raycastOrigin;
                RaycastHit obstacleHit = DebugRaycast3D(_raycastOrigin, _raycastDirection, Vector3.Distance(_potentialPlayerHit.transform.position, _raycastOrigin), obstacleMask.value, Color.yellow, true);
                if (obstacleHit.collider == null)
                {
                    playerTarget = _potentialPlayerHit.GetComponent<PlayerCharacter>();

                    //FoxlairEventManager.Instance.TargetingSystem_OnTargetEnemyAcquired_Event?.Invoke(PlayerTarget);
                    enemyCharacter.PlayerSpotted(playerTarget);
                    return true;
                }
                else
                {
                    //FoxlairEventManager.Instance.TargetingSystem_OnTargetEnemyLost_Event?.Invoke();
                    enemyCharacter.PlayerLost(previousPlayerTarget);
                    return false;
                }
            }
            else
            {
                enemyCharacter.PlayerLost(previousPlayerTarget);
                //FoxlairEventManager.Instance.TargetingSystem_OnTargetEnemyLost_Event?.Invoke();
                return false;
            }
        }

        protected virtual void ScanIfNeeded()
        {
            if (Time.time - _lastScanTimestamp > durationBetweenScans)
            {
                ScanForPlayerTarget();
                _lastScanTimestamp = Time.time;
            }
        }

        protected virtual void OnDrawGizmos()
        {
            if (drawDebugRadius)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_raycastOrigin, scanRadius);
            }
        }

        protected void DetermineRaycastOrigin()
        {
            _raycastOrigin = this.transform.position;
        }


        /// <summary>
        /// Draws a debug ray in 3D and does the actual raycast
        /// </summary>
        /// <returns>The raycast hit.</returns>
        /// <param name="rayOriginPoint">Ray origin point.</param>
        /// <param name="rayDirection">Ray direction.</param>
        /// <param name="rayDistance">Ray distance.</param>
        /// <param name="mask">Mask.</param>
        /// <param name="debug">If set to <c>true</c> debug.</param>
        /// <param name="color">Color.</param>
        /// <param name="drawGizmo">If set to <c>true</c> draw gizmo.</param>
        public static RaycastHit DebugRaycast3D(Vector3 rayOriginPoint, Vector3 rayDirection, float rayDistance, LayerMask mask, Color color, bool drawGizmo = false)
        {
            if (drawGizmo)
            {
                Debug.DrawRay(rayOriginPoint, rayDirection * rayDistance, color);
            }
            RaycastHit hit;
            Physics.Raycast(rayOriginPoint, rayDirection, out hit, rayDistance, mask);
            return hit;
        }
    }
}