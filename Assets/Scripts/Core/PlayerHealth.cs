using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;

namespace RPG.Core
{
	[RequireComponent(typeof(CombatTarget))]
	public class PlayerHealth : Health
	{
		[SerializeField]
		private int reviveHealth = 10;
		protected override int ReviveHealth { get { return reviveHealth; } set { reviveHealth = value; } }

		[SerializeField]
		private int maxHealth = 100;
		protected override int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }

		[SerializeField]
		private int defaultHealth = 100;
		protected override int DefaultHealth { get { return defaultHealth; } set { defaultHealth = value; } }

		private void Start()
		{
			OnDeath += () => { Debug.Log("player died"); if (animator != null) animator.SetTrigger("die"); };
		}

	}
}