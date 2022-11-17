using UnityEngine;
using UnityEngine.Playables;

namespace HeroStory
{

    public class DirectorController : MonoBehaviour
    {
        [SerializeField] Door m_DoorScript;
        private PlayableDirector m_PlayableDirector;

        void Start()
        {
            m_PlayableDirector = GetComponent<PlayableDirector>();
            if(m_DoorScript != null) m_DoorScript.DoorOpened += OnDoorOpened;
        }

        private void OnDoorOpened()
        {
            //Debug.Log(m_PlayableDirector.name);
            m_PlayableDirector.Play();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                m_PlayableDirector.Play();
            }
        }
    }

}
