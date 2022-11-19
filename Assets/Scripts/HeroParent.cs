using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace HeroStory
{

    public class HeroParent : MonoBehaviour
    {
        public GameObject m_Parent;

        private enum ParentPosition
        {
            Idle,
            Fire,
            Back
        }

        private ParentPosition m_Position;

        public void Update()
        {
            if (m_Position != ParentPosition.Idle)
            {
                var constraint = m_Parent.GetComponent<MultiParentConstraint>();
                var sourceObjects = constraint.data.sourceObjects;

                sourceObjects.SetWeight(0, m_Position == ParentPosition.Back ? 1f : 0f);
                sourceObjects.SetWeight(1, m_Position == ParentPosition.Fire ? 1f : 0f);
                constraint.data.sourceObjects = sourceObjects;

                m_Position = ParentPosition.Idle;
            }
        }

        public void Start()
        {
            m_Position = ParentPosition.Back;
        }

        public void Fire()
        {
            m_Position = ParentPosition.Fire;
        }

        public void Back()
        {
            m_Position = ParentPosition.Back;
        }

    }

}
