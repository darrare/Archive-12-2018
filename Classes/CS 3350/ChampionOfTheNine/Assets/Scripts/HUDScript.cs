using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls the HUD
/// </summary>
public class HUDScript : MonoBehaviour
{
    #region Fields

    [SerializeField]Image[] gcdBars;
    [SerializeField]Image[] timerBars;
    [SerializeField]Image secondaryCDBar;
    [SerializeField]Image powerCDBar;
    [SerializeField]Image specialCDBar;

    #endregion

    #region Properties

    public Image[] GcdBars
    { get { return gcdBars; } }

    public Image[] TimerBars
    { get { return timerBars; } }

    public Image SecondaryCDBar
    { get { return secondaryCDBar; } }

    public Image PowerCDBar
    { get { return powerCDBar; } }
    
    public Image SpecialCDBar
    { get { return specialCDBar; } }

    #endregion
}
