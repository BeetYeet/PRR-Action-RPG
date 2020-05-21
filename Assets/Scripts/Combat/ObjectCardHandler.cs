using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class ObjectCardHandler : MonoBehaviour
    {
        public Health health;
        public HealthbarHandler healthbar;

        private void Start()
        {
            health.OnDamaged += OnChangeHealth;
            health.OnHealed += OnChangeHealth;
            health.OnDeath += healthbar.Destroy;
            OnChangeHealth();
            healthbar.ResetValues();
        }

        private void OnChangeHealth()
        {
            healthbar.UpdateHealth(health.health / (float)health.MaxHealth);
        }
    }
}