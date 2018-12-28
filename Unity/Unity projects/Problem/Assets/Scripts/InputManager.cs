using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;

public static class InputManager {
	public enum ControllerLayout {RETRO, NEWAGE, TRIGGERHAPPY};

	public static Dictionary<string, CustomInput> keys = new Dictionary<string, CustomInput>();
	static Dictionary<string, CustomInput> retro = new Dictionary<string, CustomInput>();
	static Dictionary<string, CustomInput> newAge = new Dictionary<string, CustomInput>();
	static Dictionary<string, CustomInput> triggerHappy = new Dictionary<string, CustomInput>();

	public static void Initialize()
	{
		//This is where I will need to load in from a file the default settings.
		//Retro
		retro.Add ("Jump", new CustomInput ("A", true));
		retro.Add ("Shoot0", new CustomInput ("RightBumper", true));
		retro.Add ("Shoot1", new CustomInput ("LeftBumper", true));
		retro.Add ("Horizontal", new CustomInput ("LeftStickHorizontal", false));
		retro.Add ("Vertical", new CustomInput ("LeftStickVertical", false));
		retro.Add ("CameraPan", new CustomInput ("RightStickVertical", false));
		retro.Add ("Start", new CustomInput("Start", true));
		retro.Add ("Sprint", new CustomInput ("X", true));
		retro.Add ("Interact", new CustomInput ("Y", true));
		retro.Add ("RightTrigger", new CustomInput ("RightTrigger", false));
		retro.Add ("LeftTrigger", new CustomInput ("LeftTrigger", false));
		retro.Add ("RightBumper", new CustomInput ("RightBumper", true));
		retro.Add ("LeftBumper", new CustomInput ("LeftBumper", true));
		retro.Add("B", new CustomInput("B", true));

		//NewAge
		newAge.Add ("Jump", new CustomInput ("A", true));
		newAge.Add ("Shoot0", new CustomInput ("RightBumper", true));
		newAge.Add ("Shoot1", new CustomInput("RightTrigger", false));
		newAge.Add ("Horizontal", new CustomInput ("LeftStickHorizontal", false));
		newAge.Add ("Vertical", new CustomInput ("LeftStickVertical", false));
		newAge.Add ("CameraPan", new CustomInput ("RightStickVertical", false));
		newAge.Add ("Start", new CustomInput("Start", true));
		newAge.Add ("Sprint", new CustomInput ("LeftTrigger", false));
		newAge.Add ("Interact", new CustomInput ("Y", true));
		newAge.Add ("RightTrigger", new CustomInput ("RightTrigger", false));
		newAge.Add ("LeftTrigger", new CustomInput ("LeftTrigger", false));
		newAge.Add ("RightBumper", new CustomInput ("RightBumper", true));
		newAge.Add ("LeftBumper", new CustomInput ("LeftBumper", true));
		newAge.Add("B", new CustomInput("B", true));

		//TriggerHappy
		triggerHappy.Add ("Jump", new CustomInput ("A", true));
		triggerHappy.Add ("Shoot0", new CustomInput("RightTrigger", false));
		triggerHappy.Add ("Shoot1", new CustomInput ("LeftTrigger", false));
		triggerHappy.Add ("Horizontal", new CustomInput ("LeftStickHorizontal", false));
		triggerHappy.Add ("Vertical", new CustomInput ("LeftStickVertical", false));
		triggerHappy.Add ("CameraPan", new CustomInput ("RightStickVertical", false));
		triggerHappy.Add ("Start", new CustomInput("Start", true));
		triggerHappy.Add ("Sprint", new CustomInput ("X", true));
		triggerHappy.Add ("Interact", new CustomInput ("Y", true));
		triggerHappy.Add ("RightTrigger", new CustomInput ("RightTrigger", false));
		triggerHappy.Add ("LeftTrigger", new CustomInput ("LeftTrigger", false));
		triggerHappy.Add ("RightBumper", new CustomInput ("RightBumper", true));
		triggerHappy.Add ("LeftBumper", new CustomInput ("LeftBumper", true));
		triggerHappy.Add("B", new CustomInput("B", true));


		keys = retro;
	}

	public static void SwitchControlScheme(ControllerLayout value)
	{
		if (value == ControllerLayout.RETRO) {
			keys = retro;
		} else if (value == ControllerLayout.NEWAGE) {
			keys = newAge;
		} else if (value == ControllerLayout.TRIGGERHAPPY) {
			keys = triggerHappy;
		}
	}

	public static float GetAxis(string value)
	{
		return keys [value].GetAxis ();
	}

	public static float GetAxisRaw(string value)
	{
		return keys [value].GetAxisRaw ();
	}

	public static bool GetButton(string value)
	{
		return keys [value].GetButton ();
	}

	public static bool GetButtonDown(string value)
	{
		return keys [value].GetButtonDown ();
	}

	public static bool GetButtonUp(string value)
	{
		return keys [value].GetButtonUp ();
	}
}



public class CustomInput {
	bool isButton;
	string managerName;
	bool canActivate = true;

	public CustomInput(string managerName, bool isButton)
	{
		this.managerName = managerName;
		this.isButton = isButton;
	}

	public float GetAxis()
	{
		return Input.GetAxis (managerName);
	}

	public float GetAxisRaw()
	{
		return Input.GetAxisRaw (managerName);
	}

	public bool GetButton()
	{
		bool result = false;
		if (isButton && Input.GetButton (managerName)) {
			result = true;
		} else if (!isButton && Input.GetAxisRaw (managerName) != 0) {
			result = true;
		}
		return result;
	}

	public bool GetButtonDown()
	{
		bool result = false;
		if (isButton && Input.GetButtonDown (managerName)) {
			result = true;
		} else if (!isButton && canActivate && Input.GetAxisRaw (managerName) != 0) {
			result = true;
			canActivate = false;
		} else if (!isButton && Input.GetAxisRaw(managerName) == 0){
			canActivate = true;
		}
		return result;
	}

	public bool GetButtonUp()
	{
		bool result = false;
		if (isButton && Input.GetButtonUp (managerName)) {
			result = true;
		} else if (!isButton && Input.GetAxisRaw (managerName) == 0) {
			result = true;
		}
		return result;
	}
}


















/*
 NOTE: the following was the code used in the UI that remapped the controls. The class is below.
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
 */

/// <summary>
/// Handles an input button for use with custom controls
/// </summary>
[Serializable]
public class InputButton
{
    #region Fields

    bool blank = false;
    bool mouse = true;
    int mouseButton = -1;
    string key = "";

    #endregion

    #region Constructors

    /// <summary>
    /// Mouse button constructor
    /// </summary>
    /// <param name="mouseButton">the mouse button to use</param>
    public InputButton(int mouseButton)
    {
        MouseButton = mouseButton;
    }

    /// <summary>
    /// Keyboard key constructor
    /// </summary>
    /// <param name="key">the keyboard key to use</param>
    public InputButton(string key)
    {
        Key = key;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the name of the button
    /// </summary>
    public string Name
    {
        get
        {
            if (blank)
            { return "(none)"; }
            else
            {
                if (mouse)
                {
                    if (GameManager.Instance.MouseButtonNames.ContainsKey(mouseButton))
                    { return GameManager.Instance.MouseButtonNames[mouseButton]; }
                    else
                    { return "Mouse Button " + mouseButton; }
                }
                else
                {
                    if (GameManager.Instance.KeyNames.ContainsKey(key))
                    { return GameManager.Instance.KeyNames[key]; }
                    else
                    { return key.ToUpper(); }
                }
            }
        }
    }

    /// <summary>
    /// Gets the name of the button with its type and formatting
    /// </summary>
    public string NameWithType
    {
        get
        {
            string text = "<color=blue><b>" + Name;
            if (!blank && !mouse)
            { text += " key"; }
            return text + "</b></color>";
        }
    }

    /// <summary>
    /// Gets or sets the input button as a mouse button
    /// </summary>
    public int MouseButton
    {
        get { return mouseButton; }
        set
        {
            mouseButton = value;
            mouse = true;
            blank = false;
        }
    }

    /// <summary>
    /// Gets or sets the input button as a keyboard key
    /// </summary>
    public string Key
    {
        get { return key; }
        set
        {
            key = value.ToLower();
            mouse = false;
            blank = false;
        }
    }

    /// <summary>
    /// Gets whether or not the button is currently pressed
    /// </summary>
    public bool Pressed
    {
        get
        {
            if (mouse)
            { return Input.GetMouseButton(mouseButton); }
            else
            { return Input.GetKey(key); }
        }
    }

    /// <summary>
    /// Gets or sets whether the button is a mouse button or not
    /// </summary>
    private bool Mouse
    {
        get { return mouse; }
        set
        {
            mouse = value;
            if (mouse)
            { key = ""; }
            else
            { mouseButton = -1; }
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Sets the button as blank
    /// </summary>
    public void SetBlank()
    {
        blank = true;
        key = "";
        mouseButton = -1;
    }

    #endregion
}


