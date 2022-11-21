using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HeroStory
{

    public class FightController : MonoBehaviour
    {
        [SerializeField] List<AudioClip> punchSounds = new List<AudioClip>();
        [SerializeField] AudioSource source;

        public void PlayPunchSounds()
        {
            AudioClip clip = punchSounds[Random.Range(0, punchSounds.Count)];
            source.PlayOneShot(clip);
        }

    }

}
