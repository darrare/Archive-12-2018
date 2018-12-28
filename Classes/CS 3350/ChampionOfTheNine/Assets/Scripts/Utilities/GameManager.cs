using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

/// <summary>
/// Singleton class that stores game data
/// </summary>
public class GameManager
{
    #region Fields

    static GameManager instance;
    Dictionary<string, Savegame> saves;
    Dictionary<CharacterType, GameObject> playerPrefabs;
    Dictionary<CharacterType, GameObject> enemyPrefabs;
    Dictionary<KingdomName, GameObject> castlePrefabs;
    Dictionary<string, AudioClip> gameSounds;
    Dictionary<string, GameObject> particles;
    Dictionary<int, string> mouseButtonNames;
    Dictionary<string, string> keyNames;
    string lastSound = "";
    Timer lastSoundTimer;
    Queue<ParticleSystem> activeParticles;
    bool paused = false;
    Timer loadDelayTimer;
    string currLoad = "";
    
    #endregion

    #region Constructor

    /// <summary>
    /// Private constructor
    /// </summary>
    private GameManager()
    {
        GameObject.DontDestroyOnLoad(new GameObject("gmUpdater", typeof(Updater)));

        // Loads the saved levels, if possible
        saves = Serializer.Deserialize<Dictionary<string, Savegame>>(Constants.SAVES_FILE);
        if (saves == null)
        { saves = new Dictionary<string, Savegame>(); }
        else
        {
            if (saves.Count > 0)
            { CurrentSaveName = saves.First().Key; }
        }

        // Loads player prefabs
        playerPrefabs = new Dictionary<CharacterType, GameObject>();
        playerPrefabs.Add(CharacterType.Ranger, Resources.Load<GameObject>(Constants.PREFAB_FOLDER + Constants.RANGER_PLAYER_PREFAB));
        playerPrefabs.Add(CharacterType.Mage, Resources.Load<GameObject>(Constants.PREFAB_FOLDER + Constants.MAGE_PLAYER_PREFAB));
        playerPrefabs.Add(CharacterType.Warrior, Resources.Load<GameObject>(Constants.PREFAB_FOLDER + Constants.WARRIOR_PLAYER_PREFAB));

        // Loads enemy prefabs
        enemyPrefabs = new Dictionary<CharacterType, GameObject>();
        enemyPrefabs.Add(CharacterType.Ranger, Resources.Load<GameObject>(Constants.PREFAB_FOLDER + Constants.RANGER_AI_PREFAB));
        enemyPrefabs.Add(CharacterType.Mage, Resources.Load<GameObject>(Constants.PREFAB_FOLDER + Constants.MAGE_AI_PREFAB));
        enemyPrefabs.Add(CharacterType.Warrior, Resources.Load<GameObject>(Constants.PREFAB_FOLDER + Constants.WARRIOR_AI_PREFAB));

        // Loads castle prefabs
        string folder = Constants.PREFAB_FOLDER + Constants.CASTLE_FOLDER;
        castlePrefabs = new Dictionary<KingdomName, GameObject>();
        castlePrefabs.Add(KingdomName.Asian, Resources.Load<GameObject>(folder + Constants.ASIAN_CASTLE_PREFAB));
        castlePrefabs.Add(KingdomName.Bandit, Resources.Load<GameObject>(folder + Constants.BANDIT_CASTLE_PREFAB));
        castlePrefabs.Add(KingdomName.Crystal, Resources.Load<GameObject>(folder + Constants.CRYSTAL_CASTLE_PREFAB));
        castlePrefabs.Add(KingdomName.Dark, Resources.Load<GameObject>(folder + Constants.DARK_CASTLE_PREFAB));
        castlePrefabs.Add(KingdomName.Desert, Resources.Load<GameObject>(folder + Constants.DESERT_CASTLE_PREFAB));
        castlePrefabs.Add(KingdomName.MedOne, Resources.Load<GameObject>(folder + Constants.MED_ONE_CASTLE_PREFAB));
        castlePrefabs.Add(KingdomName.MedTwo, Resources.Load<GameObject>(folder + Constants.MED_TWO_CASTLE_PREFAB));
        castlePrefabs.Add(KingdomName.Viking, Resources.Load<GameObject>(folder + Constants.VIKING_CASTLE_PREFAB));
        castlePrefabs.Add(KingdomName.Village, Resources.Load<GameObject>(folder + Constants.VILLAGE_CASTLE_PREFAB));

        // Loads the game sounds
        loadDelayTimer = new Timer(0.1f);
        loadDelayTimer.Register(LoadTimerFinished);
        lastSoundTimer = new Timer(0);
        lastSoundTimer.Register(LastSoundTimerFinished);
        gameSounds = new Dictionary<string, AudioClip>();
        AudioClip[] sounds = Resources.LoadAll<AudioClip>(Constants.SND_FOLDER);
        foreach (AudioClip sound in sounds)
        { gameSounds.Add(sound.name, sound); }

        // Loads the particles
        activeParticles = new Queue<ParticleSystem>();
        particles = new Dictionary<string, GameObject>();
        GameObject[] parts = Resources.LoadAll<GameObject>(Constants.PART_FOLDER);
        foreach (GameObject part in parts)
        { particles.Add(part.name, part); }

        // Adds mouse button and key names
        mouseButtonNames = new Dictionary<int, string>();
        mouseButtonNames.Add(0, "Left Mouse Button");
        mouseButtonNames.Add(1, "Right Mouse Button");
        mouseButtonNames.Add(2, "Middle Mouse Button");
        keyNames = new Dictionary<string, string>();
        keyNames.Add(" ", "Spacebar");
        keyNames.Add("\r", "Enter");
        keyNames.Add("\b", "Backspace");
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the singleton instance of the game manager
    /// </summary>
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            { instance = new GameManager(); }

            return instance;
        }
    }

    public Dictionary<string, Savegame> Saves
    { get { return saves; } }

    public Dictionary<CharacterType, GameObject> EnemyPrefabs
    { get { return enemyPrefabs; } }

    public Dictionary<KingdomName, GameObject> CastlePrefabs
    { get { return castlePrefabs; } }

    public KingdomName CurrentLoadedKingdom
    { get; set; }

    public bool HasSaves
    { get { return saves.Count != 0; } }

    /// <summary>
    /// Gets the currently loaded save
    /// </summary>
    public Savegame CurrentSave
    { get { return saves[CurrentSaveName]; } }

    /// <summary>
    /// Gets the special mouse button names dictionary
    /// </summary>
    public Dictionary<int, string> MouseButtonNames
    { get { return mouseButtonNames; } }

    /// <summary>
    /// Gets the special key names dictionary
    /// </summary>
    public Dictionary<string, string> KeyNames
    { get { return keyNames; } }

    /// <summary>
    /// Gets the player prefab dictionary
    /// </summary>
    public Dictionary<CharacterType, GameObject> PlayerPrefabs
    { get { return playerPrefabs; } }

    /// <summary>
    /// Gets the pre-loaded game sounds dictionary
    /// </summary>
    public Dictionary<string, AudioClip> GameSounds
    { get { return gameSounds; } }

    /// <summary>
    /// Gets or sets the current save name
    /// </summary>
    public string CurrentSaveName
    { get; set; }

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
            PauseableObjectScript[] objs = GameObject.FindObjectsOfType<PauseableObjectScript>();
            foreach (PauseableObjectScript obj in objs)
            { obj.Paused = value; }

            // Pauses/unpauses particles
            try
            {
                if (value)
                {
                    foreach (ParticleSystem part in activeParticles)
                    { part.Pause(); }
                }
                else
                {
                    foreach (ParticleSystem part in activeParticles)
                    { part.Play(); }
                }
            }
            catch (Exception) { }
        }
    }

    #endregion

    #region Public Methods

    public KingdomName GetPrevKingdom()
    {
        int i = saves[CurrentSaveName].Kingdoms.IndexOf(CurrentLoadedKingdom);
        if (i == 0)
        { return saves[CurrentSaveName].Kingdoms[Constants.NUM_KINGDOMS - 1]; }
        return saves[CurrentSaveName].Kingdoms[i - 1];
    }

    /// <summary>
    /// Plays a sound with a randomly modified pitch
    /// </summary>
    /// <param name="source">the audio source</param>
    /// <param name="clip">the sound</param>
    public void PlaySoundPitched(AudioSource source, AudioClip clip)
    {
        if (lastSound != clip.name)
        {
            source.pitch = 1 + UnityEngine.Random.Range(-Constants.PITCH_CHANGE, Constants.PITCH_CHANGE);
            source.PlayOneShot(clip);
            lastSoundTimer.TotalSeconds = clip.length / 2;
            lastSoundTimer.Start();
            lastSound = clip.name;
        }
    }

    /// <summary>
    /// Spawns a particle emitter at the given location
    /// </summary>
    /// <param name="name">the name of the particle emitter prefab to spawn</param>
    /// <param name="position">the position at which to spawn the particle emitter</param>
    /// <returns>the particle system game object</returns>
    public GameObject SpawnParticle(string name, Vector3 position)
    {
        GameObject particle = ((GameObject)GameObject.Instantiate(particles[name], position, particles[name].transform.rotation));
        activeParticles.Enqueue(particle.GetComponent<ParticleSystem>());
        return particle;
    }

    /// <summary>
    /// Deletes a savegame
    /// </summary>
    public void DeleteSave()
    {
        saves.Remove(CurrentSaveName);
        Save();
    }

    /// <summary>
    /// Creates a new savegame
    /// </summary>
    public void CreateNewSavegame(CharacterType playerType, AudioSource source = null)
    {
        AddSave(playerType);
        if (saves.Count == 1)
        { LoadLevel(Constants.TUTORIAL_SCENE, source); }
        else
        { LoadLevel(Constants.CHAR_CREATE_SCENE, source); }
    }

    /// <summary>
    /// Adds a new savegame
    /// </summary>
    public void AddSave(CharacterType playerType)
    {
        if (!saves.ContainsKey(CurrentSaveName))
        { saves.Add(CurrentSaveName, new Savegame(playerType)); }
        else
        { saves[CurrentSaveName] = new Savegame(playerType); }
        Save();
    }

    /// <summary>
    /// Saves the current state of the savegames to a file
    /// </summary>
    public void Save()
    {
        Serializer.Serialize(Constants.SAVES_FILE, saves);
    }

    public void LoadGameLevel(KingdomName name, AudioSource source = null)
    {
        CurrentLoadedKingdom = name;
        LoadLevel(Constants.LEVEL_SCENE, source);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="source"></param>
    public void LoadLevel(string name, AudioSource source = null)
    {
        if (source != null)
        { LoadLevelWithSound(name, source); }
        else
        { Application.LoadLevel(name); }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Updates the game manager
    /// </summary>
    private void Update()
    {
        // Checks if the oldest particle should be removed
        if (activeParticles.Count > 0)
        {
            try
            {
                if (!activeParticles.Peek().IsAlive())
                {
                    GameObject.Destroy(activeParticles.Peek().gameObject);
                    activeParticles.Dequeue();
                }
            }
            catch (Exception)
            { activeParticles.Dequeue(); }
        }
        lastSoundTimer.Update();
        loadDelayTimer.Update();
    }

    /// <summary>
    /// Handles the last sound timer finishing
    /// </summary>
    private void LastSoundTimerFinished()
    {
        lastSound = "";
    }

    private void LoadLevelWithSound(string name, AudioSource source)
    {
        source.PlayOneShot(gameSounds[Constants.CLICK_SND]);
        currLoad = name;
        loadDelayTimer.Start();
    }

    private void LoadTimerFinished()
    {
        Application.LoadLevel(currLoad);
    }

    #endregion

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
            GameManager.Instance.Update();
        }
    }

    #endregion
}
