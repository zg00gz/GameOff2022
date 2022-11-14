using UnityEngine;

namespace HeroStory
{

    public class HeroController : MonoBehaviour
    {
        protected static HeroController s_Instance;
        public static HeroController Instance { get { return s_Instance; } }
        
        private Rigidbody m_Rigidbody;

        // Move
        public bool IsInputBlocked;
        [SerializeField] float m_Speed = 5f;
        [SerializeField] float m_RotationSmoothTime = 0.1f;
        [SerializeField] float m_RotationSmoothVelocity;
        [SerializeField] ParticleSystem m_Shoot;

        // Action
        public bool IsActionAvailable;
        public Transform TargetAction;
        public bool IsShootEnabled;

        void Awake()
        {
            if (s_Instance != null)
            {
                Debug.Log("HeroController - gameobject destroyed");
                Destroy(gameObject);
                return;
            }
            s_Instance = this;

            m_Rigidbody = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if(IsActionAvailable && Input.GetButtonDown("Jump") && !IsInputBlocked) // TODO action Fire2 clique droit + Bouton X ?
            {
                if(TargetAction != null)
                {
                    TargetAction.GetComponent<CodePoint>()?.PlayAction();
                    TargetAction.GetComponent<PointController>()?.PlayAction();
                }
            }

            if(!IsInputBlocked && IsShootEnabled && Input.GetButtonDown("Fire1")) // TODO Tire Fire1 clique gauche + Bouton A ?
            {
                m_Shoot.Play();
            }
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
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref m_RotationSmoothVelocity, m_RotationSmoothTime);
                m_Rigidbody.MoveRotation(Quaternion.Euler(0f, angle, 0f));
                m_Rigidbody.MovePosition(transform.position + direction * Time.deltaTime * m_Speed);
            }
        }

    }

}