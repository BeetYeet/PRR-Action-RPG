using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;

namespace RPG.Core
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField]
        private Weapon weapon;

        public Weapon Weapon { get { return weapon; } } // In case we want to make it so you cant pick up a weapon you are alredy holding

        [SerializeField]
        private List<GameObject> destroyOnPickup;

        [SerializeField]
        private ParticleSystem pickupParticles;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);

                if (pickupParticles != null)
                    pickupParticles.Play();
                StartCoroutine(Destroy());
            }
        }

        private IEnumerator Destroy()
        {
            GetComponent<Collider>().enabled = false;
            destroyOnPickup.ForEach(x => Destroy(x));
            yield return new WaitForSeconds(10f);
            Destroy(gameObject);
        }
    }
}