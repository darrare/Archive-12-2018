using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that handles the character creation screen
/// </summary>
public class CharacterCreationScript : ControlSelectScript
{
    #region Fields

    static CharacterCreationScript instance;

    [SerializeField]GameObject selectButton;
    CharacterType selectedType;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the static instance of the script
    /// </summary>
    public static CharacterCreationScript Instance
    { get { return instance; } }

    #endregion

    #region Public Methods

    /// <summary>
    /// Sets the chosen character type
    /// </summary>
    /// <param name="type">the type</param>
    public void SetCharacterType(CharacterType type)
    {
        audioSource.PlayOneShot(GameManager.Instance.GameSounds[Constants.CLICK_SND]);
        previewObjects[selectedType].SetActive(false);
        previewObjects[type].SetActive(true);
        selectedType = type;
        selectButton.SetActive(true);
        title.text = classNames[type];
    }

    /// <summary>
    /// Handles the confirm selection button being pressed
    /// </summary>
    public void SelectButtonPressed()
    {
        GameManager.Instance.CurrentSave.PlayerType = selectedType;
        GameManager.Instance.Save();
        GameManager.Instance.LoadLevel(Constants.MAP_SCENE, audioSource);
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected override void Start()
    {
        instance = this;
        base.Start();
    }

    #endregion
}
