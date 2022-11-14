using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroStory
{

    public class AreaController : MonoBehaviour
    {
        [SerializeField] Door m_DoorScript;

        void Start()
        {
            m_DoorScript.DoorExit += OnDoorExit;
        }

        private void OnDoorExit()
        {
            Destroy(gameObject);
        }
    }

}
