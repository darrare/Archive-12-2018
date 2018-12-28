using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mediocracy
{
    public class WinLoseMenuScript : UIElementScript
    {
        public override MenuType Type => MenuType.WinMenu;

        public void ReturnToMenuButton()
        {
            SceneManager.LoadScene("Main");
        }

    }

}