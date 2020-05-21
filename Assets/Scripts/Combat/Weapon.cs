using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private GameObject prefab = null;
        [SerializeField] private AnimatorOverrideController animatorOverride = null;

        [SerializeField] private bool isRightHanded = true;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private int damage = 10;
        [SerializeField] private Projectile projectile = null;
        [SerializeField] private bool isHoming = false;

        public void Spawn(Transform leftHandTransform, Transform rightHandTransform, Animator animator)
        {
            if (prefab != null)
            {
                if (isRightHanded)
                {
                    if (rightHandTransform != null)
                        Instantiate(prefab, rightHandTransform);
                    else
                        Debug.LogError("Weapon was not passed a transform to attatch to!");
                }
                else
                {
                    if (leftHandTransform != null)
                        Instantiate(prefab, leftHandTransform).name = name;
                    else
                        Debug.LogError("Weapon was not passed a transform to attatch to!");
                }
            }
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }

        public void Despawn(Transform leftHand, Transform rightHand, Animator animator)
        {
            if (leftHand != null)
            {
                RemoveWeapon(leftHand);
            }
            if (rightHand != null)
            {
                RemoveWeapon(rightHand);
            }
            if (animator != null)
            {
                animator.runtimeAnimatorController = null;
            }
        }

        private void RemoveWeapon(Transform hand)
        {
            if (prefab == null) // unarmed
                return;
            Transform obj = hand.Find(name);
            if (obj != null)
                Destroy(obj.gameObject);
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform leftHand, Transform rightHand, Health target)
        {
            Vector3 handPos = (isRightHanded ? rightHand : leftHand).position;
            Projectile projectileInstance =
                Instantiate(projectile,
                            handPos,
                            Quaternion.LookRotation((handPos - target.transform.position).normalized, Vector3.up));
            projectileInstance.SetTarget(target, damage, isHoming);
        }

        public float GetDamage()
        {
            return damage;
        }

        public float GetRange()
        {
            return attackRange;
        }

        public float GetTimeBetweenAttacks()
        {
            return timeBetweenAttacks;
        }
    }
}