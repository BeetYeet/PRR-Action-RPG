using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
	public abstract class Health : MonoBehaviour
	{
		[SerializeField]
		int health = 100;
		public int CurrentHealth { get { return health; } internal set { health = value; } }
		protected abstract int ReviveHealth { get; set; }
		protected abstract int MaxHealth { get; set; }
		protected abstract int DefaultHealth { get; set; }

		public virtual Animator animator { get { return this.GetComponent<Animator>(); } }

		public bool Alive
		{
			get { return CurrentHealth > 0; }
			set
			{
				if (Alive)
				{
					if (!value)
					{
						CurrentHealth = 0;
					}
				}
				else
				{
					if (value)
					{
						Revive();
					}
				}
			}
		}

		public void Revive()
		{
			CurrentHealth = ReviveHealth;
			OnRevive();
		}

		public float HealthPart { get { return CurrentHealth / (float)MaxHealth; } }

		public virtual bool TakeDamage(int damage)
		{
			if (!Alive)
				return false;
			bool wasAlive = Alive;
			CurrentHealth -= damage;
			if (CurrentHealth < 0)
				CurrentHealth = 0;
			if (CurrentHealth > MaxHealth)
				CurrentHealth = MaxHealth;

			if (wasAlive && !Alive)
			{
				OnDeath();
				GetComponent<ActionScheduler>().CancelCurrentAction();
				return true;
			}

			return false;
		}


		public virtual event System.Action OnDeath;
		public virtual event System.Action OnRevive;
	}
}
