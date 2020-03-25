using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        public int portalToScene = 0;
        public Transform spawnpoint;
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Portal Hit Something");
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }


        IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(portalToScene);
            Debug.Log("Scene Loaded");

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            Destroy(gameObject);
        }

        void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            Camera.main.transform.position += otherPortal.spawnpoint.position - player.transform.position;
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnpoint.position);
            player.transform.rotation = otherPortal.transform.rotation;
        }


        Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this)
                    continue;
                return portal;
            }
            return null;
        }
    }
}