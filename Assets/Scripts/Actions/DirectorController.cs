using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace HeroStory
{

    public class DirectorController : MonoBehaviour
    {
        [SerializeField] Door m_DoorScript;

        void Start()
        {
            m_PlayableDirector = GetComponent<PlayableDirector>();
            if(m_DoorScript != null) m_DoorScript.DoorOpened += OnDoorOpened;
        }

        private void OnDoorOpened()
        {
            m_PlayableDirector.Play();
        }



        [SerializeField] float m_HeroRotationTarget;
        [SerializeField] Vector3 m_HeroPositionTarget;
        private PlayableDirector m_PlayableDirector;

        private void OnTriggerEnter(Collider other)
        {
            // LevelDone
            if (other.CompareTag("Player") && GameManager.Instance.IsLevelDone)
            {
                GameManager.Instance.NextStep(m_HeroRotationTarget);
                StartCoroutine(PlayDirectorEnd());
            }
        }
        IEnumerator PlayDirectorEnd()
        {   
            Vector3 targetPosition = GameManager.Instance.LevelValues.SpawnPoints[GameManager.Instance.CurrentStep];
            yield return new WaitUntil(() => HeroController.Instance.transform.position == targetPosition );
            m_PlayableDirector.Play();
        }
    }

}
