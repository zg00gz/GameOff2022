using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    //private HeroInput m_Input;
    protected Rigidbody m_Rigidbody; // Ou character controller ???

    public float ForwardSpeed = 2f;
    public float RotationSpeed = 10f;
    public float RotationSmoothTime = 0.1f;
    public float RotationSmoothVelocity;

    void Awake() // Start ?
    {
        m_Rigidbody = GetComponent<Rigidbody>();

    }
    
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref RotationSmoothVelocity, RotationSmoothTime);

            m_Rigidbody.MoveRotation(Quaternion.Euler(0f, angle, 0f));
            m_Rigidbody.MovePosition(transform.position + direction * Time.deltaTime * ForwardSpeed);
        }

    }

}
