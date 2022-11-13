using UnityEngine;

namespace HeroStory
{

    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] Door m_DoorScript;
        [SerializeField] Material m_InitialMaterial;
        [SerializeField] Material m_CheckedMaterial;
        
        private bool m_IsChecked = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !m_IsChecked)
            {
                m_IsChecked = true;
                m_DoorScript.ChangeNbChecked(1);
                GetComponent<MeshRenderer>().material = m_CheckedMaterial;
            }
            else if(other.CompareTag("Player") && m_IsChecked && !m_DoorScript.IsChecked)
            {
                m_IsChecked = false;
                m_DoorScript.ChangeNbChecked(-1);
                GetComponent<MeshRenderer>().material = m_InitialMaterial;
            }
        }
    }

}
