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

            if (m_DoorScript)
                m_DoorScript.DoorExit += OnDoorExit;
            else
                SetNoConstraints();
        }

        private void OnDoorExit()
        {
            SetNoConstraints();
        }

        private void SetNoConstraints()
        {
            m_Rb.constraints = RigidbodyConstraints.None;

        }
    }

}