using UnityEngine;
using Foxlair.Tools;
using Foxlair.Enemies;
using Foxlair.Harvesting;

namespace Foxlair.Character.Targeting
{
    public class CharacterTargetingHandler : MonoBehaviour
    {
        protected Vector3 _aimDirection;
        protected Collider[] _hit;
        protected Vector3 _raycastDirection;
        protected Collider _potentialEnemyHit;
        protected Collider _potentialHarvestResourceHit;


        public LayerMask TargetsMask;
        public LayerMask HarvestResourceMask;
        public LayerMask ObstacleMask;

        protected Vector3 _raycastOrigin;

        public float ScanRadius = 15f;

        public float DurationBetweenScans = 0.2f;
        protected float _lastScanTimestamp = 0f;

        public Enemy EnemyTarget;
        public ResourceNode HarvestResourceTarget;

        public bool DrawDebugRadius = true;

        //Weapon currentlyEquippedWeapon;

        void Update()
        {
            DetermineRaycastOrigin();
            ScanIfNeeded();
        }

        /// <summary>
        /// Scans for targets by performing an overlap detection, then verifying line of fire with a boxcast
        /// </summary>
        /// <returns></returns>
        protected bool ScanForEnemyTargets()
        {
            EnemyTarget = null;

            float nearestDistance = float.MaxValue;
            float distance;

            //_hit = Physics.OverlapSphere(currentlyEquippedWeapon.transform.position, ScanRadius, TargetsMask);
            _hit = Physics.OverlapSphere(transform.position, ScanRadius, TargetsMask);

            if (_hit.Length > 0)
            {
                foreach (Collider collider in _hit)
                {
                    distance = (_raycastOrigin - collider.transform.position).sqrMagnitude;
                    if (distance < nearestDistance)
                    {
                        _potentialEnemyHit = collider;
                        nearestDistance = distance;
                    }
                }

                // we cast a ray to make sure there's no obstacle
                _raycastDirection = _potentialEnemyHit.transform.position - _raycastOrigin;
                RaycastHit obstacleHit = DebugRaycast3D(_raycastOrigin, _raycastDirection, Vector3.Distance(_potentialEnemyHit.transform.position, _raycastOrigin), ObstacleMask.value, Color.yellow, true);
                if (obstacleHit.collider == null)
                {
                    EnemyTarget = _potentialEnemyHit.GetComponent<Enemy>();
                    PlayerManager.Instance.PlayerTargetEnemy = EnemyTarget;

                    return true;
                }
                else
                {
                    PlayerManager.Instance.PlayerTargetEnemy = null;
                    return false;
                }
            }
            else
            {
                PlayerManager.Instance.PlayerTargetEnemy = null;
                return false;
            }
        }


        /// <summary>
        /// Scans for Harvest Resource targets by performing an overlap detection, then verifying line of fire with a boxcast
        /// </summary>
        /// <returns></returns>
        protected bool ScanForHarvestResourceTargets()
        {
            HarvestResourceTarget = null;

            float nearestDistance = float.MaxValue;
            float distance;

            //_hit = Physics.OverlapSphere(currentlyEquippedWeapon.transform.position, ScanRadius, TargetsMask);
            _hit = Physics.OverlapSphere(transform.position, ScanRadius, HarvestResourceMask);

            if (_hit.Length > 0)
            {
                foreach (Collider collider in _hit)
                {
                    distance = (_raycastOrigin - collider.transform.position).sqrMagnitude;
                    if (distance < nearestDistance)
                    {
                        _potentialHarvestResourceHit = collider;
                        nearestDistance = distance;
                    }
                }

                // we cast a ray to make sure there's no obstacle
                _raycastDirection = _potentialHarvestResourceHit.transform.position - _raycastOrigin;
                RaycastHit obstacleHit = DebugRaycast3D(_raycastOrigin, _raycastDirection, Vector3.Distance(_potentialHarvestResourceHit.transform.position, _raycastOrigin), ObstacleMask.value, Color.yellow, true);
                if (obstacleHit.collider == null)
                {
                    HarvestResourceTarget = _potentialHarvestResourceHit.GetComponent<ResourceNode>();
                    PlayerManager.Instance.PlayerTargetResourceNode = HarvestResourceTarget;

                    return true;
                }
                else
                {
                    PlayerManager.Instance.PlayerTargetResourceNode = null;

                    return false;
                }

            }
            else
            {
                PlayerManager.Instance.PlayerTargetResourceNode = null;

                return false;
            }
        }


        /// <summary>
        /// Performs a periodic scan
        /// </summary>
        protected virtual void ScanIfNeeded()
        {
            if (Time.time - _lastScanTimestamp > DurationBetweenScans)
            {
                ScanForEnemyTargets();
                ScanForHarvestResourceTargets();
                _lastScanTimestamp = Time.time;
            }
        }



        protected virtual void OnDrawGizmos()
        {
            if (DrawDebugRadius)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_raycastOrigin, ScanRadius);
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