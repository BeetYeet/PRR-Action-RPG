using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;

namespace RPG.Combat
{
    [RequireComponent(typeof(CombatTarget))]
    public class PlayerHealth : Health
    {
        [SerializeField]
        private float reviveHealth = 10;

        public override float ReviveHealth { get { return reviveHealth; } internal set { reviveHealth = value; } }

        [SerializeField]
        private float maxHealth = 100;

        public override float MaxHealth { get { return maxHealth; } internal set { maxHealth = value; } }

        [SerializeField]
        private float defaultHealth = 100;

        public override float DefaultHealth { get { return defaultHealth; } internal set { defaultHealth = value; } }

        private void Start()
        {
            OnDeath += () => { Debug.Log("player died"); if (animator != null) animator.SetTrigger("die"); };
        }
    }
}