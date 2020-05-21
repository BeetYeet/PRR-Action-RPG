using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Movement;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Weapon defaultWeapon = null;
        [SerializeField] private Weapon currentWeapon = null;

        public float attackRotationSpeed = 100f;
        private Transform target;
        private Mover mover;

        [SerializeField]
        private List<Faction> attackableAlignments = new List<Faction>() { Faction.Player };

        private float timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            mover = GetComponent<Mover>();
            EquipWeapon(defaultWeapon);
        }

        private void Update()
        {
            Health health = GetComponent<Health>();
            if (health == null || !health.Alive)
                return;
            timeSinceLastAttack += Time.deltaTime;

            if (target == null)
            {
                return;
            }
            if (GetIsInRange())
            {
                mover.Cancel();
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target.position - transform.position, Vector3.up), attackRotationSpeed);
                if (timeSinceLastAttack > currentWeapon.GetTimeBetweenAttacks())
                {
                    AttackBehavoiur();
                    timeSinceLastAttack = 0f;
                }
            }
            else
            {
                mover.MoveTo(target.position, 1f);
            }
        }

        private void SpawnWeapon(Weapon weapon)
        {
            if (weapon == null)
                return;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(leftHandTransform, rightHandTransform, animator);
        }

        public void EquipWeapon(Weapon weapon)
        {
            if (currentWeapon != null)
            {
                currentWeapon.Despawn(leftHandTransform, rightHandTransform, GetComponent<Animator>());
            }

            if (weapon == null)
            {
                currentWeapon = defaultWeapon;
            }
            else
            {
                currentWeapon = weapon;
            }

            SpawnWeapon(currentWeapon);
        }

        private void AttackBehavoiur()
        {
            if (!target.GetComponent<Health>().Alive)
            {
                target = null;
                return;
            }
            Animator anim = GetComponent<Animator>();
            if (anim != null)
                anim.SetTrigger("attack");
            mover.attackIsAnimating = true;
        }

        private void Shoot()
        {
            if (target == null)
            {
                mover.attackIsAnimating = false;
                return;
            }
            Health targetHealth = target.GetComponent<Health>();
            if (targetHealth != null && currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(leftHandTransform, rightHandTransform, targetHealth);
            }
            mover.attackIsAnimating = false;
        }

        private void DealMeleeDamage(Health targetHealth)
        {
            if (currentWeapon.GetDamage() > 0)
            {
                bool died = targetHealth.Damage(currentWeapon.GetDamage());
                if (died)
                {
                    target = null;
                }
            }
            if (currentWeapon.GetDamage() < 0)
            {
                targetHealth.Heal(-currentWeapon.GetDamage());
            }
        }

        private void Hit()
        {
            if (target == null)
                return;
            Health targetHealth = target.GetComponent<Health>();
            if (targetHealth != null)
            {
                DealMeleeDamage(targetHealth);
            }
            mover.attackIsAnimating = false;
        }

        private bool GetIsInRange()
        {
            return currentWeapon.GetRange() > Vector3.Distance(target.position, transform.position); ;
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