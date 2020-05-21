using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 1;

        private Health target = null;
        private int damage = 0;
        public bool isHoming = false;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        private void Update()
        {
            if (target == null) return;

            if (isHoming) transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, int damage, bool isHoming)
        {
            this.target = target;
            this.damage = damage;
            this.isHoming = isHoming;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target)
                return;

            target.AlterHealth(-damage);
            Destroy(gameObject);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();

            if (targetCapsule == null)
            {
                return target.transform.position + Vector3.up;
            }

            return target.transform.position + Vector3.up * targetCapsule.height / 2f;
        }
    }
}