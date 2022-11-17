using UnityEngine;

namespace HeroStory
{

    public class HeroAnimations : MonoBehaviour
    {
        private void SetIsFighting()
        {
            HeroController.Instance.IsFightEnabled = true;
        }
        private void SetIsNotFighting()
        {
            HeroController.Instance.IsFightEnabled = false;
        }
    }

}
