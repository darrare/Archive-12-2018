using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Script for initializing the player in a testing scene
/// </summary>
public class TestInitializerScript : MonoBehaviour
{
    [SerializeField]RectTransform hudParent;
    [SerializeField]GameObject hudPrefab;
    [SerializeField]Image healthBar;
    [SerializeField]Image energyBar;
    [SerializeField]PlayerScript player;

	private void Start()
	{
        GameObject hud = Instantiate<GameObject>(hudPrefab);
        hud.transform.SetParent(hudParent, false);
        HUDScript hudScript = hud.GetComponent<HUDScript>();
        player.Initialize(healthBar, energyBar, hudScript.GcdBars, hudScript.TimerBars, hudScript.SecondaryCDBar, hudScript.PowerCDBar, hudScript.SpecialCDBar);
	}
}
