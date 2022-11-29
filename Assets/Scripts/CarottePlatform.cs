using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarottePlatform : MonoBehaviour
{
    private AudioSource m_AudioSource;
    private Animator m_Animation;
    [SerializeField] AudioClip m_Sound;

    void Start()
    {
        m_Animation = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void SetIsUp(bool value)
    {
        m_Animation.SetBool("isUp", value);
    }

    // Animation event
    private void PlayDownSound()
    {
        if (!m_Animation.GetBool("isUp"))
        {
            m_AudioSource.PlayOneShot(m_Sound);
        }

    }

    // Animation event
    private void PlayUpSound()
    {
        if (m_Animation.GetBool("isUp"))
        {
            m_AudioSource.PlayOneShot(m_Sound);
        }

    }
}
