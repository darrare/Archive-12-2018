using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Manager
{
    static Manager instance;
    Dictionary<BodyPart, MoveWhenActivated> thingsToMove = new Dictionary<BodyPart, MoveWhenActivated>();

    /// <summary>
    /// Singleton for the manager
    /// </summary>
    public static Manager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Manager();
            }
            return instance;
        }
    }

    public Manager()
    {
        Object.DontDestroyOnLoad(new GameObject("Manager", typeof(Updater)));
    }

    public Dictionary<BodyPart, MoveWhenActivated> ThingsToMove
    {
        get { return thingsToMove; }
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void Start()
    {
        foreach (KeyValuePair<BodyPart, MoveWhenActivated> obj in thingsToMove)
        {
            if (obj.Value.Timer != 0)
            {
                return;
            }
            obj.Value.IsStarted = true;
        }
    }

    public void BackToNormal()
    {
        foreach (KeyValuePair<BodyPart, MoveWhenActivated> obj in thingsToMove)
        {
            if (obj.Value.Timer != 0)
            {
                return;
            }
            obj.Value.IsReturning = true;
        }

    }

    /// <summary>
    /// Internal class that updates the game manager
    /// </summary>
    class Updater : MonoBehaviour
    {
        /// <summary>
        /// Update is called once per frame
        /// </summary>
        private void Update()
        {
            Instance.Update();
        }
    }
}

public enum BodyPart
{
    FRleg,
    FLleg,
    BRleg,
    BLleg,
    Butt,
    Torso,
    Belly,
    Head,

}
