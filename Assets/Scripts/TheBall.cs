using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HeroStory
{

    public class TheBall : MonoBehaviour
    {
        [SerializeField] Door m_DoorScript;
        private Rigidbody m_Rb;

        void Start()
        {
            m_Rb = GetComponent<Rigidbody>();
            m_DoorScript.DoorExit += OnDoorExit;
        }

        private void OnDoorExit()
        {
            m_Rb.constraints = RigidbodyConstraints.None;
        }

    }

}