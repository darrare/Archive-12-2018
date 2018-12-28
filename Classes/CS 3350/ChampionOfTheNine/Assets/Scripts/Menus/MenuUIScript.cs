using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls menu UI
/// </summary>
public class MenuUIScript : MonoBehaviour
{
    #region Fields

    string surveyLink = "https://docs.google.com/forms/d/1hu5R5tAJW5bNKXLF-g_WYmtkdFDxG7vqoQs3SRqTBqY/viewform";
    [SerializeField]GameObject saveMenu;
    [SerializeField]GameObject loadMenu;
    protected AudioSource audioSource;

    #endregion

    #region Public Methods

    /// <summary>
    /// Handles the play button being pressed
    /// </summary>
    public void PlayButtonPressed()
    {
        audioSource.PlayOneShot(GameManager.Instance.GameSounds[Constants.CLICK_SND]);
        saveMenu.SetActive(true);
    }

    /// <summary>
    /// Handles the load button being pressed
    /// </summary>
    public void LoadButtonPressed()
    {
        audioSource.PlayOneShot(GameManager.Instance.GameSounds[Constants.CLICK_SND]);
        loadMenu.SetActive(true);
    }

    /// <summary>
    /// Handles the quit button being pressed
    /// </summary>
    public void QuitButtonPressed()
    {
        audioSource.PlayOneShot(GameManager.Instance.GameSounds[Constants.CLICK_SND]);
        Application.Quit();
    }

    /// <summary>
    /// Handles the back button being pressed
    /// </summary>
    public void BackButtonPressed()
    {
        GameManager.Instance.LoadLevel(Constants.MAIN_MENU_SCENE, audioSource);
    }

    /// <summary>
    /// Handles the tutorial button being pressed
    /// </summary>
    public void TutorialButtonPressed()
    {
        GameManager.Instance.CurrentSaveName = Constants.TUT_SAVE;
        GameManager.Instance.AddSave(CharacterType.Ranger);
        GameManager.Instance.LoadLevel(Constants.TUTORIAL_SCENE, audioSource);
    }

    /// <summary>
    /// Handles the credits button being pressed
    /// </summary>
    public void CreditsButtonPressed()
    {
        GameManager.Instance.LoadLevel(Constants.CREDITS_SCENE, audioSource);
    }

    /// <summary>
    /// Handles the survey button being pressed
    /// </summary>
    public void SurveyButtonPressed()
    {
        Application.OpenURL(surveyLink);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected virtual void Start()
    {
        GameManager.Instance.Paused = false;
        audioSource = GetComponent<AudioSource>();
    }

    #endregion
}
