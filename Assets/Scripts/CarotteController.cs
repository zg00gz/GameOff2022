using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;


namespace HeroStory

{
    public class CarotteController : MonoBehaviour
    {
        [SerializeField] Animator m_ArmatureAnimation;
        [SerializeField] GameObject m_HeadAim;
        [SerializeField] GameObject m_HeadAimTarget;

        [SerializeField] ShootCarot m_Shoot_L;
        [SerializeField] ShootCarot m_Shoot_R;

        [SerializeField] GameObject m_PanelHealth;
        [SerializeField] float m_Health = 1500;
        private float m_StartHealth;
        public float Health
        {
            get { return m_Health; }
            set
            {
                if (value < 0)
                {
                    m_Health = 0;
                }
                else
                {
                    m_Health = value;
                }
                UpdateHealth();
            }
        }

        [SerializeField] bool m_IsFollowingPlayer;

        public bool IsAimTargetEnable;
        private int[] m_TargetZ;
        private int m_IndexTargetZ;

        private bool m_IsShooting_L;
        private bool m_IsShooting_R;


        void Start()
        {
            m_StartHealth = m_Health;
        }

        void Update()
        {
            if(m_IsShooting_L)
            {
                m_Shoot_L.Fire(1);
            }
            if(m_IsShooting_R)
            {
                m_Shoot_R.Fire(1);
            }
            if(m_IsFollowingPlayer)
            {
                m_HeadAimTarget.transform.position = HeroController.Instance.transform.position;
            }
            else if (IsAimTargetEnable)
            {
                float speed = 15.0f;
                if (m_IndexTargetZ >= m_TargetZ.Length)
                {
                    IsAimTargetEnable = false;
                    StopShooting();
                }
                else if (m_HeadAimTarget.transform.localPosition.z <= m_TargetZ[m_IndexTargetZ])
                {
                    //Debug.Log(">=");
                    m_HeadAimTarget.transform.Translate(Vector3.forward * Time.deltaTime * speed);

                    if(m_HeadAimTarget.transform.localPosition.z >= m_TargetZ[m_IndexTargetZ])
                        m_IndexTargetZ++;
                }
                else if (m_HeadAimTarget.transform.localPosition.z >= m_TargetZ[m_IndexTargetZ])
                {
                    //Debug.Log("<=");
                    m_HeadAimTarget.transform.Translate(Vector3.back * Time.deltaTime * speed);

                    if (m_HeadAimTarget.transform.localPosition.z <= m_TargetZ[m_IndexTargetZ])
                        m_IndexTargetZ++;
                }
        }
        }

        public void FollowPlayer(bool isFollowing)
        {
            EnableHeadAim(isFollowing);
            m_IsFollowingPlayer = isFollowing;
        }

        public void EnableHeadAim(bool enable)
        {
            IsAimTargetEnable = true;
            var constraint = m_HeadAim.GetComponent<MultiAimConstraint>();
            var sourceObjects = constraint.data.sourceObjects;
            sourceObjects.SetWeight(0, enable ? 1f : 0f);

            constraint.data.sourceObjects = sourceObjects;
        }

        public void ShootPlayer()
        {
            Debug.Log("ShootPlayer()");
            m_IsFollowingPlayer = true;
            IsAimTargetEnable = false;
            StartCoroutine(StartShootingAfterDelay(1.0f));
        }

        public void ShootAll()
        {
            int[] positionsX = new int[] { 3, -3, 2, -2 };
            var positionX = positionsX[Random.Range(0, positionsX.Length)];
            m_IndexTargetZ = 0;
            m_TargetZ = new [] { positionX, -positionX, 0 };
            m_HeadAimTarget.transform.localPosition = new Vector3(-2, 0, m_HeadAimTarget.transform.localPosition.z);

           IsAimTargetEnable = true;
            m_IsFollowingPlayer = false;
            m_IsShooting_L = true;
            m_IsShooting_R = true;

            StartCoroutine(StartShootingAfterDelay(1.5f));
        }

        public void ShootTheSky()
        {
            m_IsFollowingPlayer = false;
            StartCoroutine(StartShootingSkyAfterDelay(1.0f));
        }

        IEnumerator StartShootingAfterDelay(float delay)
        {
            Debug.Log("StartShootingAfterDelay()");

            m_ArmatureAnimation.SetBool("isShooting", true);
            yield return new WaitForSeconds(delay);
            m_IsShooting_L = true;
            m_IsShooting_R = true;
        }

        IEnumerator StartShootingSkyAfterDelay(float delay)
        {
            m_ArmatureAnimation.SetBool("isShooting", false);
            m_ArmatureAnimation.SetBool("shootSky", true);
            yield return new WaitForSeconds(delay);
            m_IsShooting_L = true;
            m_IsShooting_R = false;
        }

        public void StopShooting()
        {
            Debug.Log("StopShooting()");

            StartCoroutine(StopShootingWithDelay(1.0f));
        }
        IEnumerator StopShootingWithDelay(float delayAnimation)
        {
            Debug.Log("StopShootingWithDelay()");

            m_IsShooting_L = false;
            m_IsShooting_R = false;
            yield return new WaitForSeconds(delayAnimation);
            m_ArmatureAnimation.SetBool("isShooting", false);
        }

        public void UpdateHealth()
        {
            if (m_Health > 0)
            {
                RectTransform panelHealth = m_PanelHealth.transform.Find("remaining").GetComponent<RectTransform>();
                panelHealth.sizeDelta = new Vector2(panelHealth.sizeDelta.x, m_Health * 400 / m_StartHealth);
                m_PanelHealth.SetActive(true);
            }
            else
            {
                m_PanelHealth.SetActive(false);
                // TODO The end !
            }
        }

    }

}
