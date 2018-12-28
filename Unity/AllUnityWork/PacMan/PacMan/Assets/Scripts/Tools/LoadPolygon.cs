using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// This class is temporary. I need to eventually fit it with error catching and handling.
/// </summary>
public class LoadPolygon : MonoBehaviour {

    [SerializeField]
    PolygonCollider2D polygonCollider;


    /// <summary>
    /// Right click on this script in the inspector to sort the array (only needs to be done once)
    /// </summary>
    [ContextMenu("Load in polygon")]
    private void LoadPolygonIntoUnity()
    {
        SerializablePolygon poly;
        IFormatter formatter = new BinaryFormatter();
        using (Stream stream = new FileStream("MostRecentSave.data", FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            poly = (SerializablePolygon)formatter.Deserialize(stream);
        }

        int index = 0;
        polygonCollider.pathCount = 0;
        foreach (Vector2[] v in poly.Paths)
        {
            polygonCollider.pathCount = index + 1;
            polygonCollider.SetPath(index, v);
            index++;
        }
    }
}
