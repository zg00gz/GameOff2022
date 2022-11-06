using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroStory
{

    public class HeroController : MonoBehaviour
    {
        //private HeroInput m_Input;
        protected Rigidbody m_Rigidbody; // Ou character controller ???

        public bool IsInputBlocked;
        public float Speed = 5f;
        public float RotationSmoothTime = 0.1f;
        public float RotationSmoothVelocity;

        void Awake() // Start ?
        {
            m_Rigidbody = GetComponent<Rigidbody>();

        }

        void FixedUpdate()
        {
            if (!IsInputBlocked)
            {
                Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
                Move(direction);
            }
        }

        private void Move(Vector3 direction)
        {
            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref RotationSmoothVelocity, RotationSmoothTime);

                m_Rigidbody.MoveRotation(Quaternion.Euler(0f, angle, 0f));
                m_Rigidbody.MovePosition(transform.position + direction * Time.deltaTime * Speed);
            }
        }

        //public void MoveAuto(Vector3 position, float speed)
        //{
        //    float targetAngle = Mathf.Atan2(position.x, position.z) * Mathf.Rad2Deg;
        //    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref RotationSmoothVelocity, RotationSmoothTime);

        //    m_Rigidbody.MoveRotation(Quaternion.Euler(0f, angle, 0f));
        //    //m_Rigidbody.MovePosition(transform.position + position * Time.deltaTime * speed);
        //    transform.position = Vector3.Lerp(transform.position, position, speed);
        //}

    }

}