using UnityEngine;


namespace HeroStory
{

    public class GameCanvas : MonoBehaviour
    {
        [SerializeField] AudioSource m_Go;

        private void PlayGo()
        {
            m_Go.Play();
        }
    }

}
