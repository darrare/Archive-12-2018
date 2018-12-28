using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that handles the character creation screen
/// </summary>
public class ControlSelectScript : MenuUIScript
{
    #region Fields

    static ControlSelectScript instance;

    [SerializeField]bool autoShow;
    [SerializeField]protected Text title;
    [SerializeField]GameObject ranger;
    [SerializeField]GameObject mage;
    [SerializeField]GameObject warrior;
    [SerializeField]GameObject remapPanel;
    [SerializeField]Text[] mainTexts;
    [SerializeField]Text[] secondTexts;
    [SerializeField]Text[] powerTexts;
    [SerializeField]Text[] specialTexts;
    protected Dictionary<CharacterType, GameObject> previewObjects;
    Dictionary<InputType, Text[]> abilityTexts;
    protected Dictionary<CharacterType, string> classNames;
    InputType currentRemap;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the static instance of the script
    /// </summary>
    public static ControlSelectScript Instance
    { get { return instance; } }

    #endregion

    #region Public Methods

    /// <summary>
    /// Handles a remap
    /// </summary>
    /// <param name="axis">the axis name</param>
    public void RemapAxisPressed(InputType inputType)
    {
        audioSource.PlayOneShot(GameManager.Instance.GameSounds[Constants.CLICK_SND]);
        remapPanel.SetActive(true);
        currentRemap = inputType;
    }

    public void ControlBackButtonPressed()
    {
        GameManager.Instance.Save();
        GameManager.Instance.LoadLevel(Constants.MAP_SCENE, audioSource);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected override void Start()
    {
        instance = this;
        previewObjects = new Dictionary<CharacterType, GameObject>();
        previewObjects.Add(CharacterType.Ranger, ranger);
        previewObjects.Add(CharacterType.Mage, mage);
        previewObjects.Add(CharacterType.Warrior, warrior);
        abilityTexts = new Dictionary<InputType, Text[]>();
        abilityTexts.Add(InputType.Main, mainTexts);
        abilityTexts.Add(InputType.Secondary, secondTexts);
        abilityTexts.Add(InputType.Power, powerTexts);
        abilityTexts.Add(InputType.Special, specialTexts);
        classNames = new Dictionary<CharacterType, string>();
        classNames.Add(CharacterType.Ranger, "RANGER");
        classNames.Add(CharacterType.Mage, "MAGE");
        classNames.Add(CharacterType.Warrior, "WARRIOR");
        UpdateMapping();

        if (autoShow)
        {
            previewObjects[GameManager.Instance.CurrentSave.PlayerType].SetActive(true);
            title.text = classNames[GameManager.Instance.CurrentSave.PlayerType];
        }
        base.Start();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        if (remapPanel.activeSelf)
        {
            if (Input.inputString != "")
            { 
                string key = Input.inputString[0].ToString();
                foreach (KeyValuePair<InputType, InputButton> input in GameManager.Instance.CurrentSave.Inputs)
                {
                    if (input.Value.Key == key)
                    { input.Value.SetBlank(); }
                }
                GameManager.Instance.CurrentSave.Inputs[currentRemap].Key = key;
                UpdateMapping();
            }
            else
            {
                // Check mouse buttons
                for (int i = 0; i < 7; i++)
                {
                    if (Input.GetMouseButtonDown(i))
                    {
                        foreach (KeyValuePair<InputType, InputButton> input in GameManager.Instance.CurrentSave.Inputs)
                        {
                            if (input.Value.MouseButton == i)
                            { input.Value.SetBlank(); }
                        }
                        GameManager.Instance.CurrentSave.Inputs[currentRemap].MouseButton = i;
                        UpdateMapping();
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Updates button mapping text on remap
    /// </summary>
    private void UpdateMapping()
    {
        foreach (KeyValuePair<InputType, Text[]> texts in abilityTexts)
        {
            foreach (Text txt in texts.Value)
            { txt.text = GameManager.Instance.CurrentSave.Inputs[texts.Key].Name; }
        }
        remapPanel.SetActive(false);
    }

    #endregion
}
