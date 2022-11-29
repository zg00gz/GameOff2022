using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroStory
{


    public class Goose : MonoBehaviour
    {
        [SerializeField] bool m_IsAutoRunning;
        [SerializeField] bool m_IsRunning;
        [SerializeField] bool m_IsFollowingPlayer;
        [SerializeField] GameObject m_Player;

        private float m_SpeedRun = 10.0f;
        private float m_SpeedWalk = 2.0f;

        [SerializeField] Animator m_Animator;

        private Rigidbody m_Rigidbody;
        private AudioSource m_AudioSource;

        [SerializeField] int m_TouchedMax = 10;
        [SerializeField] int m_PlayerDamage = 5;
        
        private int m_Touched;
        private bool m_IsDestroyed;

        [SerializeField] Vector3 direction = Vector3.right;
        

        void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_AudioSource = GetComponent<AudioSource>();

            if (m_IsAutoRunning)
            {
                m_Animator.SetTrigger("idle");
                StartCoroutine(RunForward());
            }
        }

        private void Update()
        {
            if (m_Touched > m_TouchedMax)
            {
                m_Rigidbody.MovePosition(transform.position + Vector3.up * Time.deltaTime * 70.0f);
                if (!m_IsDestroyed) PlayDestroy();
            }
            else
            {
                if (m_IsFollowingPlayer)
                {
                    transform.LookAt(m_Player.transform);

                    var followPos = new Vector3(m_Player.transform.position.x, transform.position.y, m_Player.transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, followPos, m_SpeedWalk * Time.deltaTime);
                }
                else if (m_IsRunning)
                {

                    if (transform.position.x < -25)
                    {
                        direction = Vector3.right;
                        transform.localRotation = Quaternion.Euler(0, -90, 0);
                    }
                    else if (transform.position.x > 25)
                    {
                        direction = Vector3.left;
                        transform.localRotation = Quaternion.Euler(0, 90, 0);
                    }

                    m_Rigidbody.MovePosition(transform.position + direction * Time.deltaTime * m_SpeedRun);
                }
            }
            
        }


        public void WalkToPlayer()
        {
            m_Animator.SetTrigger("idle");
            m_IsFollowingPlayer = true;
            m_Animator.SetTrigger("walk");
        }

        IEnumerator RunForward()
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 5.0f));
            m_IsRunning = true;
            m_Animator.SetBool("isRunning", true);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.tag != "Floor")
            {
                m_Touched++;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player") HeroController.Instance.Health -= m_PlayerDamage;
        }

        private void PlayDestroy()
        {
            m_IsDestroyed = true;
            m_AudioSource.Play();
            Destroy(gameObject, 2.0f);
        }

        public void Touched()
        {
            m_Touched++;
        }
    }


}
