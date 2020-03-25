using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        NavMeshAgent agent;
        Animator anim;
        Health health;
        public float movementSpeed = 5.66f;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            agent.enabled = health.Alive;
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 position, float speedFraction = 1f)
        {
            if (GetComponent<Health>() != null && !GetComponent<Health>().Alive)
                return;
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(position, Mathf.Clamp01(speedFraction));
        }

        public void MoveTo(Vector3 posititon, float speedFraction)
        {
            agent.speed = movementSpeed * Mathf.Clamp01(speedFraction);
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