using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


namespace HeroStory
{

    public class UI_Level : MonoBehaviour
    {
        private Label _Time;

        
        private void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();
            
            _Time = uiDocument.rootVisualElement.Q<Label>("Time");
            //_Time.style.display = DisplayStyle.Flex;
        }

        private void OnDisable()
        {
            
        }

        public void DisplayTimer(string timeToDisplay)
        {
            Debug.Log("UI DisplayTimer");
            _Time.text = timeToDisplay;
            _Time.style.display = DisplayStyle.Flex;
        }
        public void UpdateTimer(string timeToDisplay)
        {
            _Time.text = timeToDisplay;
        }
        public void ElapsedTimeScreen(string time = "")
        {
            Debug.Log("UI ElapsedTimeScreen => " + time);
            if (GameManager.Instance.IsLevelDone)
            {
                // Gain étoile : affichage en jaune avec animation + affichage du temps + best time
            }

            _Time.style.display = DisplayStyle.None;
            //TODO afficher le résultat avec un bouton retry 
        }

    }

}
