using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        public enum PortalIdentifier { A, B, C, D, E, F }

        public int portalToScene = 0;
        public PortalIdentifier portalToIdentifier = PortalIdentifier.A;
        public Transform spawnpoint;
        public PortalIdentifier identifier = PortalIdentifier.A;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Portal Hit Something");
            if (other.tag == "Player")
            {
                TransitionSlider.instance.TransitionOut();
                TransitionSlider.instance.OnTransitionedOut += OnTransitionedOut;
            }
        }

        private void OnTransitionedOut()
        {
            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(portalToScene);
            Debug.Log("Scene Loaded");

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            Camera.main.transform.position += otherPortal.spawnpoint.position - player.transform.position;
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnpoint.position);
            player.transform.rotation = otherPortal.transform.rotation;
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this)
                    continue;
                if (portal.identifier == portalToIdentifier)
                    return portal;
            }
            return null;
        }
    }
}