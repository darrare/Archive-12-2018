using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager
{
    #region singleton stuff
    static UIManager instance;

    /// <summary>
    /// Singleton
    /// </summary>
    public static UIManager Instance
    { get { return instance ?? (instance = new UIManager()); } }

    private UIManager()
    {
        ItemSprites = new Dictionary<ItemType, Sprite>()
        {
            { ItemType.None, Resources.Load<Sprite>("Art/NoneItem") },
            { ItemType.KittenCannon, Resources.Load<Sprite>("Art/KittenCannonItem") },
            { ItemType.Harpoon, Resources.Load<Sprite>("Art/HarpoonItem") },
            { ItemType.FakePedestrian, Resources.Load<Sprite>("Art/FakePedestrianItem") },
            { ItemType.Shield, Resources.Load<Sprite>("Art/ShieldItem") },
        };
    }
    #endregion

    #region properties

    public InventoryUIScript InventoryUI
    { get; set; }

    public HealthUIScript HealthUI
    { get; set; }



    #endregion

    #region fields

    Dictionary<ItemType, Sprite> ItemSprites;

    #endregion

    #region Public methods

    public void UpdateInventoryIcon(ItemType item)
    {
        if (InventoryUI)
        {
            InventoryUI.UpdateIventoryIcon(ItemSprites[item]);
        }
    }

    public void UpdateHealthBar(float percentage)
    {
        if (HealthUI)
        {
            HealthUI.UpdateHealthBar(percentage);
        }
    }

    #endregion
}
