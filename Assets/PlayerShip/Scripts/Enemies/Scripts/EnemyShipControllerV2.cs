using UnityEngine;
using System;
using Ascent.Weaponry;
using Ascent.WaypointsSystem;

using PathFindingResult = Ascent.WaypointsSystem.PathFindingResult;
using Waypoint = Ascent.WaypointsSystem.RuntimeWaypoint;
using Ascent.Managers.Pools;
using Ascent.Items;
using Ascent.Managers.Game;

namespace Ascent.Enemies
{
    public class EnemyShipControllerV2 : AutoPilotedShip, IHittableObject
    {
        public static event Action OnEnemyDestroy;

        [Header("Enemy Ship Settings")]
        public WaypointNetworkV2 waypoints;
        public LayerMask targetLayer;
        public LayerMask layersToIgnoreInSightTest;
        public float radarRadius = 10f;
        public float sightTestSphereCastRadius = 1.5f;
        public bool debugPath = true;
        public bool debugProcessing = true;
        public bool debugReachabilityTest = true;
        public Color debugPathColor = Color.green;
        public Color debugProcessingColor = Color.gray;
        public bool alive = true;
        public float delayToFirstShot = 2f;
        public PlayerShip.PlayerShipWeaponryController player;

        [Header("Loot Settings")]
        public ItemIncreaseHull itemIncreaseHullPrefab;
        public ItemIncreaseShield itemIncreaseShieldPrefab;
        public ItemLaserAmmo itemLaserAmmoPrefab;
        public ItemUnguidedMissileAmmo itemUnguidedMissileAmmoPrefab;
        public ItemGuidedMissileAmmo itemGuidedMissileAmmoPrefab;

        private float health = 6f;

        [Header("Weapons Settings")]
        public PlasmaGunCannon leftCannon;
        public PlasmaGunCannon rightCannon;

        protected enum States
        {
            WaitingWaypointSystemToGetReady,
            SearchingForTarget,
            AttackTarget,
            HuntForTarget
        }
        protected States state;
        protected Action currentStateUpdateMethod;
        protected Collider target;
        protected PathFindingResult currentPath;
        protected int currentPathIndex;
        protected float distanceToReachPathNode = 1f;
        protected float refreshPathToTargetWhenHeMovesBy = 1f;
        protected Vector3? targetPositionAtLastPathFinding;
        public float attackRange = 10f;
        protected float nextDodgeAtTime;
        protected float dodgeIntervalMin = 2f;
        protected float dodgeIntervalMax = 4f;

        // Pause
        protected Vector3 savedVelocityBeforePause;
        protected Vector3 savedAngularVelocityBeforePause;

        protected void Start()
        {
            if (waypoints == null)
                Debug.LogWarning("waypoints is null.");

            if (leftCannon == null)
                throw new Exception("leftCannon is null.");

            if (rightCannon == null)
                throw new Exception("rightCannon is null.");

            if (itemIncreaseHullPrefab == null)
                throw new Exception("itemIncreaseHullPrefab is null.");

            if (itemIncreaseShieldPrefab == null)
                throw new Exception("itemIncreaseShieldPrefab is null.");

            if (itemLaserAmmoPrefab == null)
                throw new Exception("itemLaserAmmoPrefab is null.");

            if (itemUnguidedMissileAmmoPrefab == null)
                throw new Exception("itemUnguidedMissileAmmoPrefab is null.");

            if (itemGuidedMissileAmmoPrefab == null)
                throw new Exception("itemGuidedMissileAmmoPrefab is null.");

            SetState(States.WaitingWaypointSystemToGetReady, true);

            PauseManager.OnPause += Pause;
            PauseManager.OnUnpause += Unpause;

            player = FindObjectOfType<PlayerShip.PlayerShipWeaponryController>();
        }
        protected virtual void OnDestroy()
        {
            PauseManager.OnPause -= Pause;
            PauseManager.OnUnpause -= Unpause;
        }

        protected override void Update()
        {
            if (!PauseManager.instance.paused)
            {
                if (currentStateUpdateMethod != null)
                {
                    currentStateUpdateMethod();
                }

                base.Update();
            }
        }

        public void Pause()
        {
            savedVelocityBeforePause = myRigidbody.velocity;
            savedAngularVelocityBeforePause = myRigidbody.angularVelocity;
            myRigidbody.isKinematic = true;
            myRigidbody.AddForce(Vector3.zero, ForceMode.VelocityChange);
            myRigidbody.AddTorque(Vector3.zero, ForceMode.VelocityChange);
        }

        public void Unpause(float pauseDeltaTime)
        {
            myRigidbody.isKinematic = false;
            myRigidbody.AddForce(savedVelocityBeforePause, ForceMode.VelocityChange);
            myRigidbody.AddTorque(savedAngularVelocityBeforePause, ForceMode.VelocityChange);
        }

        public void PlayerHasDied()
        {
            pilotTargetPosition = null;
            SetState(States.SearchingForTarget);
        }

        protected void SetState(States newState, bool ignoreSameStateCheck = false)
        {
            if (!ignoreSameStateCheck && newState == state)
            {
                return;
            }

            switch (newState)
            {
                case States.WaitingWaypointSystemToGetReady:
                    currentStateUpdateMethod = UpdateWaitingWaypointSystemToGetReady;
                    break;

                case States.SearchingForTarget:
                    currentStateUpdateMethod = UpdateSearchingForTarget;
                    break;

                case States.AttackTarget: 
                    attackStartTime = Time.time;
                    currentStateUpdateMethod = UpdateAttackTarget;
                    break;

                case States.HuntForTarget:
                    currentStateUpdateMethod = UpdateHuntForTarget;
                    break;

                default:
                    throw new Exception("Unexpected state.");
            }

            state = newState;
            //Debug.LogFormat("New state: {0}", state);
        }

        protected bool IsColliderInSight(Collider collider)
        {
            RaycastHit hit;
            var ray = new Ray(transform.position, collider.transform.position - transform.position);
            if (Physics.Raycast(ray, out hit, float.MaxValue, ~layersToIgnoreInSightTest))
            {
                if (hit.collider == collider)
                {
                    return true;
                }
                else
                {
                    //Debug.LogFormat("IsInSight false. Looking for {0}, found {1}.", collider.name, hit.collider.name); 
                    return false;
                }
            }

            //Debug.LogFormat("IsInSight false. Raycast return nothing."); 
            return false;
        }

        protected bool IsPositionDirectlyReachable(Vector3 targetPosition)
        {
            var sqrMagnitudeToPosition = (targetPosition - transform.position).sqrMagnitude;

            // First, check for straight line of sight, from center to center.
            RaycastHit hit;
            if (Physics.Raycast(transform.position, targetPosition - transform.position, out hit, float.MaxValue, ~layersToIgnoreInSightTest))
            {
                var sqrMagnitudeToHit = (hit.point - transform.position).sqrMagnitude;

                // If the hit point is closer than the position that we want to see, there is something in between.
                if (sqrMagnitudeToHit < sqrMagnitudeToPosition)
                {
                    Debug.DrawLine(transform.position, hit.point);
                    return false;
                }
            }

            // Now we check if the boundaries of the object are also directly reachable to destination point.
            var hits =
                PhysicsExtensions.RaycastCilinderWithEightRays(
                    transform.position,
                    targetPosition,
                    sightTestSphereCastRadius,
                    ~layersToIgnoreInSightTest,
                    debugReachabilityTest
                );

            var obstructed = false;
            foreach (var h in hits)
            {
                var sqrMagnitudeToHit = (h.point - transform.position).sqrMagnitude;

                // If the hit point is closer than the position that we want to see, there is something in between.
                if (sqrMagnitudeToHit < sqrMagnitudeToPosition)
                {
                    obstructed = true;
                    break;
                }
            }

            return !obstructed;

            //var origins = new Vector3[8];
            //origins[0] = transform.TransformPoint(Vector3.up * sightTestSphereCastRadius);
            //origins[1] = transform.TransformPoint((Vector3.up + Vector3.right).normalized * sightTestSphereCastRadius);
            //origins[2] = transform.TransformPoint(Vector3.right * sightTestSphereCastRadius);
            //origins[3] = transform.TransformPoint((Vector3.down + Vector3.right).normalized * sightTestSphereCastRadius);
            //origins[4] = transform.TransformPoint(Vector3.down * sightTestSphereCastRadius);
            //origins[5] = transform.TransformPoint((Vector3.down + Vector3.left).normalized * sightTestSphereCastRadius);
            //origins[6] = transform.TransformPoint(Vector3.left * sightTestSphereCastRadius);
            //origins[7] = transform.TransformPoint((Vector3.up + Vector3.left).normalized * sightTestSphereCastRadius);

            //var direction = targetPosition - transform.position;
            //var sqrMagnitudeToPosition = (targetPosition - transform.position).sqrMagnitude;

            //RaycastHit hit;
            //foreach (var origin in origins)
            //{
            //    if (Physics.Raycast(origin, direction, out hit, float.MaxValue, ~layersToIgnoreInSightTest))
            //    {
            //        if (debugReachabilityTest)
            //        {
            //            Debug.DrawLine(origin, hit.point);
            //        }

            //        var sqrMagnitudeToHit = (hit.point - transform.position).sqrMagnitude;

            //        // If the hit point is closer than the position that we want to see, there is something in between.
            //        if (sqrMagnitudeToHit < sqrMagnitudeToPosition)
            //        {
            //            return false;
            //        }
            //    }
            //}

            //// If nothing was hit, there is nothing between the position and the observer.
            //return true;
        }

        protected void UpdateWaitingWaypointSystemToGetReady()
        {
            if (waypoints == null || waypoints.isReady)
            {
                if (target == null)
                {
                    SetState(States.SearchingForTarget);
                }
                else
                {
                    SetState(States.AttackTarget);
                }
            }
        }

        protected void UpdateSearchingForTarget()
        {
            var colliders = Physics.OverlapSphere(transform.position, radarRadius, targetLayer);
            foreach (var collider in colliders)
            {
                if (IsColliderInSight(collider))
                {
                    target = collider;
                    SetState(States.AttackTarget);
                    break;
                }
            }
        }

        public void SetTarget(Collider collider)
        {
            target = collider;
            SetState(States.AttackTarget);
        }

        protected float attackStartTime = 0;
        protected void UpdateAttackTarget()
        {
            if (!IsColliderInSight(target))
            {
                autoLookToPilotTargetPosition = true;

                // Lost sight of target.
                pilotTargetPosition = null;
                SetState(States.HuntForTarget);
            }
            else
            {
                autoLookToPilotTargetPosition = false;
                if (Vector3.Distance(transform.position, target.transform.position) < attackRange)
                {
                    // Dodge!
                    if (Time.time >= nextDodgeAtTime)
                    {
                        // Set next dodge time.
                        nextDodgeAtTime = Time.time + UnityEngine.Random.Range(dodgeIntervalMin, dodgeIntervalMax);

                        // Try to find a random position around the target and go there.
                        // If not found in [max] attempts, give up this time.
                        for (var i = 0; i < 30; i++)
                        {
                            var newPosition = target.transform.position + (UnityEngine.Random.rotation * (Vector3.forward * attackRange));
                            if (IsPositionDirectlyReachable(newPosition))
                            {
                                pilotTargetPosition = newPosition;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    pilotTargetPosition = target.transform.position;
                }

                // Aim to target
                LookToPosition(target.transform.position);

                // Delay the first shot.
                if (Time.time - attackStartTime > delayToFirstShot)
                {
                    // Shoot!
                    leftCannon.Shoot();
                    rightCannon.Shoot();
                }
            }
        }

        protected RuntimeWaypoint FindNearestReachableWaypoint(Vector3 position)
        {
            var orderedWaypoints = waypoints.GetWaypointsOrderedByProximity(position);

            for (var i = 0; i < orderedWaypoints.Length; i++)
            {
                var wp = orderedWaypoints[i];

                if (PhysicsExtensions.CheckSightBetweenTwoPositions(position, wp.position, ~layersToIgnoreInSightTest))
                {
                    if (debugReachabilityTest)
                        Debug.DrawLine(position, wp.position, Color.green);

                    return wp;
                }
                else if (debugReachabilityTest)
                {
                    Debug.DrawLine(position, wp.position, Color.red);
                }
            }

            return null;
        }

        protected void UpdateHuntForTarget()
        {
            if (IsColliderInSight(target))
            {
                // Target is in sight again!
                if(!player.hullAlarmEnabled)
                    AudioManager.instance.Play(AudioBank.MUS_TENSION, this.gameObject);

                	AudioManager.instance.Play(AudioBank.SFX_ENEMY_TALK, this.gameObject);
                currentPath = null;
                targetPositionAtLastPathFinding = null;
                SetState(States.AttackTarget);
            }
            else if (waypoints != null)
            {
                if (targetPositionAtLastPathFinding == null || waypoints.connectionToggledLastFrame ||
                    Vector3.Distance(targetPositionAtLastPathFinding.Value, target.transform.position) > refreshPathToTargetWhenHeMovesBy)
                {
                    targetPositionAtLastPathFinding = target.transform.position;
                    var enemyWaypoint = FindNearestReachableWaypoint(transform.position);
                    var playerWaypoint = FindNearestReachableWaypoint(target.transform.position);
                    currentPath = waypoints.FindPath(enemyWaypoint, playerWaypoint);
                    currentPathIndex = 0;
                }

                if (currentPath != null && currentPath.success)
                {
                    if (Vector3.Distance(transform.position, currentPath.path[currentPathIndex].position) < distanceToReachPathNode)
                    {
                        if (currentPathIndex < currentPath.path.Length - 1)
                        {
                            currentPathIndex++;
                        }
                    }

                    for (var i = currentPath.path.Length - 1; i > currentPathIndex; i--)
                    {
                        if (IsPositionDirectlyReachable(currentPath.path[i].position))
                        {
                            currentPathIndex = i;
                        }
                    }

                    pilotTargetPosition = currentPath.path[currentPathIndex].position;
                }
                else
                {
                    //AudioManager.instance.Play(AudioBank.MUS_NONE, this.gameObject);
                    currentPath = null;
                    currentPathIndex = 0;
                    pilotTargetPosition = null;
                }
            }
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if (debugProcessing && currentPath != null && currentPath.cameFrom != null && currentPath.success)
            {
                Gizmos.color = debugProcessingColor;
                foreach (var item in currentPath.cameFrom)
                {
                    Gizmos.DrawWireSphere(item.Key.position, .1f);

                    if (item.Value != null)
                    {
                        Gizmos.DrawWireSphere(item.Value.position, .1f);
                        Gizmos.DrawLine(item.Key.position, item.Value.position);
                    }
                }
            }

            if (debugPath && currentPath != null && currentPath.success)
            {
                Gizmos.color = debugPathColor;
                Waypoint lastWp = null;
                foreach (var wp in currentPath.path)
                {
                    Gizmos.DrawWireSphere(wp.position, .1f);

                    if (lastWp != null)
                    {
                        Gizmos.DrawLine(wp.position, lastWp.position);
                    }

                    lastWp = wp;
                }
            }
        }

        public void OnHitByWeapon(float damage, bool shotByPlayer)
        {
            TakeDamage(damage);
        }

        public void TakeDamage(float damage)
        {
            health -= damage;

            if (health <= 0)
            {
                alive = false;
                PoolManager.instance.Spawn<EnemyExplosion01>(null, transform);
                Destroy(this.gameObject.transform.parent.gameObject);
                SpawnLoot();
                AudioManager.instance.Play(AudioBank.MUS_NONE, this.gameObject);

                if (OnEnemyDestroy != null)
                    OnEnemyDestroy();
            }
        }

        protected void SpawnLoot()
        {
            if (UnityEngine.Random.value > .7f)
            {
                var item = (ItemIncreaseHull)Instantiate(itemIncreaseHullPrefab, transform.position, Quaternion.identity);
                item.transform.parent = GameManager.instance.gameRoot.transform;
                item.GetComponent<Rigidbody>().AddForce(RandomExtensions.vector3 * 30f);
            }

            if (UnityEngine.Random.value > .7f)
            {
                var item = (ItemIncreaseShield)Instantiate(itemIncreaseShieldPrefab, transform.position, Quaternion.identity);
                item.transform.parent = GameManager.instance.gameRoot.transform;
                item.GetComponent<Rigidbody>().AddForce(RandomExtensions.vector3 * 30f);
            }

            if (UnityEngine.Random.value > .7f)
            {
                var item = (ItemLaserAmmo)Instantiate(itemLaserAmmoPrefab, transform.position, Quaternion.identity);
                item.transform.parent = GameManager.instance.gameRoot.transform;
                item.GetComponent<Rigidbody>().AddForce(RandomExtensions.vector3 * 30f);
            }

            if (UnityEngine.Random.value > .7f)
            {
                var item = (ItemUnguidedMissileAmmo)Instantiate(itemUnguidedMissileAmmoPrefab, transform.position, Quaternion.identity);
                item.transform.parent = GameManager.instance.gameRoot.transform;
                item.GetComponent<Rigidbody>().AddForce(RandomExtensions.vector3 * 30f);
            }

            if (UnityEngine.Random.value > .7f)
            {
                var item = (ItemGuidedMissileAmmo)Instantiate(itemGuidedMissileAmmoPrefab, transform.position, Quaternion.identity);
                item.transform.parent = GameManager.instance.gameRoot.transform;
                item.GetComponent<Rigidbody>().AddForce(RandomExtensions.vector3 * 30f);
            }
        }
    }
}