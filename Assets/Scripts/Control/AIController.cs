using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Movement;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float dropChaseDistance = 10f;
        Fighter fighter;
        GameObject player;
        Mover mover;
        float alertness = 1.99f;
        public float lazyTime = 20f;
        public float becomeAlertTime = 1f;
        public float becomeSuspiciousTime = 1f;
        public float alertDecayTime = 20f;
        public float suspiciousDecayTime = 5f;
        public Transform pathParent;
        public Vector3 basePosition;
        int targetWaypoint = 0;
        bool atNextWaypoint = false;
        public float waypointStayTime = 1f;


        private void Start()
        {
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
            if (pathParent != null)
                if (pathParent.childCount != 0f)
                    basePosition = pathParent.GetChild(GetClosestPoint()).position;
                else
                    basePosition = transform.position;
            else
                basePosition = transform.position;

            targetWaypoint = GetClosestPoint();
        }



        bool PlayerIsInRange()
        {
            return DistanceToPlayer() < GetSearchRange();
        }

        float DistanceToPlayer()
        {
            return Vector3.Distance(player.transform.position, transform.position);
        }

        void Update()
        {
            CalculateState();
            UpdatePath();
        }

        int GetClosestPoint()
        {
            if (pathParent == null)
                return 0;
            int closest = 0;
            float closestDist = Mathf.Infinity;
            for (int i = 0; i < pathParent.childCount; i++)
            {
                float dist = Vector3.Distance(pathParent.GetChild(i).position, transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = i;
                }
            }
            return closest;
        }

        private void UpdatePath()
        {
            if (GetAlertness() != AlertState.Aware && GetAlertness() != AlertState.Alert)
                return;
            if (pathParent == null)
            {
                PatheToBase();
                return;
            }
            if (pathParent.childCount == 1)
                Pathe();
            if (pathParent.childCount < 2)
                return;

            // not chasing, pathe along path
            if (!atNextWaypoint && Vector3.Distance(transform.position, GetNextPoint()) < .2f)
            {
                atNextWaypoint = true;
                Invoke("NextWaypoint", waypointStayTime);
            }
            Pathe();

        }

        void PatheToBase()
        {
            mover.MoveTo(basePosition, 0.5f);
        }

        void NextWaypoint()
        {
            atNextWaypoint = false;
            targetWaypoint++;
            if (targetWaypoint == pathParent.childCount)
                targetWaypoint = 0;
        }

        void Pathe()
        {
            mover.MoveTo(GetNextPoint(), 0.5f);
        }


        Vector3 GetNextPoint()
        {
            try
            {
                return pathParent.GetChild(targetWaypoint).position;
            }
            catch (System.NullReferenceException e)
            {
                return basePosition;
            }
        }

        public float GetAlertnessSpeed()
        {
            switch (GetAlertness())
            {
                default:
                    return 0.4f;
                case AlertState.Chasing:
                    return 1f;
                case AlertState.Suspicious:
                    return 0.65f;
            }
        }

        private void CalculateState()
        {
            switch (GetAlertness())
            {
                default:
                    if (PlayerIsInRange())
                    {
                        alertness += Time.deltaTime / becomeAlertTime;
                    }
                    else
                    {
                        alertness -= Time.deltaTime / lazyTime;
                    }
                    break;
                case AlertState.Alert:
                    if (PlayerIsInRange())
                    {
                        alertness += Time.deltaTime / becomeSuspiciousTime;
                    }
                    else
                    {
                        alertness -= Time.deltaTime / alertDecayTime;
                    }
                    break;
                case AlertState.Suspicious:
                    if (PlayerIsInRange())
                    {
                        alertness += 1f;
                    }
                    else
                    {
                        alertness -= Time.deltaTime / suspiciousDecayTime;
                        if (pathParent != null)
                            targetWaypoint = GetClosestPoint();
                    }
                    break;
                case AlertState.Chasing:
                    if (!PlayerIsInRange())
                    {
                        alertness = 2.99f;
                        fighter.Cancel();
                    }
                    else
                        fighter.TryAttack(player.GetComponent<CombatTarget>());
                    break;
            }
        }

        private AlertState GetAlertness()
        {
            return (AlertState)Mathf.FloorToInt(Mathf.Clamp(alertness, 0, 3.5f));
        }

        private void OnDrawGizmosSelected()
        {
            DrawPath();
        }

        private void OnDrawGizmos()
        {
            DrawAlert();
        }

        private void DrawPath()
        {
            if (pathParent == null || pathParent.childCount < 2)
                return;
            Gizmos.color = Color.gray;
            Gizmos.DrawLine(pathParent.GetChild(0).position, pathParent.GetChild(pathParent.childCount - 1).position);
            for (int i = 0; i < pathParent.childCount; i++)
            {
                Gizmos.color = Color.gray;
                if (i != pathParent.childCount - 1)
                    Gizmos.DrawLine(pathParent.GetChild(i).position, pathParent.GetChild(i + 1).position);
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(pathParent.GetChild(i).position, .3f);
            }
        }

        void DrawAlert()
        {
            float range = GetSearchRange();
            switch (GetAlertness())
            {
                default:
                    Gizmos.color = Color.green;
                    break;
                case AlertState.Alert:
                    Gizmos.color = Color.blue;
                    break;
                case AlertState.Suspicious:
                    Gizmos.color = Color.Lerp(Color.red, Color.blue, 0.5f);
                    break;
                case AlertState.Chasing:
                    Gizmos.color = Color.Lerp(Color.red, Color.yellow, 0.5f);
                    break;
            }
            Gizmos.DrawWireSphere(transform.position, range);
        }

        float GetSearchRange()
        {
            switch (GetAlertness())
            {
                default:
                    return chaseDistance * .75f;
                case AlertState.Alert:
                    return chaseDistance;
                case AlertState.Suspicious:
                    return chaseDistance * 1.5f;
                case AlertState.Chasing:
                    return dropChaseDistance;
            }
        }


        enum AlertState
        {
            Aware, Alert, Suspicious, Chasing
        }
    }
}