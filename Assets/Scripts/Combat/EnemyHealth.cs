using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;

namespace RPG.Combat
{
    [RequireComponent(typeof(CombatTarget))]
    public class EnemyHealth : Health
    {
        [SerializeField]
        private float reviveHealth = 50;

        public override float ReviveHealth { get { return reviveHealth; } internal set { reviveHealth = value; } }

        [SerializeField]
        private float maxHealth = 50;

        public override float MaxHealth { get { return maxHealth; } internal set { maxHealth = value; } }

        [SerializeField]
        private float defaultHealth = 50;

        public override float DefaultHealth { get { return defaultHealth; } internal set { defaultHealth = value; } }

        private void Start()
        {
            OnDeath += Death;
        }

        private void Death()
        {
            if (animator != null) animator.SetTrigger("die");
            foreach (Collider col in GetComponents<Collider>())
            {
                col.enabled = false;
            }
            Invoke("Destruct", 30f);
        }

        private void Destruct()
        {
            Destroy(gameObject);
        }
    }
}