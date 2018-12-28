using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.IO;

delegate void ClickBehaviour();

public class LevelBuilderUIScript : MonoBehaviour
{
    #region fields

    public Text debugText;

    ClickBehaviour OnClickAction;

    [SerializeField]
    RectTransform optionsPanel;

    [SerializeField]
    RectTransform buildPanel;

    [SerializeField]
    RectTransform savePanel;

    [SerializeField]
    RectTransform loadPanel;

    [SerializeField]
    RectTransform uploadPanel;

    List<RectTransform> panels = new List<RectTransform>(); //stores all the above panels so I can foreach over them

    [SerializeField]
    RectTransform verifyDeletionPanel;

    [SerializeField]
    RectTransform verifyMainMenuPanel;

    [SerializeField]
    RectTransform verifyLoadPanel;

    [SerializeField]
    RectTransform verifyOverwritePanel;

    [SerializeField]
    GameObject parentObject;

    [SerializeField]
    InputField saveLevelName;

    [SerializeField]
    InputField saveCreatorName;

    [SerializeField]
    InputField loadLevelName;

    [SerializeField]
    Dropdown loadDropdown;

    bool doButtonsWork = true; //Whenever the different panels are moving around, we will disable the buttons so shit doesn't get weird
    float transitionTimer = 0;
    float transitionMaxTime = .5f;
    float cameraZoomMin = -30f, cameraZoomMax = -6f;
    float cameraVelocityScale = 60f;
    float maxCameraVelocity = 30f;
    bool canClick = true;

    #endregion

    #region properties

    /// <summary>
    /// The current type of tool selected
    /// </summary>
    public LevelObjectType PlacingToolType
    { get; private set; }

    #endregion

    #region private methods

    // Use this for initialization
    void Awake ()
    {
        UIManager.Instance.LevelBuilderUI = this;
        verifyDeletionPanel.gameObject.SetActive(false);
        verifyMainMenuPanel.gameObject.SetActive(false);
        verifyLoadPanel.gameObject.SetActive(false);
        verifyOverwritePanel.gameObject.SetActive(false);

        panels.Add(optionsPanel);
        panels.Add(buildPanel);
        panels.Add(savePanel);
        panels.Add(loadPanel);
        panels.Add(uploadPanel);

        OnClickAction = new ClickBehaviour(PanToolAction);
	}
	
	/// <summary>
    /// Called once per frame
    /// </summary>
	void Update ()
    {
        //if the mouse button is down, and the mouse is not over any UI element
#if UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0 && canClick && !transform.GetComponentsInChildren<RectTransform>().Where(t => t.transform.parent == transform && t.gameObject.activeInHierarchy).Any(t => RectTransformUtility.RectangleContainsScreenPoint(t, Input.GetTouch(0).position, LevelBuilderManager.Instance.MainCam.Camera)))
        {
            OnClickAction();
        }
        else if (!canClick)
        {
            canClick = true;
        }
#else
        if (Input.GetMouseButton(0) && canClick && !transform.GetComponentsInChildren<RectTransform>().Where(t => t.transform.parent == transform && t.gameObject.activeInHierarchy).Any(t => RectTransformUtility.RectangleContainsScreenPoint(t, Input.mousePosition, LevelBuilderManager.Instance.MainCam.Camera)))
        {
            OnClickAction();
        }
        else if (!canClick)
        {
            canClick = true;
        }
#endif

    }

    /// <summary>
    /// Referenced by delegate OnClickAction to dictate what to do whenever a click is detected
    /// </summary>
    Vector3 position;
    RaycastHit hit;
    Ray ray;
    void PlacingToolAction()
    {
        if (!LevelBuilderManager.Instance.LevelBuilderObjects.ContainsKey(PlacingToolType))
        {
            debugText.text += "\n We don't have this tool.";
            return;
        }
#if UNITY_IOS || UNITY_ANDROID
        ray = LevelBuilderManager.Instance.MainCam.Camera.ScreenPointToRay(Input.GetTouch(0).position);
        if (!Physics.Raycast(ray, out hit) && xPlane.Raycast(ray, out distanceToXPlane))
        {
            position = ray.GetPoint(distanceToXPlane);
            debugText.text += "\n Adding new block at position: " + position;
            position = new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), 0);
            //LevelBuilderManager.Instance.LevelObjects.Add(new LevelBuilderObject(position, PlacingToolType)); //This is something we probably want to do whenever we save, instead of when the object is placed.
            GameObject newObj = Instantiate(LevelBuilderManager.Instance.LevelBuilderObjects[PlacingToolType], parentObject.transform, false);
            newObj.transform.position = position;
            newObj.name = PlacingToolType.ToString();
        }
#else
        ray = LevelBuilderManager.Instance.MainCam.Camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out hit) && xPlane.Raycast(ray, out distanceToXPlane))
        {
            position = ray.GetPoint(distanceToXPlane);
            position = new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), 0);
            //LevelBuilderManager.Instance.LevelObjects.Add(new LevelBuilderObject(position, PlacingToolType)); //This is something we probably want to do whenever we save, instead of when the object is placed.
            GameObject newObj = Instantiate(LevelBuilderManager.Instance.LevelBuilderObjects[PlacingToolType], parentObject.transform, false);
            newObj.transform.position = position;
            newObj.name = PlacingToolType.ToString();
        }
#endif
    }

    /// <summary>
    /// Referenced by delegate OnClickAction to dictate what to do whenever a click is detected
    /// </summary>
    void ErasingToolAction()
    {
#if UNITY_IOS || UNITY_ANDROID
        ray = LevelBuilderManager.Instance.MainCam.Camera.ScreenPointToRay(Input.GetTouch(0).position);
        if (Physics.Raycast(ray, out hit))
        {
            Destroy(hit.transform.gameObject);
        }
#else
        ray = LevelBuilderManager.Instance.MainCam.Camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Destroy(hit.transform.gameObject);
        }
#endif

    }

    /// <summary>
    /// Referenced by delegate OnClickAction to dictate what to do whenever a click is detected
    /// </summary>
    Vector3? prevTouchPosition = null;
    Vector3? curTouchPosition = null;
    float panTimer = 0;
    float panTimerGoal = .05f;
    bool resetPanToolIsRunning = false;
    Plane xPlane = new Plane(Vector3.forward, Vector3.zero); //a plane that touches every point at z = 0 on the x-y coordinates
    float distanceToXPlane;
    Vector3 unClampedVelocity;
    void PanToolAction()
    {
        if (!resetPanToolIsRunning)
        {
            resetPanToolIsRunning = true;
            StartCoroutine(ResetPanTool());
        }

        panTimer = 0;

#if UNITY_IOS || UNITY_ANDROID
        if (prevTouchPosition == null)
        {
            prevTouchPosition = Input.GetTouch(0).position;
        }
        else
        {
            ray = LevelBuilderManager.Instance.MainCam.Camera.ScreenPointToRay((Vector3)prevTouchPosition);

            if (xPlane.Raycast(ray, out distanceToXPlane))
            {
                prevTouchPosition = ray.GetPoint(distanceToXPlane);
            }

            ray = LevelBuilderManager.Instance.MainCam.Camera.ScreenPointToRay(Input.GetTouch(0).position);

            if (xPlane.Raycast(ray, out distanceToXPlane))
            {
                curTouchPosition = ray.GetPoint(distanceToXPlane);
            }

            //LevelBuilderManager.Instance.MainCam.transform.position += (Vector3)(prevTouchPosition - curTouchPosition);
            unClampedVelocity = (Vector3)(prevTouchPosition - curTouchPosition) * cameraVelocityScale;
            if (unClampedVelocity.magnitude > maxCameraVelocity)
            {
                unClampedVelocity *= (maxCameraVelocity / unClampedVelocity.magnitude);
            }
            LevelBuilderManager.Instance.MainCam.rBody.velocity = unClampedVelocity;// new Vector3(Mathf.Clamp(unClampedVelocity.x, -maxCameraVelocity, maxCameraVelocity), Mathf.Clamp(unClampedVelocity.y, -maxCameraVelocity, maxCameraVelocity), 0);
            prevTouchPosition = Input.mousePosition;
#else
        if (prevTouchPosition == null)
        {
            prevTouchPosition = Input.mousePosition;
        }
        else
        {
            ray = LevelBuilderManager.Instance.MainCam.Camera.ScreenPointToRay((Vector3)prevTouchPosition);

            if (xPlane.Raycast(ray, out distanceToXPlane))
            {
                prevTouchPosition = ray.GetPoint(distanceToXPlane);
            }

            ray = LevelBuilderManager.Instance.MainCam.Camera.ScreenPointToRay(Input.mousePosition);

            if (xPlane.Raycast(ray, out distanceToXPlane))
            {
                curTouchPosition = ray.GetPoint(distanceToXPlane);
            }

            //LevelBuilderManager.Instance.MainCam.transform.position += (Vector3)(prevTouchPosition - curTouchPosition);
            unClampedVelocity = (Vector3)(prevTouchPosition - curTouchPosition) * cameraVelocityScale;
            if (unClampedVelocity.magnitude > maxCameraVelocity)
            {
                unClampedVelocity *= (maxCameraVelocity / unClampedVelocity.magnitude);
            }
            LevelBuilderManager.Instance.MainCam.rBody.velocity = unClampedVelocity;// new Vector3(Mathf.Clamp(unClampedVelocity.x, -maxCameraVelocity, maxCameraVelocity), Mathf.Clamp(unClampedVelocity.y, -maxCameraVelocity, maxCameraVelocity), 0);
            prevTouchPosition = Input.mousePosition;
#endif
        }
}

    /// <summary>
    /// Basically a timer that will fire if the PanToolAction was called, and then not called for a very short time
    /// </summary>
    IEnumerator ResetPanTool()
    {
        for (;;)
        {
            panTimer += Time.deltaTime;
            if (panTimer >= panTimerGoal)
            {
                panTimer = 0;
                prevTouchPosition = null;
                resetPanToolIsRunning = false;
                yield break;
            }
            yield return null;
        }
    }

#endregion

#region Buttons that open new panels

    /// <summary>
    /// Called whenever the UI button to go to the tool panel is clicked
    /// </summary>
    public void ToolButtonClicked()
    {
        if (doButtonsWork)
        {
            StartCoroutine(SlidePanels(buildPanel));
        }
    }

    /// <summary>
    /// Called whenever the UI button to go to the trash confirmation panel is clicked
    /// </summary>
    public void TrashButtonClicked()
    {
        if (doButtonsWork)
        {
            verifyDeletionPanel.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Called whenever the UI button to go to the save panel is clicked
    /// </summary>
    public void SaveButtonClicked()
    {
        if (doButtonsWork)
        {
            StartCoroutine(SlidePanels(savePanel));
        }
    }

    /// <summary>
    /// Called whenever the UI button to go to the load panel is clicked
    /// </summary>
    public void LoadButtonClicked()
    {
        if (doButtonsWork)
        {
            StartCoroutine(SlidePanels(loadPanel));
            loadDropdown.ClearOptions();
            loadDropdown.AddOptions(LevelBuilderManager.Instance.LevelNames);
        }
    }

    /// <summary>
    /// Called whenever the UI button to go to the upload panel is clicked
    /// </summary>
    public void UploadButtonClicked()
    {
        if (doButtonsWork)
        {
            StartCoroutine(SlidePanels(uploadPanel));
        }
    }

    /// <summary>
    /// Called whenever the UI button to go to the main menu return panel is cicked
    /// </summary>
    public void MainMenuButtonClicked()
    {
        if (doButtonsWork)
        {
            verifyMainMenuPanel.gameObject.SetActive(true);
        }
    }

#endregion

#region Buttons that have specific functions

    /// <summary>
    /// Returns the UI to display just the default options panel
    /// </summary>
    public void ReturnToOptionsPanelClicked()
    {
        if (doButtonsWork)
        {
            //TODO: Annoying issue where if I come to this method via a full screen panel, it will cause the options panel to slide out.
            verifyDeletionPanel.gameObject.SetActive(false);
            verifyMainMenuPanel.gameObject.SetActive(false);
            verifyOverwritePanel.gameObject.SetActive(false);
            verifyLoadPanel.gameObject.SetActive(false);
            StartCoroutine(SlidePanels(optionsPanel));
        }
    }

    /// <summary>
    /// The player has selected a specific block type to spawn
    /// </summary>
    public void PlacingToolClicked(LevelObjectType type)
    {
        if (doButtonsWork)
        {
            PlacingToolType = type;
            OnClickAction = PlacingToolAction;
        }
    }

    /// <summary>
    /// Called when the eraser tool has been clicked.
    /// </summary>
    public void EraserToolClicked()
    {
        if (doButtonsWork)
        {
            OnClickAction = ErasingToolAction;
        }
    }

    /// <summary>
    /// Called when the pan tool has been clicked.
    /// </summary>
    public void PanToolClicked()
    {
        if (doButtonsWork)
        {
            OnClickAction = PanToolAction;
        }
    }

    /// <summary>
    /// Called whenever the zoom in tool has been clicked.
    /// </summary>
    public void ZoomInClicked()
    {
        if (doButtonsWork)
        {
            //Top one is for perspective camera, bottom for ortho
            canClick = false;
            LevelBuilderManager.Instance.MainCam.transform.position = new Vector3(LevelBuilderManager.Instance.MainCam.transform.position.x, LevelBuilderManager.Instance.MainCam.transform.position.y, Mathf.Clamp(LevelBuilderManager.Instance.MainCam.transform.position.z + 2, cameraZoomMin, cameraZoomMax));
            //LevelBuilderManager.Instance.MainCam.Camera.orthographicSize = Mathf.Clamp(LevelBuilderManager.Instance.MainCam.Camera.orthographicSize - 1, cameraZoomMin, cameraZoomMax);
        }
    }

    /// <summary>
    /// Called whenever the zoom out tool has been clicked.
    /// </summary>
    public void ZoomOutClicked()
    {
        if (doButtonsWork)
        {
            //Top one is for perspective camera, bottom for ortho
            canClick = false;
            LevelBuilderManager.Instance.MainCam.transform.position = new Vector3(LevelBuilderManager.Instance.MainCam.transform.position.x, LevelBuilderManager.Instance.MainCam.transform.position.y, Mathf.Clamp(LevelBuilderManager.Instance.MainCam.transform.position.z - 2, cameraZoomMin, cameraZoomMax));
            //LevelBuilderManager.Instance.MainCam.Camera.orthographicSize = Mathf.Clamp(LevelBuilderManager.Instance.MainCam.Camera.orthographicSize + 1, cameraZoomMin, cameraZoomMax);
        }
    }

    /// <summary>
    /// The user has confirmed to remove every object from the scene.
    /// </summary>
    public void EraseConfirmButtonClicked()
    {
        if (doButtonsWork)
        {
            OnClickAction = PanToolAction;
            verifyDeletionPanel.gameObject.SetActive(false);
            verifyLoadPanel.gameObject.SetActive(false);
            verifyOverwritePanel.gameObject.SetActive(false);
            foreach (Transform t in parentObject.transform)
            {
                Destroy(t.gameObject);
            }
        }
    }
    
    /// <summary>
    /// The user has confirmed to save this scene
    /// </summary>
    public void SaveConfirmButtonClicked()
    {
        if (doButtonsWork)
        {
            if (saveLevelName.text == "" || saveCreatorName.text == "")
            {
                //TODO: Error, the player hasn't filled out the required information. Play a sound or do something here.
            }
#if UNITY_IOS || UNITY_ANDROID
            else if (File.Exists(Application.persistentDataPath + @"\" + saveLevelName.text + ".LevelData"))
            {
                //TODO: popup the warning UI that says the file already exists, are they sure they want to replace it.
                verifyOverwritePanel.gameObject.SetActive(true);
            }
#else
            else if (File.Exists(saveLevelName.text + ".LevelData"))
            {
                //TODO: popup the warning UI that says the file already exists, are they sure they want to replace it.
                verifyOverwritePanel.gameObject.SetActive(true);
            }
#endif
            else
            {
                SaveOverwriteConfirmedButtonClick();
            }
        }
    }

    /// <summary>
    /// The player has been warned that the file already exists, but they have chosen to save over it
    /// </summary>
    public void SaveOverwriteConfirmedButtonClick()
    {
        //TODO: Currently the notes field literally does nothing. Perhaps remove it and simply put it on the cloud button submission.
        List<LevelBuilderObject> level = new List<LevelBuilderObject>();

        foreach (Transform t in parentObject.transform)
        {
            level.Add(new LevelBuilderObject(t.position, (LevelObjectType)Enum.Parse(typeof(LevelObjectType), t.name, true)));
        }
        LevelBuilderManager.Instance.SerializeListOfLevelBuilderObject(level, saveLevelName.text, saveCreatorName.text);
        ReturnToOptionsPanelClicked();
    }

    /// <summary>
    /// The user has clicked a button to load a scene, we need to verify that they want to delete everything currently there.
    /// </summary>
    public void LoadVerifyDeletionOfCurrentButtonClicked()
    {
        if (doButtonsWork)
        {
            verifyLoadPanel.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// The user has confirmed to load a scene
    /// </summary>
    public void LoadConfirmButtonClicked()
    {
        if (doButtonsWork)
        {
            LevelData level = null;
            if (loadLevelName.text != "")
            {
                level = LevelBuilderManager.Instance.DeserializeLevelData(loadLevelName.text);
            }
            else if (loadDropdown.captionText.text != "")
            {
                level = LevelBuilderManager.Instance.DeserializeLevelData(loadDropdown.captionText.text.Split('.')[0]);
            }
            else
            {
                //TODO: Error, neither of the two requirements above were met.
            }

            if (level != null)
            {
                EraseConfirmButtonClicked(); //Delete the current level before we create the new level
                GameObject newObj;
                foreach (LevelBuilderObject l in level.Data)
                {
                    newObj = Instantiate(LevelBuilderManager.Instance.LevelBuilderObjects[l.Type], parentObject.transform, false);
                    newObj.transform.position = l.Position;
                    newObj.name = l.Type.ToString();
                }
                ReturnToOptionsPanelClicked();
            }
        }
    }

    /// <summary>
    /// The user has confirmed to upload the level.
    /// </summary>
    public void UploadConfirmButtonClicked()
    {
        //Do all the fun automatic email stuff to email the serialized level to me.
        if (doButtonsWork)
        {

        }
    }

    /// <summary>
    /// User has clicked the button to return to the main menu and lose all unsaved progress.
    /// </summary>
    public void ReturnToMainMenuConfirmButtonClicked()
    {
        if (doButtonsWork)
        {
            //load into the new scene   
        }
    }

#endregion

#region Coroutines

    /// <summary>
    /// Moves all of the panels to where they should be over time
    /// </summary>
    /// <param name="panelToBeVisible">The panel that should become visible after the transition</param>
    IEnumerator SlidePanels(RectTransform panelToBeVisible)
    {
        //If the panel is already out, don't do anything
        if (panelToBeVisible.anchoredPosition.x < -(panelToBeVisible.rect.width / 2) - 1)
        {
            yield break;
        }
        //otherwise, make the panels move to their correct positions
        for (;;)
        {
            transitionTimer += Time.deltaTime;
            doButtonsWork = false;

            foreach (RectTransform r in panels)
            {
                if (r == panelToBeVisible)
                {
                    r.anchoredPosition = Vector2.Lerp(new Vector2((r.rect.width / 2) + 2, 0), new Vector2(-(r.rect.width / 2) - 2, 0), transitionTimer / transitionMaxTime);
                }
                else if (r.anchoredPosition.x <= (r.rect.width / 2) + 1)
                {
                    r.anchoredPosition = Vector2.Lerp(new Vector2(-(r.rect.width / 2) - 2, 0), new Vector2((r.rect.width / 2) + 2, 0), transitionTimer / transitionMaxTime);
                }
            }

            if (transitionTimer >= transitionMaxTime)
            {
                transitionTimer = 0;
                doButtonsWork = true;
                yield break;
            }
            else
            {
                yield return null;
            }
        }
    }

#endregion
}
