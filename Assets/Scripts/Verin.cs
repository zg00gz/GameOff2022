using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroStory
{

    public class Verin : MonoBehaviour
    {
        public bool IsInteractable;
        public bool IsPlayer;

        private void OnTriggerEnter(Collider other)
        {
            if (!IsPlayer && other.CompareTag("TheBall"))
            {
                IsInteractable = true;
            }
            else if(IsPlayer && other.CompareTag("Player"))
            {
                IsInteractable = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!IsPlayer && other.CompareTag("TheBall"))
            {
                IsInteractable = false;
            }
            else if (IsPlayer && other.CompareTag("Player"))
            {
                IsInteractable = false;
            }
        }
    }

}
