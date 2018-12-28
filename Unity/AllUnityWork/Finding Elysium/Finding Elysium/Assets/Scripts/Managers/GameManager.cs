using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GameManager
{
    static GameManager instance;

    bool paused = false;

    /// <summary>
    /// Get the dictionary of runtime-loaded prefabs
    /// </summary>
    public Dictionary<string, GameObject> RuntimePrefabs { get; private set; }

    /// <summary>
    /// The list of all the planets available
    /// </summary>
    public List<Planet> Planets { get; private set; }

    /// <summary>
    /// The player controlled ship
    /// </summary>
    public ShipController Ship
    { get; private set; }

    /// <summary>
    /// Gets or sets whether or not the game is paused
    /// </summary>
    public bool Paused
    {
        get { return paused; }
        set
        {
            paused = value;

            // Pauses/unpauses game objects
            foreach (PauseableObjectScript obj in Object.FindObjectsOfType<PauseableObjectScript>())
            {
                obj.Paused = value;
            }
        }
    }

    /// <summary>
    /// Singleton for the GameManager class
    /// </summary>
    public static GameManager Instance
    {
        get { return instance ?? (instance = new GameManager()); }
    }

    private GameManager()
    {
        Object.DontDestroyOnLoad(new GameObject("GameManagerUpdater", typeof(Updater)));

        RuntimePrefabs = Resources.LoadAll<GameObject>(Constants.RUNTIME_PREFABS_LOCATION).ToDictionary(p => p.name);
        Planets = new List<Planet>();
    }

    /// <summary>
    /// Initializes the game
    /// </summary>
    public void StartNewGame()
    {
        SpawnShip();
        GenerateBackground();
        GeneratePlanets();
    }

    private void Update()
    {
        if (!paused)
        {
            
        }
    }


    private void SpawnShip()
    {
        GameObject shipObj = Object.Instantiate(RuntimePrefabs["Ship"], null) as GameObject;
        Ship = shipObj.GetComponent<ShipController>();
    }

    private void GenerateBackground()
    {
        Transform seamlessBackgroundParent = GameObject.Find("SeamlessBackground").transform;
        int offset = 10;
        for (int i = -offset; i < offset; i++)
        {
            for (int j = -offset; j < offset; j++)
            {
                GameObject newBackground = Object.Instantiate(RuntimePrefabs["seamlessStarBackground"], seamlessBackgroundParent, false) as GameObject;
                newBackground.transform.position = new Vector3(newBackground.GetComponent<SpriteRenderer>().sprite.texture.width / newBackground.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit * i * 5,
                    newBackground.GetComponent<SpriteRenderer>().sprite.texture.height / newBackground.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit * j * 5,
                    seamlessBackgroundParent.position.z);
            }
        }
    }

    private void GeneratePlanets()
    {
        Vector3 planetSpawnInit = new Vector3(1000, 1000, 0);
        Vector3 spawnLocation = new Vector3();
     
        for (int i = 0; i < Constants.PLANETS_TO_SPAWN; i++)
        {
            GameObject planetObj = Object.Instantiate(RuntimePrefabs["Planet"], planetSpawnInit, Quaternion.identity) as GameObject;
            Planet planet = planetObj.GetComponent<Planet>();
            planet.Initialize();
            do
            {
                spawnLocation = new Vector3(Random.Range(-Constants.PLANET_SPAWN_DISTANCE_MAX, Constants.PLANET_SPAWN_DISTANCE_MAX), Random.Range(-Constants.PLANET_SPAWN_DISTANCE_MAX, Constants.PLANET_SPAWN_DISTANCE_MAX), 0);
            } while (Vector3.Distance(spawnLocation, Vector3.zero) > Constants.PLANET_SPAWN_DISTANCE_MAX || Planets.Any(t => Vector3.Distance(t.transform.position, spawnLocation) <= Constants.PLANET_MIN_DISTANCE_FROM_OTHER_PLANET + t.Radius * t.Scale + planet.Radius * planet.Scale));
            planetObj.transform.position = spawnLocation;
            Planets.Add(planet.GetComponent<Planet>());
        }
    }



    #region Updater class

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

    #endregion
}
