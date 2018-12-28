using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_Canvas_Controller : MonoBehaviour
{
    #region Variables

    public GameObject pauseWindow;
    public GameObject objectivesButton;
    public GameObject controlsButton;
    public GameObject instructionsButton;
    public GameObject creditsButton;
    public GameObject backstoryButton;
    public GameObject mainMenuButton;
    public GameObject objectivesPanel;
    public GameObject controlsPanel;
    public GameObject instructionsPanel;
    public GameObject creditsPanel;
    public GameObject backstoryPanel;

    #endregion

    #region Start

    // Use this for initialization
    void Start ()
    {
		
	}

    #endregion

    #region Update

    // Update is called once per frame
    void Update ()
    {
		if(Input.GetKeyUp(KeyCode.Escape))
        {
            if(pauseWindow.activeSelf)
            {
                pauseWindow.SetActive(false);
                Game_Manager.Instance.pauseGame = false;
            }
            else
            {
                pauseWindow.SetActive(true);
                Game_Manager.Instance.pauseGame = true;
            }
        }
	}

    #endregion

    #region Public Methods

        #region Load Scene

        public void Load_Scene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

        #endregion

        #region Objectives Window

        public void ObjectivesOpen()
        {
            objectivesButton.SetActive(false);
            controlsButton.SetActive(false);
            instructionsButton.SetActive(false);
            creditsButton.SetActive(false);
            backstoryButton.SetActive(false);
            mainMenuButton.SetActive(false);
            objectivesPanel.SetActive(true);
            controlsPanel.SetActive(false);
            instructionsPanel.SetActive(false);
            creditsPanel.SetActive(false);
            backstoryPanel.SetActive(false);
        }

        #endregion

        #region Controls Window

        public void ControlsOpen()
        {
            objectivesButton.SetActive(false);
            controlsButton.SetActive(false);
            instructionsButton.SetActive(false);
            creditsButton.SetActive(false);
            backstoryButton.SetActive(false);
            mainMenuButton.SetActive(false);
            objectivesPanel.SetActive(false);
            controlsPanel.SetActive(true);
            instructionsPanel.SetActive(false);
            creditsPanel.SetActive(false);
            backstoryPanel.SetActive(false);
        }

        #endregion

        #region Instructions Window

        public void InstructionsOpen()
        {
            objectivesButton.SetActive(false);
            controlsButton.SetActive(false);
            instructionsButton.SetActive(false);
            creditsButton.SetActive(false);
            backstoryButton.SetActive(false);
            mainMenuButton.SetActive(false);
            objectivesPanel.SetActive(false);
            controlsPanel.SetActive(false);
            instructionsPanel.SetActive(true);
            creditsPanel.SetActive(false);
            backstoryPanel.SetActive(false);
        }

        #endregion

        #region Credits Window

        public void CreditsOpen()
        {
            objectivesButton.SetActive(false);
            controlsButton.SetActive(false);
            instructionsButton.SetActive(false);
            creditsButton.SetActive(false);
            backstoryButton.SetActive(false);
            mainMenuButton.SetActive(false);
            objectivesPanel.SetActive(false);
            controlsPanel.SetActive(false);
            instructionsPanel.SetActive(false);
            creditsPanel.SetActive(true);
            backstoryPanel.SetActive(false);
        }

        #endregion

        #region Backstory Window

        public void BackstoryOpen()
        {
            objectivesButton.SetActive(false);
            controlsButton.SetActive(false);
            instructionsButton.SetActive(false);
            creditsButton.SetActive(false);
            backstoryButton.SetActive(false);
            mainMenuButton.SetActive(false);
            objectivesPanel.SetActive(false);
            controlsPanel.SetActive(false);
            instructionsPanel.SetActive(false);
            creditsPanel.SetActive(false);
            backstoryPanel.SetActive(true);
        }

        #endregion

        #region Back To Pause Menu

        public void BackToPauseMenu()
        {
            objectivesButton.SetActive(true);
            controlsButton.SetActive(true);
            instructionsButton.SetActive(true);
            creditsButton.SetActive(true);
            backstoryButton.SetActive(true);
            mainMenuButton.SetActive(true);
            objectivesPanel.SetActive(false);
            controlsPanel.SetActive(false);
            instructionsPanel.SetActive(false);
            creditsPanel.SetActive(false);
            backstoryPanel.SetActive(false);
        }

    #endregion

        #region Main Menu

        public void MainMenu()
        {
            Game_Manager.Instance.firePlantsGrown = 0;
            Game_Manager.Instance.gameOver = false;
            Game_Manager.Instance.pauseGame = false;
            Game_Manager.Instance.placingUpgrade = false;
            Game_Manager.Instance.purchasedFireUpgrade = false;
            Game_Manager.Instance.purchasedIceUpgrade = false;
            Game_Manager.Instance.purchasedVoidUpgrade = false;
            Game_Manager.Instance.purchasedWaterEfficiency = false;
            Game_Manager.Instance.money = 200;
            Game_Manager.Instance.dayTimer = 900;
            Game_Manager.Instance.waveNumber = 1;
            Game_Manager.Instance.firePlantsGrown = 0;
            Game_Manager.Instance.icePlantsGrown = 0;
            Game_Manager.Instance.voidPlantsGrown = 0;
            Game_Manager.Instance.currentPlantSelection = Game_Manager.PlantType.FIRE;
            Game_Manager.Instance.gameStarted = false;
            Load_Scene("Main Menu");
        }

        #endregion

    #endregion
}
