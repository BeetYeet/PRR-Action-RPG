using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Combat;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        private NavMeshAgent agent;
        private Animator anim;
        private Health health;
        public float movementSpeed = 5.66f;
        private float currentSpeed = 5.66f;

        public bool attackIsAnimating = false;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            health = GetComponent<Health>();
        }

        private void Update()
        {
            agent.enabled = health.Alive;
            UpdateAnimator();
            if (attackIsAnimating)
            {
                agent.speed = currentSpeed / 2f;
            }
            else
            {
                agent.speed = currentSpeed;
            }
        }

        public void StartMoveAction(Vector3 position, float speedFraction = 1f)
        {
            if (attackIsAnimating)
                return;
            if (GetComponent<Health>() != null && !GetComponent<Health>().Alive)
                return;
            ActionScheduler actionScheduler = GetComponent<ActionScheduler>();
            actionScheduler.CancelCurrentAction();
            actionScheduler.StartAction(this);
            MoveTo(position, Mathf.Clamp01(speedFraction));
        }

        public void MoveTo(Vector3 posititon, float speedFraction)
        {
            if (!health.Alive || attackIsAnimating)
                return;
            currentSpeed = movementSpeed * Mathf.Clamp01(speedFraction);
            agent.speed = currentSpeed;
            if (agent.isStopped)
                agent.isStopped = false;
            agent.destination = posititon;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            anim.SetFloat("forwardSpeed", localVelocity.z);
        }

        public void Cancel()
        {
            agent.isStopped = true;
        }
    }
}