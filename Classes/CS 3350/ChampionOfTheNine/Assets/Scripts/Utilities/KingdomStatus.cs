using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Class that stores the elements of a kingdom's status that need to be saved
/// </summary>
[Serializable]
public class KingdomStatus
{
    #region Fields


    #endregion

    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    public KingdomStatus(KingdomName name)
    {
        Name = name;
    }

    #endregion

    #region Properties

    public KingdomName Name
    { get; set; }

    #endregion

    #region Public Methods



    #endregion
}
