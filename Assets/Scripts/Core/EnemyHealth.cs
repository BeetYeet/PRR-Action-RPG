using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
namespace RPG.Core
{
	[RequireComponent(typeof(CombatTarget))]
	public class EnemyHealth : Health
	{
		[SerializeField]
		private int reviveHealth = 50;
		protected override int ReviveHealth { get { return reviveHealth; } set { reviveHealth = value; } }
		[SerializeField]
		private int maxHealth = 50;
		protected override int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
		[SerializeField]
		private int defaultHealth = 50;
		protected override int DefaultHealth { get { return defaultHealth; } set { defaultHealth = value; } }


		private void Start()
		{
			OnDeath += Death;
		}

		void Death()
		{
			if (animator != null) animator.SetTrigger("die");
			foreach(Collider col in GetComponents<Collider>()){
				col.enabled = false;
			}
			Invoke("Destruct", 30f);
		}

		void Destruct()
		{
			Destroy(gameObject);
		}
	}
}