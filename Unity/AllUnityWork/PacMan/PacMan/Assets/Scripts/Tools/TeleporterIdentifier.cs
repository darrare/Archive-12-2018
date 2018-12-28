using UnityEngine;
using System.Collections;

/// <summary>
/// Whenever you have a teleporter, put this object on it and give it the same number in the inspector
/// as the teleporter you want it to connect to.
/// </summary>
public class TeleporterIdentifier : MonoBehaviour {
    [SerializeField]
    int identifier;

    public int Identifier { get { return identifier; } }
}
