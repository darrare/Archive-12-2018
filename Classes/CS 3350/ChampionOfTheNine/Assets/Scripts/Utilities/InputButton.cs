using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
