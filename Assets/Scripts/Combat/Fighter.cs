using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;
using RPG.Movement;

namespace RPG.Combat
{
	public class Fighter : MonoBehaviour, IAction
	{
		[SerializeField] float attackRange = 2f;
		[SerializeField] float timeBetweenAttacks = 1f;
		public float attackRotationSpeed = 100f;
		Transform target;
		Mover mover;
		[SerializeField]
		List<Faction> attackableAlignments = new List<Faction>() { Faction.Player };

		[SerializeField]
		int damage = 10;


		float timeSinceLastAttack = Mathf.Infinity;

		private void Start()
		{
			mover = GetComponent<Mover>();
		}

		private void Update()
		{
			Health health = GetComponent<Health>();
			if (health != null && !health.Alive)
				return;
			timeSinceLastAttack += Time.deltaTime;

			if (target == null)
			{
				return;
			}
			if (!GetIsInRange())
			{
				mover.MoveTo(target.position, 1f);
			}
			else
			{
				mover.Cancel();
				transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target.position - transform.position, Vector3.up), attackRotationSpeed);
				if (timeSinceLastAttack > timeBetweenAttacks)
				{
					AttackBehavoiur();
					timeSinceLastAttack = 0f;
				}
			}

		}

		private void AttackBehavoiur()
		{
			Animator anim = GetComponent<Animator>();
			if (anim != null)
				anim.SetTrigger("attack");
		}

		void Hit()
		{
			if (target == null)
				return;
			Health targetHealth = target.GetComponent<Health>();
			if (targetHealth != null)
			{
				bool died = targetHealth.TakeDamage(damage);
				if (died)
				{
					target = null;
					Debug.Log($"Hit enemy for {damage} damage and killed them");
				}
				else
				{
					Debug.Log($"Hit enemy for {damage} damage and didnt kill them");
				}
			}
		}

		private bool GetIsInRange()
		{
			return attackRange > Vector3.Distance(target.position, transform.position); ;
		}

		public void TryAttack(CombatTarget combatTarget)
		{
			if (!CanAttack(combatTarget))
				return;
			GetComponent<ActionScheduler>().StartAction(this);
			target = combatTarget.transform;
		}

		public bool CanAttack(CombatTarget combatTarget)
		{
			return attackableAlignments.Contains(combatTarget.Alignment) && combatTarget.GetComponent<Health>().Alive;
		}

		public void Cancel()
		{
			target = null;
		}
	}
	public enum Faction { Player, Enemy }
}