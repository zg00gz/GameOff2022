using System.Collections;
using UnityEngine;

namespace HeroStory
{

    public class Caisse : MonoBehaviour
    {

        public void PlayAction()
        {
            StartCoroutine(PlayExplode());
        }

        IEnumerator PlayExplode()
        {
            HeroController.Instance.IsActionAvailable = false;

            yield return new WaitForSeconds(0.45f); // Fight animation
            HeroController.Instance.GetComponent<FightController>().PlayPunchSounds();

            HeroController.Instance.IsActionAvailable = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                HeroController.Instance.TargetAction = transform;
                HeroController.Instance.IsActionAvailable = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                HeroController.Instance.TargetAction = null;
                HeroController.Instance.IsActionAvailable = false;
            }
        }

    }

}
