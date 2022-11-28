using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroStory
{

    public class HeroController : MonoBehaviour
    {
        protected static HeroController s_Instance;
        public static HeroController Instance { get { return s_Instance; } }
        
        private Rigidbody m_Rigidbody;
        private Shoot m_Shoot;

        // Move
        public bool IsInputBlocked;
        [SerializeField] float m_Speed = 5f;
        private float m_SpeedInit;
        private float m_SpeedFire = 5f;
        [SerializeField] float m_RotationSmoothTime = 0.1f;
        [SerializeField] float m_RotationSmoothVelocity;
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
        [SerializeField] GameObject m_PanelFireBalls;
        [SerializeField] GameObject m_PanelHealth;
        [SerializeField] Animator m_AlertHealthDecrease;

        private float m_FireBallsMax = 300;
        private float m_FireBalls;
        public float FireBalls
        {
            get { return m_FireBalls; }
            set
            {
                if (value > m_FireBallsMax)
                {
                    m_FireBalls = m_FireBallsMax;
                    //Debug.LogError("Fireballs - Value max!");
                }
                else if (value < 0)
                {
                    m_FireBalls = 0;
                    //Debug.LogError("Fireballs - Value min!");
                }
                else
                {
                    m_FireBalls = value;
                }
                UpdateFireBalls();
            }
        }

        private float m_HealthMax = 300;
        private float m_HealthBeforeUpdate;
        private float m_Health = 5;
        public float Health
        {
            get { return m_Health; }
            set
            {
                if (!IsInputBlocked) // Avoid damage during level 7 when input blocked
                {
                    m_HealthBeforeUpdate = m_Health;
                    if (value > m_HealthMax)
                    {
                        m_Health = m_HealthMax;
                        //Debug.LogError("Health - Value max!");
                    }
                    else if (value < 0)
                    {
                        m_Health = 0;
                        //Debug.LogError("Health - Value min!");
                    }
                    else
                    {
                        m_Health = value;
                    }
                    UpdateHealth();
                }
                else
                    m_Health = m_HealthBeforeUpdate;
            }
        }


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
            m_SpeedInit = m_Speed;

            m_Shoot = GetComponentInChildren<Shoot>();
            m_PanelFireBalls.SetActive(false);
            m_PanelHealth.SetActive(false);
        }

        void Update()
        {
            if (!IsInputBlocked && (Input.GetButtonDown("Fire2") || Input.GetButtonDown("Jump")) )
            {
                var fightHand = new List<string>() { "fight", "fight", "fight2" };
                m_ArmatureAnimation.SetTrigger(fightHand[Random.Range(0, fightHand.Count)]);

                if (IsActionAvailable && TargetAction != null)
                {
                    TargetAction.GetComponent<CodePoint>()?.PlayAction();
                    TargetAction.GetComponent<PointController>()?.PlayAction();
                    TargetAction.GetComponent<VerinController>()?.PlayAction();
                    TargetAction.GetComponent<LoopController>()?.PlayAction();
                    TargetAction.GetComponent<Caisse>()?.PlayAction();

                    AddFightForce(0.45f);
                }
            }
            if(!IsInputBlocked && !IsFightEnabled && IsShootEnabled && (Input.GetButton("Fire1") || Input.GetButton("Fire3")) )
            {
                m_Speed = m_SpeedFire;
                m_ArmatureAnimation.SetBool("isShooting", true);
                m_Shoot.Fire(1);
            }
            if (Input.GetButtonUp("Fire1") || Input.GetButtonUp("Fire3"))
            {
                m_Speed = m_SpeedInit;
                m_ArmatureAnimation.SetBool("isShooting", false);
            }

            // Si on passe à travers un collider
            //if (transform.position.y < -2)
            //{
            //    GameManager.Instance.Retry();
            //}
        }

        void FixedUpdate()
        {
            if (!IsInputBlocked)
            {
                Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
                Move(direction);
            }
        }

        private void UpdateFireBalls()
        {
            if (m_FireBalls > 0)
            {
                IsShootEnabled = true;
                RectTransform panelFireBall = m_PanelFireBalls.transform.Find("remaining").GetComponent<RectTransform>();
                panelFireBall.sizeDelta = new Vector2(panelFireBall.sizeDelta.x, m_FireBalls);
                m_PanelFireBalls.SetActive(true);
            }
            else
            {
                IsShootEnabled = false;
                m_PanelFireBalls.SetActive(false);
            }
        }
        private void UpdateHealth()
        {
            if(m_HealthBeforeUpdate > m_Health) m_AlertHealthDecrease.SetTrigger("touch");

            if (m_Health > 0)
            {
                RectTransform panenHealth = m_PanelHealth.transform.Find("remaining").GetComponent<RectTransform>();
                panenHealth.sizeDelta = new Vector2(panenHealth.sizeDelta.x, m_Health);
                m_PanelHealth.SetActive(true);
            }
            else
            {
                m_PanelHealth.SetActive(false);
                IsInputBlocked = true;
                m_ArmatureAnimation.SetBool("isShooting", false);
                m_ArmatureAnimation.SetBool("isWalking", false);
                m_ArmatureAnimation.SetBool("isKO", true);
                StartCoroutine(DisplayKOMessage());
            }
        }
        IEnumerator DisplayKOMessage()
        {
            yield return new WaitForSeconds(2.0f);
            GameManager.Instance.OnPlayerKO();
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
            //Debug.Log("MoveToPoint - Start");

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
            //Debug.Log("MoveToPoint - END");
            SetIsWalking(false);
            IsInputBlocked = false;
        }

        private void AddFightForce(float delay)
        {
            StartCoroutine(FightForce(delay));
            
        }
        IEnumerator FightForce(float delay)
        {
            yield return new WaitForSeconds(delay);

            float radius = 5.0F;
            float power = 250.0F;

            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                // TODO si besoin force différente suivant les tags
                if (rb != null && rb.tag != "Player")
                {
                    rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
                }
                    
            }
        }
        
}

}