using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIScript : MonoBehaviour {

    Image healthBar;

    private void Start()
    {
        UIManager.Instance.HealthUI = this;
        healthBar = GetComponent<Image>();
    }


    /// <summary>
    /// Updates the healthbar to represent player health
    /// </summary>
    /// <param name="percentage">The percent the bar should be (curHealth / healthMaximum)</param>
    public void UpdateHealthBar(float percentage)
    {
        healthBar.fillAmount = percentage;
    }
}
