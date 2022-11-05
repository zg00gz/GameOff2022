using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] int m_NbRequired;
    private Animator m_CheckedAnimation;

    private int m_NbChecked = 0;

    public void ChangeNbChecked(int value)
    {
        m_NbChecked += value;
        if (m_NbChecked >= m_NbRequired)
        {
            Debug.Log("Door opened !");
        }
    }

}
