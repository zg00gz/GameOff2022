using System.Collections;
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
        [SerializeField] Animator m_ArmatureAnimation;
        // Move auto
        [SerializeField] float m_AutoLerpSpeed = 4f;
        [SerializeField] float m_AutoRotateSpeed = 50f;

        // Action
        public bool IsActionAvailable;
        public Transform TargetAction;
        public bool IsShootEnabled;
        public bool IsFightEnabled;
        public bool IsWalking;
        

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
            //if(IsActionAvailable && Input.GetButtonDown("Jump") && !IsInputBlocked) 
            // IsActionAvailable à utiliser pour les levier et animation spécifique ?
            // TODO action Fire2 clique droit + Bouton X ?
            if (!IsInputBlocked && Input.GetButtonDown("Jump"))
            {
                m_ArmatureAnimation.SetTrigger("fight");

                if (IsActionAvailable && TargetAction != null)
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
            if (direction.magnitude >= 0.1f && !IsFightEnabled)
            {
                if (!IsWalking) SetIsWalking(true);
                
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref m_RotationSmoothVelocity, m_RotationSmoothTime);
                m_Rigidbody.MoveRotation(Quaternion.Euler(0f, angle, 0f));
                m_Rigidbody.MovePosition(transform.position + direction * Time.deltaTime * m_Speed);
            }
            else
            {
                if (IsWalking) SetIsWalking(false);
            }
        }
        public void SetIsWalking(bool isWalking)
        {
            IsWalking = isWalking;
            m_ArmatureAnimation.SetBool("isWalking", isWalking);
        }

        public void MoveAuto(Vector3 targetPosition, float targetRotation)
        {
            StartCoroutine(MoveToPoint(targetPosition, targetRotation));
        }
        IEnumerator MoveToPoint(Vector3 targetPosition, float targetRotation)
        {
            IsInputBlocked = true;
            SetIsWalking(true);
            Debug.Log("MoveToPoint - Start");

            Vector3 startPos = HeroController.Instance.transform.position;
            Vector3 endPos = targetPosition;

            float journeyLength = Vector3.Distance(startPos, endPos);
            float startTime = Time.time;
            float distanceCovered = (Time.time - startTime) * m_AutoLerpSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            while (fractionOfJourney < 1)
            {
                // Position
                distanceCovered = (Time.time - startTime) * m_AutoLerpSpeed;
                fractionOfJourney = distanceCovered / journeyLength;
                transform.position = Vector3.Lerp(startPos, endPos, fractionOfJourney);

                // Rotation
                var rotationToward = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, targetRotation, 0)), m_AutoRotateSpeed * Time.deltaTime);
                transform.rotation = rotationToward;

                yield return null;
            }
            Debug.Log("MoveToPoint - END");
            SetIsWalking(false);
            IsInputBlocked = false;
        }
        /*
        public void RotateAuto(Vector3 targetPosition)
        {
            StartCoroutine(RotateToPoint(targetPosition));
        }
        IEnumerator RotateToPoint(Vector3 targetPosition)
        {
            IsInputBlocked = true;

            Vector3 startPos = HeroController.Instance.transform.position;
            Vector3 endPos = targetPosition;
            float targetAngle = Mathf.Atan2(startPos.x, endPos.z) * Mathf.Rad2Deg - 180;

            while (transform.rotation != Quaternion.Euler(new Vector3(0, targetAngle, 0)))
            {
                var rotationToward = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, targetAngle, 0)), m_AutoRotateSpeed * Time.deltaTime);
                transform.rotation = rotationToward;

                yield return null;
            }

            IsInputBlocked = false;
        }
        */
    }

}