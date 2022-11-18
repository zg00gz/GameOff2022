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

        //public float radius = 5.0F;
        //public float power = 10.0F;
        //private void OnTriggerEnter(Collider other)
        //{
        //    if (other.gameObject.CompareTag("Player") && HeroController.Instance.IsFightEnabled)
        //    {
        //            Debug.Log(other.name);
        //            m_Rb.AddExplosionForce(power, other.transform.position, radius, 3.0F);
        //    }
        //}

        //private void OnCollisionStay(Collision collision)
        //{
        //    if(collision.gameObject.CompareTag("Player") && HeroController.Instance.IsFightEnabled)
        //    {
        //        GetComponent<Rigidbody>().AddForce(-collision.GetContact(0).normal);
        //    }
        //}
    }

}