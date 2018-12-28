using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Enumeration for the different sky states
/// </summary>
public enum SkyStateType
{
    Day,
    DayToDusk,
    Dusk,
    DuskToNight,
    Night,
    NightToDawn,
    Dawn,
    DawnToDay
}
