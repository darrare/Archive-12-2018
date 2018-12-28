using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mediocracy
{
    public class MainMenuScript : UIElementScript
    {
        public override MenuType Type => MenuType.MainMenu;

        public GameObject optionsPanel;
        public GameObject creditsPanel;

        public void Start()
        {
            optionsPanel.SetActive(false);
            creditsPanel.SetActive(false);
        }

        public void PlayButton()
        {
            SceneManager.LoadScene("Level");
        }

        public void QuitButton()
        {
            Application.Quit();
        }

        public void OptionsButton()
        {
            optionsPanel.SetActive(true);
        }

        public void CreditsButton()
        {
            creditsPanel.SetActive(true);
        }

        public void CloseButton()
        {
            optionsPanel.SetActive(false);
            creditsPanel.SetActive(false);
            CloseUIElement();
        }

    }

}