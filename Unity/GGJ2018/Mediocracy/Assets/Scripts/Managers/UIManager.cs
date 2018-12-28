using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Mediocracy
{
    public enum MenuType
    {
        MainMenu,
        PauseMenu,
        WinMenu,
        LoseMenu,
        GameOverlay,
        RhythmGame,
        Tuple,
        None
    }


    public class UIManager
    {

        #region singleton

        static UIManager instance;

        /// <summary>
        /// Singleton for the uimanager class
        /// </summary>
        public static UIManager Instance
        { get { return instance ?? (instance = new UIManager()); } }

        UIManager()
        {
            Dictionary<string, GameObject> prefabs = Resources.LoadAll<GameObject>(Constants.UI_PREFABS_FILE_LOCATION).ToDictionary(t => t.name);

            foreach (KeyValuePair<string, GameObject> p in prefabs)
            {
                UIElementPrefabs.Add((MenuType)Enum.Parse(typeof(MenuType), p.Key, true), p.Value);
            }
        }

        #endregion

        #region properties

        /// <summary>
        /// UI Elements that need to be opened/closed
        /// </summary>
        private Dictionary<MenuType, GameObject> UIElementPrefabs
        { get; set; } = new Dictionary<MenuType, GameObject>();

        /// <summary>
        /// Stack of current UI elements
        /// </summary>
        private List<UIElementScript> UIStack
        { get; set; } = new List<UIElementScript>();

        #endregion

        #region public methods

        /// <summary>
        /// Opens up a menu
        /// </summary>
        /// <param name="type">The type of menu we want to open</param>
        public void OpenUI(MenuType type)
        {
            if (UIStack.All(t => t.Type != type))
            {
                UIStack.Add(Object.Instantiate(UIElementPrefabs[type]).GetComponent<UIElementScript>());
            }
        }

        /// <summary>
        /// Closes a specific type of ui element (probably based on what they clicked)
        /// </summary>
        /// <param name="type">The type of Ui we want to close</param>
        public void CloseUI(MenuType type)
        {
            UIStack.RemoveAll(t => t.Type == type);

            if (UIStack.Count == 0)
                GameManager.Instance.IsPaused = false;
        }

        /// <summary>
        /// Checks to see if a menu is open
        /// </summary>
        /// <param name="type">The type of menu to check</param>
        public bool IsMenuOpen(MenuType type = MenuType.None)
        {
            return UIStack.Any(t => t.Type == type);
        }

        /// <summary>
        /// Checks to see whether any menu is open
        /// </summary>
        /// <returns></returns>
        public bool IsAnyMenuOpen()
        {
            return UIStack.Count != 0;
        }

        #endregion
    }

}
