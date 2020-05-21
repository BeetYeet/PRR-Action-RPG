using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public abstract class Health : MonoBehaviour
    {
        [SerializeField]
        internal float health = 100;

        public float CurrentHealth { get { return health; } internal set { health = value; } }
        public abstract float ReviveHealth { get; internal set; }
        public abstract float MaxHealth { get; internal set; }
        public abstract float DefaultHealth { get; internal set; }

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

        public virtual bool Damage(float damage)
        {
            damage = (float)Mathf.Clamp(damage, 0, Mathf.Infinity);
            return AlterHealth(-damage);
        }

        public virtual bool Heal(float healing)
        {
            healing = (float)Mathf.Clamp(healing, 0, Mathf.Infinity);
            return AlterHealth(healing);
        }

        public virtual bool AlterHealth(float healthDelta)
        {
            if (!Alive)
                return false;

            CurrentHealth += healthDelta;

            if (healthDelta > 0)
                OnHealed?.Invoke();
            if (healthDelta < 0)
                OnDamaged?.Invoke();

            if (CurrentHealth < 0)
                CurrentHealth = 0;
            if (CurrentHealth > MaxHealth)
                CurrentHealth = MaxHealth;

            if (!Alive)
            {
                OnDeath();
                GetComponent<Core.ActionScheduler>().CancelCurrentAction();
                return true;
            }

            return false;
        }

        public virtual event System.Action OnDamaged;

        public virtual event System.Action OnHealed;

        public virtual event System.Action OnDeath;

        public virtual event System.Action OnRevive;
    }
}