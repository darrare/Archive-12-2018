using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField]
    InputField usernameInputField;

    [SerializeField]
    Text errorText;

    /// <summary>
    /// Called whenever the log in button is clicked
    /// </summary>
    public void LoginButtonClick()
    {
        if (usernameInputField.text != "")
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            errorText.text = "Please enter a valid username.";
        }
    }
}
