using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop_Canvas_Controller : MonoBehaviour
{
    #region Variables

    public GameObject shopWindow;
    public GameObject shopWindowTitle;
    public GameObject farmUpgradesButton;
    public GameObject plantUpgradesButton;
    public GameObject farmUpgradesWindow;
    public GameObject plantUpgradesWindow;
    public GameObject waterEfficiencyUpgradeButton;
    public GameObject fireUpgradeButton;
    public GameObject iceUpgradeButton;
    public GameObject voidUpgradeButton;
    int timer = 0;

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
		if(timer > 0)
        {
            timer--;
        }
        else
        {
            if (Game_Manager.Instance.placingUpgrade)
            {
                Game_Manager.Instance.pauseGame = false;
            }
        }

        if(Game_Manager.Instance.purchasedWaterEfficiency)
        {
            waterEfficiencyUpgradeButton.GetComponentInChildren<Text>().text = "Purchased";
        }

        if (Game_Manager.Instance.purchasedFireUpgrade)
        {
            fireUpgradeButton.GetComponentInChildren<Text>().text = "Purchased";
        }

        if (Game_Manager.Instance.purchasedIceUpgrade)
        {
            iceUpgradeButton.GetComponentInChildren<Text>().text = "Purchased";
        }

        if (Game_Manager.Instance.purchasedVoidUpgrade)
        {
            voidUpgradeButton.GetComponentInChildren<Text>().text = "Purchased";
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

        #region Shop Window

        public void ShopWindowOpenClose()
        {
            if (shopWindow.activeSelf)
            {
                BackToPauseMenu();
                shopWindow.SetActive(false);
                Game_Manager.Instance.pauseGame = false;
            }
            else
            {
                shopWindow.SetActive(true);
                Game_Manager.Instance.pauseGame = true;
            }
        }

        #endregion

        #region Farm Upgrades Window

        public void FarmUpgradesWindow()
        {
            shopWindowTitle.SetActive(false);
            farmUpgradesButton.SetActive(false);
            plantUpgradesButton.SetActive(false);
            farmUpgradesWindow.SetActive(true);
            plantUpgradesWindow.SetActive(false);
        }

        #endregion

        #region Plant Upgrades Window

        public void PlantUpgradesWindow()
        {
            shopWindowTitle.SetActive(false);
            farmUpgradesButton.SetActive(false);
            plantUpgradesButton.SetActive(false);
            farmUpgradesWindow.SetActive(false);
            plantUpgradesWindow.SetActive(true);
        }

        #endregion

        #region Water Efficiency Upgrade

        public void WaterEfficiencyUpgrade()
        {
            if ((Game_Manager.Instance.money >= 200) && !Game_Manager.Instance.purchasedWaterEfficiency)
            {
                waterEfficiencyUpgradeButton.GetComponentInChildren<Text>().text = "Purchased";
                Game_Manager.Instance.purchasedWaterEfficiency = true;
                Game_Manager.Instance.money -= 200;
            }
        }

        #endregion

        #region Sprinkler Upgrade

        public void SprinklerUpgrade()
        {
            if(Game_Manager.Instance.money >= 25)
            {
                Game_Manager.Instance.placingUpgrade = true;
                Game_Manager.Instance.currentUpgrade = Game_Manager.PlaceableUpgrade.Sprinkler;
                Game_Manager.Instance.money -= 25;
                shopWindowTitle.SetActive(true);
                farmUpgradesButton.SetActive(true);
                plantUpgradesButton.SetActive(true);
                farmUpgradesWindow.SetActive(false);
                plantUpgradesWindow.SetActive(false);
                shopWindow.SetActive(false);
                timer = 30;
            }
        }

        #endregion

        #region Fertilizer Upgrade

        public void FertilizerUpgrade()
        {
            if (Game_Manager.Instance.money >= 50)
            {
                Game_Manager.Instance.placingUpgrade = true;
                Game_Manager.Instance.currentUpgrade = Game_Manager.PlaceableUpgrade.Fertilizer;
                Game_Manager.Instance.money -= 50;
                shopWindowTitle.SetActive(true);
                farmUpgradesButton.SetActive(true);
                plantUpgradesButton.SetActive(true);
                farmUpgradesWindow.SetActive(false);
                plantUpgradesWindow.SetActive(false);
                shopWindow.SetActive(false);
                timer = 30;
            }
        }

    #endregion

        #region Fire Plant Upgrade

        public void FirePlantUpgrade()
        {
            if ((Game_Manager.Instance.money >= 200) && !Game_Manager.Instance.purchasedFireUpgrade)
            {
                Game_Manager.Instance.money -= 200;
                Game_Manager.Instance.purchasedFireUpgrade = true;
                fireUpgradeButton.GetComponentInChildren<Text>().text = "Purchased";
            }
        }

        #endregion

        #region Ice Plant Upgrade

        public void IcePlantUpgrade()
        {
            if ((Game_Manager.Instance.money >= 200) && !Game_Manager.Instance.purchasedIceUpgrade)
            {
                Game_Manager.Instance.money -= 200;
                Game_Manager.Instance.purchasedIceUpgrade = true;
                iceUpgradeButton.GetComponentInChildren<Text>().text = "Purchased";
            }
        }

        #endregion

        #region Void Plant Upgrade

        public void VoidPlantUpgrade()
        {
            if ((Game_Manager.Instance.money >= 200) && !Game_Manager.Instance.purchasedVoidUpgrade)
            {
                Game_Manager.Instance.money -= 200;
                Game_Manager.Instance.purchasedVoidUpgrade = true;
                voidUpgradeButton.GetComponentInChildren<Text>().text = "Purchased";
            }
        }

        #endregion

        #region Back To Pause Menu

        public void BackToPauseMenu()
        {
            shopWindowTitle.SetActive(true);
            farmUpgradesButton.SetActive(true);
            plantUpgradesButton.SetActive(true);
            farmUpgradesWindow.SetActive(false);
            plantUpgradesWindow.SetActive(false);
        }

        #endregion

    #endregion
}
