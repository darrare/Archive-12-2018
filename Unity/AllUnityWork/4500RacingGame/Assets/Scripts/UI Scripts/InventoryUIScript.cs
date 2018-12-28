using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIScript : MonoBehaviour
{
    Image imageIcon;

    private void Start()
    {
        UIManager.Instance.InventoryUI = this;
        imageIcon = GetComponent<Image>();
    }


    public void UpdateIventoryIcon(Sprite sprite)
    {
        imageIcon.sprite = sprite;
    }
}
