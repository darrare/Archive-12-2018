using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mediocracy
{
    public class PauseMenuScript : UIElementScript
    {
        public override MenuType Type => MenuType.PauseMenu;

        public GameObject pausePanel;

        public void Start()
        {
            pausePanel.SetActive(false);
        }

        public void PauseButton()
        {
            pausePanel.SetActive(true);
        }

        public void ReturnToMenuButton()
        {
            SceneManager.LoadScene("Main");
        }

        public void CloseButton()
        {
            pausePanel.SetActive(false);
            CloseUIElement();
        }

    }

}