using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    #region Singleton stuff

    static GameManager instance;

    /// <summary>
    /// Singleton
    /// </summary>
    public static GameManager Instance
    { get { return instance ?? (instance = new GameManager()); } }

    private GameManager()
    {
        Object.DontDestroyOnLoad(new GameObject("Updater", typeof(Updater)));

        Debug.Log("DO THIS LIKE THE AUDIOMANAGER WITH THE AUTOMATIC LOADING BASED ON NAME.");
        AttackItemPrefabs = new Dictionary<ItemType, GameObject>()
        {
            { ItemType.None, null },
            { ItemType.FakePedestrian, null },
            { ItemType.KittenCannon, null },
            { ItemType.Harpoon, null },
            { ItemType.Shield, null },
        };
    }

    class Updater : MonoBehaviour
    {
        private void Update()
        {
            instance.Update();
        }
    }

    #endregion

    #region properties

    /// <summary>
    /// The car script the player is using
    /// </summary>
    public CarController Player
    { get; set; }

    public Dictionary<ItemType, GameObject> AttackItemPrefabs
    { get; private set; }

    #endregion

    #region private methods

    void Update()
    {
        #region for debugging

        if (Input.GetKeyDown(KeyCode.U))
        {
            Player.GetComponent<HealthScript>().TakeDamage(.3f);
        }

        #endregion
    }

    #endregion
}
