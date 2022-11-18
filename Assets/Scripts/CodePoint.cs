using System.Collections;
using UnityEngine;

namespace HeroStory
{

    public class CodePoint : MonoBehaviour
    {
        [SerializeField] Door m_DoorScript;
        [SerializeField] float m_RotationChecked;
        [SerializeField] bool m_IsChecked;

        public void PlayAction()
        {
           if(!m_DoorScript.IsOpened) StartCoroutine(PlayRotation());
        }

        IEnumerator PlayRotation()
        {
            HeroController.Instance.IsActionAvailable = false;

            float lerpDuration = 0.2f;
            float timeElapsed = 0;
            Quaternion startRotation = transform.rotation;
            Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 90, 0);

            yield return new WaitForSeconds(0.45f); // Fight animation

            while (timeElapsed < lerpDuration)
            {
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            transform.rotation = targetRotation;

            Debug.Log("PlayRotation - " + transform.eulerAngles.y + " - " + m_RotationChecked + " - " + m_RotationChecked);
            if (m_IsChecked && Mathf.RoundToInt(transform.eulerAngles.y) != m_RotationChecked) // Values inspector : 0, 90, ,-180, -90 => reals eulerAngles values to set => 0, 90, 180, 270
            {
                Debug.Log("Unchecked");
                m_IsChecked = false;
                m_DoorScript.ChangeNbChecked(-1);
            }
            else if (!m_IsChecked && Mathf.RoundToInt(transform.eulerAngles.y) == m_RotationChecked)
            {
                Debug.Log("Checked");
                m_IsChecked = true;
                m_DoorScript.ChangeNbChecked(1);
            }

            HeroController.Instance.IsActionAvailable = true;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                HeroController.Instance.TargetAction = transform;
                HeroController.Instance.IsActionAvailable = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                HeroController.Instance.TargetAction = null;
                HeroController.Instance.IsActionAvailable = false;
            }
        }
    }

}
