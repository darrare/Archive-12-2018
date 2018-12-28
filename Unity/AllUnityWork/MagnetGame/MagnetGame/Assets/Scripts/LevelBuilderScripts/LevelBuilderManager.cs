using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using System.Linq;

public enum LevelObjectType
{
    Error, //Something wrong happened, the string name didn't match the enumeration.ToString().
    Objective,
    Block,
};

public class LevelBuilderManager
{
    #region fields

    static LevelBuilderManager instance;

    #endregion

    #region singleton, constructor, and initializer

    /// <summary>
    /// Singleton for the LevelBuilderManager class
    /// </summary>
    public static LevelBuilderManager Instance
    { get { return (instance ?? (instance = new LevelBuilderManager())); } }

    /// <summary>
    /// Private constructor for the game manager
    /// </summary>
    private LevelBuilderManager()
    {
        LevelObjects = new List<LevelBuilderObject>();

        //Loads all of the prefabs into the dictionary so we can use them to build the level.
        //Each prefab name MUST match the name of the enumeration for this to work.
        LevelBuilderObjects = Resources.LoadAll<GameObject>("Prefabs/LevelBuilderPrefabs").ToDictionary(t =>
        {
            foreach (LevelObjectType y in Enum.GetValues(typeof(LevelObjectType)))
            {
                if (y.ToString() == t.name)
                {
                    return y;
                }
            }
            return LevelObjectType.Error;
        });

        //Handle the levelNames list
        LevelNames = new List<string>();
#if UNITY_IOS || UNITY_ANDROID
        //if the first run ever, create the binary names file
        try
        {
            if (!File.Exists(Application.persistentDataPath + @"\LevelNames.Names"))
            {
                SaveLevelNames(Application.persistentDataPath + @"\LevelNames.Names");
            }
            else //otherwise, load in the level names so that we can use it
            {
                LoadLevelNames(Application.persistentDataPath + @"\LevelNames.Names");
            }
        }
        catch(Exception e)
        {
            Debug.Log("If you are getting this message it means that you are trying to run the android build on windows.");
        }

#else
        //if the first run ever, create the binary names file
        if (!File.Exists(@"\LevelNames.Names"))
        {
            SaveLevelNames(@"\LevelNames.Names");
        }
        else //otherwise, load in the level names so that we can use it
        {
            LoadLevelNames(@"\LevelNames.Names");
        }
#endif
    }

    #endregion

    #region properties

    /// <summary>
    /// The names of the levels currently saved on the device
    /// </summary>
    public List<string> LevelNames
    { get; private set; }

    /// <summary>
    /// The main camera in the level builder
    /// </summary>
    public LevelBuilderCameraScript MainCam
    { get; set; }

    /// <summary>
    /// The levels data, serializable.
    /// </summary>
    public List<LevelBuilderObject> LevelObjects
    { get; private set; }

    /// <summary>
    /// A dictionary of possible prefabs that can be put into the scene
    /// </summary>
    public Dictionary<LevelObjectType, GameObject> LevelBuilderObjects
    { get; private set; }

    #endregion

    #region public methods

    /// <summary>
    /// Save the level names data structure as a binary on the device
    /// </summary>
    /// <param name="path">The path (so we can do it differently based on platform)</param>
    public void SaveLevelNames(string path)
    {
        IFormatter formatter = new BinaryFormatter();
        using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            formatter.Serialize(stream, LevelNames);
        }
    }

    /// <summary>
    /// Load the level names data structure from the device
    /// </summary>
    /// <param name="path">The path (so we can do it differently based on platform)</param>
    public void LoadLevelNames(string path)
    {
        IFormatter formatter = new BinaryFormatter();
        using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            LevelNames = (List<string>)formatter.Deserialize(stream);
        }
    }

    //This method was fucky, so i deleted it but it is good for reference I guess
//    /// <summary>
//    /// Gets a list of the files of a specific extention type in a location.
//    /// </summary>
//    /// <param name = "location" > The location we want to search (Does not get used on mobile devices)</param>
//    /// <param name = "extension" > The extention we want a list of.</param>
//    /// <returns>A string array containing the names of the files</returns>
//    public string[] GetFilesOfSpecificExtention(string location, string extension)
//    {
//        string[] result = null;
//        try
//        {
//#if UNITY_IOS || UNITY_ANDROID
//            result = Directory.GetFiles(Application.persistentDataPath, "*" + extension, SearchOption.TopDirectoryOnly);
//            for (int i = 0; i < result.Length; i++)
//            {
//                string[] temp = result[i].Split(@"\"[0], '.');
//                result[i] = temp[temp.Length - 2];
//                UIManager.Instance.LevelBuilderUI.debugText.text += "\n " + result[i];
//            }
//#else
//            result = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\" + location, "*" + extension, SearchOption.TopDirectoryOnly);
//            for (int i = 0; i < result.Length; i++)
//            {
//                string[] temp = result[i].Split(@"\"[0], '.');
//                result[i] = temp[temp.Length - 2];
//            }
//#endif
//        }
//        catch (Exception e)
//        {
//            UIManager.Instance.LevelBuilderUI.debugText.text += "\n " + e;
//        }

//        return result;
//    }

    /// <summary>
    /// Serializes a list of level builder objects so that we can deserialize it into a level.
    /// </summary>
    /// <param name="list">The contents of the level</param>
    /// <param name="name">The name of the level</param>
    /// <param name="creatorName">The name of the creator of the level</param>
    public void SerializeListOfLevelBuilderObject(List<LevelBuilderObject> list, string name, string creatorName)
    {
        try
        {
            LevelData level = new LevelData(list, name, creatorName);
            IFormatter formatter = new BinaryFormatter();
#if UNITY_IOS || UNITY_ANDROID
            using (Stream stream = new FileStream(Application.persistentDataPath + @"\" + name + ".LevelData", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, level);
            }
            if (!LevelNames.Contains(name))
            {
                LevelNames.Add(name);
                SaveLevelNames(Application.persistentDataPath + @"\LevelNames.Names");
            }
#else
            using (Stream stream = new FileStream(name + ".LevelData", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, level);
            }
            if (!LevelNames.Contains(name))
            {
                LevelNames.Add(name);
                SaveLevelNames(@"\LevelNames.Names");
            }
#endif
        }
        catch (Exception e)
        {
            UIManager.Instance.LevelBuilderUI.debugText.text += "\n " + e;
        }

    }

    /// <summary>
    /// Deserialize a binary file into a LevelData object.
    /// </summary>
    /// <param name="levelName">Name of the level, not including .LevelData</param>
    /// <returns>The leveldata for the level we want to build</returns>
    public LevelData DeserializeLevelData(string levelName)
    {
        try
        {
            IFormatter formatter = new BinaryFormatter();
#if UNITY_IOS || UNITY_ANDROID
            using (Stream stream = new FileStream(Application.persistentDataPath + @"\" + levelName + ".LevelData", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return (LevelData)formatter.Deserialize(stream);
            }
#else
            using (Stream stream = new FileStream(levelName + ".LevelData", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return (LevelData)formatter.Deserialize(stream);
            }
#endif
        }
        catch (Exception e) //TODO: Test to see if this actually works or not. I have absolutely no idea.
        {
            Debug.Log(e);
            return null;
        }

    }

#endregion

#region private methods

#endregion

}

/// <summary>
/// An entire container for the specifications of a level
/// </summary>
[Serializable]
public class LevelData
{
    /// <summary>
    /// The contents of the level.
    /// </summary>
    public List<LevelBuilderObject> Data
    { get; private set; }

    /// <summary>
    /// The name of the level
    /// </summary>
    public string LevelName
    { get; private set; }

    /// <summary>
    /// The name of the creator of the level.
    /// </summary>
    public string LevelCreator
    { get; private set; }

    /// <summary>
    /// Constructor for the level data class. Will probably need to add more parameters to this as we go.
    /// </summary>
    /// <param name="data">The objects in the level</param>
    /// <param name="levelName">The name of the level.</param>
    /// <param name="levelCreator">The name of the creator of the level</param>
    public LevelData(List<LevelBuilderObject> data, string levelName, string levelCreator)
    {
        Data = data;
        LevelName = LevelName;
        LevelCreator = LevelCreator;
    }
}

/// <summary>
/// An object that is in the scene. This needs to be serializable
/// </summary>
[Serializable]
public class LevelBuilderObject
{
    Vec3 position;
    LevelObjectType type;

    /// <summary>
    /// Creates a serializable data structure that can be used to represent the data in the level
    /// </summary>
    /// <param name="position">Position of the object</param>
    /// <param name="type">Type of the object we are going to spawn</param>
    public LevelBuilderObject(Vector3 position, LevelObjectType type)
    {
        this.position = new Vec3(position.x, position.y, position.z);
        this.type = type;
    }

    /// <summary>
    /// The position of the game object
    /// </summary>
    public Vector3 Position
    { get { return position.ToVector3; } }

    /// <summary>
    /// Gets the type of this levelbuilderobject
    /// </summary>
    public LevelObjectType Type
    { get { return type; } }
}

/// <summary>
/// Custom vector3 class so that we can serialize it.
/// </summary>
[Serializable]
public class Vec3
{
    public float x;
    public float y;
    public float z;

    /// <summary>
    /// Create a serialized version of a Vector3
    /// </summary>
    /// <param name="x">x value</param>
    /// <param name="y">y value</param>
    /// <param name="z">z value, defaults to 0 if not input</param>
    public Vec3(float x, float y, float z = 0)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    /// <summary>
    /// Converts our Vec3 to a Vector3
    /// </summary>
    public Vector3 ToVector3
    {
        get { return new Vector3(x, y, z); }
    }
}