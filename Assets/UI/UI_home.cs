using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


namespace HeroStory
{

    public class UI_home : MonoBehaviour
    {
        private Button _Level1_1;
        private Button _Level1_2;
        private Button _Level1_3;

        private void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();


            _Level1_1 = uiDocument.rootVisualElement.Q<Button>("Level1-1");
            _Level1_2 = uiDocument.rootVisualElement.Q<Button>("Level1-2");
            _Level1_3 = uiDocument.rootVisualElement.Q<Button>("Level1-3");

            _Level1_1.RegisterCallback<ClickEvent>(LoadScene1);
            _Level1_2.RegisterCallback<ClickEvent>(LoadScene2);
            _Level1_3.RegisterCallback<ClickEvent>(LoadScene3);
        }

        private void OnDisable()
        {
            //_Level1_1.UnregisterCallback<ClickEvent>(LoadScene("Hero-PreProto"));
        }

        private void LoadScene1(ClickEvent evt)
        {
            SceneManager.LoadScene("Hero-PreProto");
        }

        private void LoadScene2(ClickEvent evt)
        {
            SceneManager.LoadScene("Hero-PreProto2");
        }

        private void LoadScene3(ClickEvent evt)
        {
            SceneManager.LoadScene("Hero-PreProto3");
        }

    }

}
