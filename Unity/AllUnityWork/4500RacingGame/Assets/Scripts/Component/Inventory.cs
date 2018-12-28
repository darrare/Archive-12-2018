using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public enum ItemType { None, KittenCannon, Harpoon, FakePedestrian, Shield }

public class Inventory : NetworkBehaviour
{
    ItemType itemType;

    delegate void AttackMethod();
    AttackMethod AtkMethod;

    Dictionary<ItemType, Action> AttackMethods;

    /// <summary>
    /// The itemtype stored in the inventory.
    /// </summary>
    public ItemType ItemType
    { get { return itemType; }
      set
        {
            itemType = value;
            AtkMethod = new AttackMethod(AttackMethods[value]);
            UIManager.Instance.UpdateInventoryIcon(value);
        }
    }

	// Use this for initialization
	void Awake () {
        AttackMethods = new Dictionary<ItemType, Action>()
        {
            { ItemType.None, NoneAttack },
            { ItemType.KittenCannon, CmdKittenCannonAttack },
            { ItemType.Harpoon, CmdHarpoonAttack },
            { ItemType.FakePedestrian, CmdFakePedestrianAttack },
            { ItemType.Shield, CmdShieldAttack },
        };
        ItemType = ItemType.None;
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            AtkMethod();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("Hacking in an item");
            PickupItem((ItemType)UnityEngine.Random.Range(1, 5));
        }
	}

    public void PickupItem(ItemType type)
    {
        ItemType = type;
    }

    void NoneAttack()
    {
        ItemType = ItemType.None;
    }

    [Command]
    void CmdKittenCannonAttack()
    {
        ItemType = ItemType.None;
        Debug.Log("Fix me");
    }

    [Command]
    void CmdHarpoonAttack()
    {
        ItemType = ItemType.None;
        //GameObject newItem = Instantiate()
    }

    [Command]
    void CmdFakePedestrianAttack()
    {
        ItemType = ItemType.None;
        Debug.Log("Fix me");
    }

    [Command]
    void CmdShieldAttack()
    {
        ItemType = ItemType.None;
        Debug.Log("Fix me");
    }


}
