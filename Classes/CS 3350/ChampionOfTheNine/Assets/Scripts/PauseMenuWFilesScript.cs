using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls a pause menu with a files display (save/load)
/// </summary>
[RequireComponent(typeof(ToggleGroup))]
public class PauseMenuWFilesScript : MonoBehaviour
{
    #region Fields

    public RectTransform scrollFieldContent;       // The scroll field for the file display
    public Toggle levelFilePrefab;  // The prefab for a level file display option
    public GameObject confButton;   // The button to confirm (save/load)

    [SerializeField]float levelFileTopOffset;   // The Y offset of the top level file display
    [SerializeField]float levelFileLeftOffset;  // The left offset of the level file displays
    [SerializeField]float levelFileRightOffset; // The right offset of the level file displays
    [SerializeField]float levelFileSpacing;     // The Y spacing between the level file displays
    [SerializeField]int levelOptionsPerScreen;  // The number of level options that can fit on the screen at once
    [SerializeField]protected AudioSource audioSource;
    protected AudioClip clickSound;

    ToggleGroup toggleGroup;        // The toggle group for the file display
    List<Toggle> levelFileOptions;  // The list of level file option toggles

    #endregion

    #region Public Methods

    /// <summary>
    /// Handles the cancel button being pressed
    /// </summary>
    public void CancelButtonPressed()
    {
        // Hides the menu
        audioSource.PlayOneShot(clickSound);
        gameObject.SetActive(false);
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Initializes the menu
    /// </summary>
    protected virtual void Initialize()
    {
        // Initializes fields
        levelFileOptions = new List<Toggle>();
        toggleGroup = GetComponent<ToggleGroup>();
        clickSound = Resources.Load<AudioClip>(Constants.SND_FOLDER + Constants.CLICK_SND);
    }

    /// <summary>
    /// Updates the menu on enable
    /// </summary>
    protected virtual void OnEnable()
    {
        // Checks for initialize
        if (levelFileOptions == null)
        { Initialize(); }
        GameManager.Instance.CurrentSaveName = "";

        // Creates the level file options
        foreach (KeyValuePair<string, Savegame> level in GameManager.Instance.Saves)
        {
            if (level.Key != Constants.TUT_SAVE)
            {
                // Creates a new toggle
                Toggle newToggle = Instantiate(levelFilePrefab);

                // Sets the toggle's data
                RectTransform newToggleRect = newToggle.GetComponent<RectTransform>();
                newToggleRect.SetParent(scrollFieldContent, false);
                newToggle.GetComponentInChildren<Text>().text = level.Key;
                newToggle.group = toggleGroup;
                newToggle.onValueChanged.AddListener(LevelFileValueChanged);

                // Positions the toggle
                newToggleRect.anchoredPosition = new Vector2(5, levelFileSpacing * levelFileOptions.Count);

                // Adds the new toggle to the list
                levelFileOptions.Add(newToggle);
            }
        }

        // Updates scroll field height
        scrollFieldContent.sizeDelta = new Vector2(0,
            -levelFileOptions.Count * levelFileSpacing);
    }

    /// <summary>
    /// Updates the menu on disable
    /// </summary>
    protected virtual void OnDisable()
    {
        // Destroys the level file options
        for (int i = levelFileOptions.Count - 1; i >= 0; i--)
        {
            Destroy(levelFileOptions[i].gameObject);
            levelFileOptions.RemoveAt(i);
        }
    }

    /// <summary>
    /// Handles clicking on a level file option
    /// </summary>
    /// <param name="value">the new value</param>
    protected virtual void LevelFileValueChanged(bool value)
    {
        // Finds the toggle that was changed and updates the current level name to match it
        audioSource.PlayOneShot(clickSound);
        foreach (Toggle toggle in levelFileOptions)
        {
            if (toggle.isOn)
            { 
                GameManager.Instance.CurrentSaveName = toggle.GetComponentInChildren<Text>().text;
                break;
            }
        }
    }

    #endregion
}
