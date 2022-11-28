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
        [SerializeField] int[] m_TargetZ;
        [SerializeField] int m_IndexTargetZ;
        [SerializeField] float m_Speed = 20.0f;

        [SerializeField] bool m_IsShooting_L;
        [SerializeField] bool m_IsShooting_R;

        [SerializeField] GameObject m_Bomb;


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
                if (m_IndexTargetZ >= m_TargetZ.Length)
                {
                    m_IsFollowingPlayer = true;
                    StopShooting();
                }
                else if (m_HeadAimTarget.transform.localPosition.z <= m_TargetZ[m_IndexTargetZ])
                {
                    //Debug.Log(">=");
                    m_HeadAimTarget.transform.Translate(Vector3.forward * Time.deltaTime * m_Speed);

                    if(m_HeadAimTarget.transform.localPosition.z >= m_TargetZ[m_IndexTargetZ])
                        m_IndexTargetZ++;
                }
                else if (m_HeadAimTarget.transform.localPosition.z >= m_TargetZ[m_IndexTargetZ])
                {
                    //Debug.Log("<=");
                    m_HeadAimTarget.transform.Translate(Vector3.back * Time.deltaTime * m_Speed);

                    if (m_HeadAimTarget.transform.localPosition.z <= m_TargetZ[m_IndexTargetZ])
                        m_IndexTargetZ++;
                }
            }
        }

        public void FollowPlayer(bool isFollowing)
        {
            m_IsFollowingPlayer = isFollowing;
        }
        public void EnableHeadAim()
        {
            IsAimTargetEnable = true;
            StartCoroutine(SetWeightHeadAim());
        }
        IEnumerator SetWeightHeadAim()
        {
            var constraint = m_HeadAim.GetComponent<MultiAimConstraint>();
            var sourceObjects = constraint.data.sourceObjects;
            var weight = 0.0f;

            while(weight < 1.0f)
            {
                weight += 0.1f;
                sourceObjects.SetWeight(0, weight);
                yield return new WaitForSeconds(0.1f);
            }

            constraint.data.sourceObjects = sourceObjects;
        }

        public void ShootPlayer()
        {
            Debug.Log("ShootPlayer()");
            m_IsFollowingPlayer = true;
            StartCoroutine(StartShootingAfterDelay(1.0f));
        }

        public void ShootAll()
        {
            Debug.Log("ShootAll()");
            int[] positionsX = new int[] { 3, -3, 2, -2 };
            var positionX = positionsX[Random.Range(0, positionsX.Length)];
            m_IndexTargetZ = 0;
            m_TargetZ = new [] { positionX, -positionX, 0 };
            m_HeadAimTarget.transform.localPosition = new Vector3(-2, 0, m_HeadAimTarget.transform.localPosition.z);
            
            m_IsFollowingPlayer = false;
            StartCoroutine(StartShootingAfterDelay(0.5f));
        }

        public void ShootTheSky()
        {
            m_IsFollowingPlayer = false;
            StartCoroutine(StartShootingSkyAfterDelay(1.5f));
        }

        IEnumerator StartShootingAfterDelay(float delay)
        {
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

            yield return new WaitForSeconds(1.0f);
            m_IsShooting_L = false;

            var nbBomb = Random.Range(1, 5);
            for(var i= 0; i < nbBomb; i++)
            {
                Vector3 spawnPos = new Vector3(Random.Range(-25, 26), 10, Random.Range(15, 31));
                GameObject bomb = Instantiate(m_Bomb, spawnPos, m_Bomb.transform.rotation);
                bomb.GetComponent<Bomb>().ExplosionDelay = Random.Range(2.5f, 5.5f);
                bomb.GetComponent<Bomb>().Armed();
            }
        }

        public void StopShooting()
        {
            Debug.Log("StopShooting()");
            m_ArmatureAnimation.SetBool("isShooting", false);
            m_IsShooting_L = false;
            m_IsShooting_R = false;
            //StartCoroutine(StopShootingWithDelay(1.0f));
        }
        //IEnumerator StopShootingWithDelay(float delayAnimation)
        //{
        //    yield return new WaitForSeconds(delayAnimation);
        //    m_IsShooting_L = false;
        //    m_IsShooting_R = false;
        //    m_ArmatureAnimation.SetBool("isShooting", false);
        //}

        public void UpdateHealth()
        {
            if (m_Health > 0)
            {
                RectTransform panelHealth = m_PanelHealth.transform.Find("remaining").GetComponent<RectTransform>();
                panelHealth.sizeDelta = new Vector2(panelHealth.sizeDelta.x, m_Health * 300 / m_StartHealth);
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
