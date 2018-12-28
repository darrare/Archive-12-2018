using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEditor.SceneManagement;

public class UI_Canvas_Controller : MonoBehaviour
{
    #region Variables

    public GameObject instructionsWindow;
    public GameObject gameOverWindow;
    public GameObject gameOverMenuButton;
    //public GameObject gameOverNextLevelButton;
    public GameObject gameOverTitle;
    public GameObject settingsWindow;
    public GameObject shopButton;
    public Button playButton;
    public Button instructionsButton;
    public Button creditsButton;
    public Button instructionsBack;
    public Button creditsBack;
    public Button mainMenu;

    #endregion

    #region Start

    // Use this for initialization
    void Start ()
    {
        Game_Manager.Instance.firePlantsGrown = 0;
        Game_Manager.Instance.gameOver = false;
        Game_Manager.Instance.money = 200;
        Game_Manager.Instance.waterLevel = 100;

        if (playButton)
        {
            if ((Input.GetJoystickNames().Length > 0) && (Input.GetJoystickNames()[0] != ""))
            {
                playButton.Select();
            }

        }
    }

    #endregion

    #region Update

    // Update is called once per frame
    void Update ()
    {
        Game_Manager.Instance.Update();

        if (Game_Manager.Instance.gameOver)
        {
            gameOverWindow.SetActive(true);
            gameOverMenuButton.SetActive(true);
            //gameOverNextLevelButton.SetActive(true);

            if ((Input.GetJoystickNames().Length > 0) && (Input.GetJoystickNames()[0] != ""))
            {
                mainMenu.Select();
            }

            if ((Game_Manager.Instance.firePlantsGrown >= Game_Manager.Instance.firePlantsRequired) && (Game_Manager.Instance.icePlantsGrown >= Game_Manager.Instance.icePlantsRequired) && (Game_Manager.Instance.voidPlantsGrown >= Game_Manager.Instance.voidPlantsRequired))
            {
                gameOverTitle.GetComponent<Text>().text = "You Win!";
            }
            else
            {
                gameOverTitle.GetComponent<Text>().text = "You Lose!";
            }
        }
	}

    #endregion

    #region Public Methods

        #region Start Game

        public void StartGame()
        {
            shopButton.SetActive(true);
            gameOverMenuButton.SetActive(false);
            //gameOverNextLevelButton.SetActive(false);
            gameOverWindow.SetActive(false);

            Game_Manager.Instance.StartLevel(Game_Manager.Instance.currentLevel);
            Game_Manager.Instance.gameStarted = true;

            switch(Game_Manager.Instance.currentLevel)
            {
                case Game_Manager.Level.ONE:
                    Game_Manager.Instance.currentLevel = Game_Manager.Level.TWO;
                    break;
                case Game_Manager.Level.TWO:
                    Game_Manager.Instance.currentLevel = Game_Manager.Level.THREE;
                    break;
                case Game_Manager.Level.THREE:
                    Game_Manager.Instance.currentLevel = Game_Manager.Level.FOUR;
                    break;
            }
        }

        #endregion

        #region Load Scene

        public void Load_Scene(string sceneName)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            }

            #endregion

        #region Instructions

        public void InstructionsOpen()
        {
            instructionsWindow.SetActive(true);

            if ((Input.GetJoystickNames().Length > 0) && (Input.GetJoystickNames()[0] != ""))
            {
                instructionsBack.Select();
            }
        }

        public void InstructionsClose()
        {
            instructionsWindow.SetActive(false);

            if ((Input.GetJoystickNames().Length > 0) && (Input.GetJoystickNames()[0] != ""))
            {
                instructionsButton.Select();
            }
        }

        #endregion

        #region Settings

        public void SettingsOpenClose()
        {
            if (settingsWindow.activeSelf)
            {
                settingsWindow.SetActive(false);
            }
            else
            {
                settingsWindow.SetActive(true);
            }
        }

        #endregion

        #region Set Controls

        public void SetControls(string newControls)
        {
            switch(newControls)
            {
                case "WASD":
                    Game_Manager.Instance.currentControls = Game_Manager.ControlScheme.WASD;
                    break;
                case "Arrows":
                    Game_Manager.Instance.currentControls = Game_Manager.ControlScheme.ARROWS;
                    break;
                case "IJKL":
                    Game_Manager.Instance.currentControls = Game_Manager.ControlScheme.IJKL;
                    break;
                default:
                    Game_Manager.Instance.currentControls = Game_Manager.ControlScheme.WASD;
                    break;
            }
        }

        #endregion

        #region Credits

        public void CreditsOpen()
        {
            gameOverWindow.SetActive(true);

            if ((Input.GetJoystickNames().Length > 0) && (Input.GetJoystickNames()[0] != ""))
            {
                creditsBack.Select();
            }
        }

        public void CreditsClose()
        {
            gameOverWindow.SetActive(false);

            if ((Input.GetJoystickNames().Length > 0) && (Input.GetJoystickNames()[0] != ""))
            {
                creditsButton.Select();
            }
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
