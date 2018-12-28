using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls the in-level world
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class WorldScript : MonoBehaviour
{
    #region Fields

    bool debugMode = false;  // Turn this off for releases

    static WorldScript instance;

    [SerializeField]RectTransform hudParent;
	[SerializeField]AudioClip daySound;
	[SerializeField]AudioClip nightSound;
    [SerializeField]Image healthBar;
    [SerializeField]Image energyBar;
    [SerializeField]Image skyDarkness;
    [SerializeField]Image playerCastleHealth;
    [SerializeField]Image enemyCastleHealth;
    [SerializeField]Image defeatDarkness;
	[SerializeField]SpriteRenderer sky;
    [SerializeField]GameObject victoryText;
    [SerializeField]GameObject loseText;
    [SerializeField]GameObject starrySky;
    [SerializeField]GameObject rangerHUD;
    [SerializeField]GameObject mageHUD;
    [SerializeField]GameObject warriorHUD;
    [SerializeField]Vector2 playerCastleLocation;
    [SerializeField]GameObject[] cloudPrefabs;

    Timer defeatDarknessTimer;
    Timer skyTimer;
    GameObject player;
    GameObject[] parallaxBackgrounds;
    Vector3 playerLocation;
    Dictionary<SkyStateType, SkyState> skyStates;
    SkyStateType currSkyState;
	AudioSource BGM;

	int[] levels = new int[Constants.MAP_LENGTH];
	float elevationWeight = 1;
	float heightDifferenceWeight = 1;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the static reference to the world script
    /// </summary>
    public static WorldScript Instance
    { get { return instance; } }

    #endregion

    #region Public Methods

    /// <summary>
    /// Handles the side with the given tag being defeated
    /// </summary>
    /// <param name="tag">the tag of the object</param>
    public void Defeat(string tag)
    {
        if (!victoryText.activeSelf && !loseText.activeSelf)
        {
            if (tag == Constants.ENEMY_TAG)
            { 
                victoryText.SetActive(true);
                if (GameManager.Instance.CurrentSave.CurrentKingdom < Constants.NUM_KINGDOMS &&
                    GameManager.Instance.CurrentLoadedKingdom == GameManager.Instance.CurrentSave.Kingdoms[GameManager.Instance.CurrentSave.CurrentKingdom])
                { 
                    GameManager.Instance.CurrentSave.CurrentKingdom++;
                    GameManager.Instance.Save();
                }
            }
            else
            { loseText.SetActive(true); }
            defeatDarknessTimer.Start();
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    private void Start()
    {
        // SEED IS HARDCODED IN DEBUG MODE
        if (debugMode)
        { Random.seed = 71; }

        instance = this;

        // Sets up the sky state dictionary
        currSkyState = SkyStateType.Day;
        skyStates = new Dictionary<SkyStateType, SkyState>();
        skyStates.Add(SkyStateType.Dawn, new SkyState(SkyStateType.DawnToDay, Constants.ORANGE_SKY_COLOR, Constants.ORANGE_SKY_COLOR, Constants.CYCLE_TIME * 
            Constants.ORANGE_TIME_PCT, Constants.ORANGE_DARKNESS_COLOR, Constants.ORANGE_DARKNESS_COLOR, Constants.BGM_MAX_VOLUME, 0, daySound));
        skyStates.Add(SkyStateType.DawnToDay, new SkyState(SkyStateType.Day, Constants.ORANGE_SKY_COLOR, Constants.DAY_SKY_COLOR, Constants.CYCLE_TIME * 
            Constants.FADE_TIME_PCT, Constants.ORANGE_DARKNESS_COLOR, Constants.DAY_DARKNESS_COLOR, 0, Constants.BGM_MAX_VOLUME));
        skyStates.Add(SkyStateType.Day, new SkyState(SkyStateType.DayToDusk, Constants.DAY_SKY_COLOR, Constants.DAY_SKY_COLOR, Constants.CYCLE_TIME * 
            Constants.DAY_TIME_PCT, Constants.DAY_DARKNESS_COLOR, Constants.DAY_DARKNESS_COLOR));
        skyStates.Add(SkyStateType.DayToDusk, new SkyState(SkyStateType.Dusk, Constants.DAY_SKY_COLOR, Constants.ORANGE_SKY_COLOR, Constants.CYCLE_TIME * 
            Constants.FADE_TIME_PCT, Constants.DAY_DARKNESS_COLOR, Constants.ORANGE_DARKNESS_COLOR));
        skyStates.Add(SkyStateType.Dusk, new SkyState(SkyStateType.DuskToNight, Constants.ORANGE_SKY_COLOR, Constants.ORANGE_SKY_COLOR, Constants.CYCLE_TIME * 
            Constants.ORANGE_TIME_PCT, Constants.ORANGE_DARKNESS_COLOR, Constants.ORANGE_DARKNESS_COLOR, Constants.BGM_MAX_VOLUME, 0, nightSound));
        skyStates.Add(SkyStateType.DuskToNight, new SkyState(SkyStateType.Night, Constants.ORANGE_SKY_COLOR, Constants.NIGHT_SKY_COLOR, Constants.CYCLE_TIME *
            Constants.ORANGE_TIME_PCT, Constants.ORANGE_DARKNESS_COLOR, Constants.NIGHT_DARKNESS_COLOR, 0, Constants.BGM_MAX_VOLUME));
        skyStates.Add(SkyStateType.Night, new SkyState(SkyStateType.NightToDawn, Constants.NIGHT_SKY_COLOR, Constants.NIGHT_SKY_COLOR, Constants.CYCLE_TIME * 
            Constants.NIGHT_TIME_PCT, Constants.NIGHT_DARKNESS_COLOR, Constants.NIGHT_DARKNESS_COLOR));
        skyStates.Add(SkyStateType.NightToDawn, new SkyState(SkyStateType.Dawn, Constants.NIGHT_SKY_COLOR, Constants.ORANGE_SKY_COLOR, Constants.CYCLE_TIME * 
            Constants.FADE_TIME_PCT, Constants.NIGHT_DARKNESS_COLOR, Constants.ORANGE_DARKNESS_COLOR));

        // Starts the sky timer
        skyTimer = new Timer(skyStates[currSkyState].TimeInState);
        skyTimer.Register(SkyTimerFinished);
        skyTimer.Start();

        parallaxBackgrounds = GameObject.FindGameObjectsWithTag(Constants.PARALLAX_BACKGROUND_TAG);
        starrySky.transform.rotation = Quaternion.Euler(0, 0, Constants.SKY_START_ROT);
        
        BGM = GetComponent<AudioSource>();
        BGM.volume = Constants.BGM_MAX_VOLUME;

        elevationWeight = Constants.ELEVATION_CHANGE_WEIGHT + Random.Range(-Constants.ELEVATION_CHANGE_OFFSET, Constants.ELEVATION_CHANGE_OFFSET); ;
        heightDifferenceWeight = Constants.HEIGHT_DIFFERENCE_WEIGHT + Random.Range(-Constants.HEIGHT_DIFFERENCE_OFFSET, Constants.HEIGHT_DIFFERENCE_OFFSET);

        if (debugMode)
        {
            Debug.Log("Elevation weight: " + elevationWeight);
            Debug.Log("Height Difference Weight: " + heightDifferenceWeight);
        }

        // Generates the map
        GenerateTerrain();
        GenerateParallaxObjects();

        // Spawns castles
        int enemyCastlePos = levels.Length - (Constants.PLATFORM_LENGTH - 4);
        Instantiate(GameManager.Instance.CastlePrefabs[GameManager.Instance.CurrentLoadedKingdom]).GetComponent<CastleScript>().Initialize(enemyCastleHealth,
            Constants.ENEMY_TAG, new Vector2(enemyCastlePos, levels[enemyCastlePos] + 1));
        Instantiate(GameManager.Instance.CastlePrefabs[GameManager.Instance.GetPrevKingdom()]).GetComponent<CastleScript>().Initialize(playerCastleHealth, 
            Constants.PLAYER_TAG, playerCastleLocation);

        // Sets up the defeat darkness
        defeatDarknessTimer = new Timer(Constants.DARKNESS_TIMER);
        defeatDarknessTimer.Register(DefeatDarknessTimerFinished);

        // Creates the player and HUD
        player = (GameObject)Instantiate(GameManager.Instance.PlayerPrefabs[GameManager.Instance.CurrentSave.PlayerType],
            playerCastleLocation, transform.rotation);
        GameObject hud;
        switch (GameManager.Instance.CurrentSave.PlayerType)
        {
            case CharacterType.Ranger:
                hud = Instantiate<GameObject>(rangerHUD);
                break;
            case CharacterType.Mage:
                hud = Instantiate<GameObject>(mageHUD);
                break;
            case CharacterType.Warrior:
                hud = Instantiate<GameObject>(warriorHUD);
                break;
            default:
                hud = Instantiate<GameObject>(rangerHUD);
                break;
        }
        hud.transform.SetParent(hudParent, false);
        HUDScript hudScript = hud.GetComponent<HUDScript>();
        player.GetComponent<PlayerScript>().Initialize(healthBar, energyBar, hudScript.GcdBars, hudScript.TimerBars, hudScript.SecondaryCDBar,
            hudScript.PowerCDBar, hudScript.SpecialCDBar);
        energyBar.color = player.GetComponent<CharacterScript>().EnergyColor;
        playerLocation = player.transform.position;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        if (!GameManager.Instance.Paused)
        {
            // Updates the parallax backgrounds
            if (playerLocation != player.transform.position)
            {
                float xDirection = Mathf.Sign(player.transform.position.x - playerLocation.x);
                for (int i = 0; i < parallaxBackgrounds.Length; i++)
                {
                    parallaxBackgrounds[i].transform.position = new Vector2(parallaxBackgrounds[i].transform.position.x +
                        (Constants.PARALLAX_SCALE * Constants.PARALLAX_LEVELS[i] * xDirection), 6);
                }
                playerLocation = player.transform.position;
            }

            // Updates the sky
            skyTimer.Update();
            starrySky.transform.Rotate(0, 0, -360 * (Time.deltaTime / Constants.CYCLE_TIME));
            float timePct = skyTimer.ElapsedSeconds / skyTimer.TotalSeconds;
            skyDarkness.color = Color.Lerp(skyStates[currSkyState].StartDarkness, skyStates[currSkyState].EndDarkness, timePct);
            sky.color = Color.Lerp(skyStates[currSkyState].StartSkyColor, skyStates[currSkyState].EndSkyColor, timePct);
            BGM.volume = Mathf.Lerp(skyStates[currSkyState].StartVolume, skyStates[currSkyState].EndVolume, timePct);

            // Updates the defeat darkness
            if (defeatDarknessTimer.IsRunning)
            {
                defeatDarknessTimer.Update();
                defeatDarkness.color = new Color(0, 0, 0, defeatDarknessTimer.ElapsedSeconds / defeatDarknessTimer.TotalSeconds);
            }
        }
    }

    /// <summary>
    /// Generates the parallax objects for the map
    /// </summary>
    private void GenerateParallaxObjects()
    {
        float horizontalPosition = 0;
        float verticalPosition = 0;

        // Generates clouds on the backgrounds
        GameObject[] parallaxBackgrounds = GameObject.FindGameObjectsWithTag(Constants.PARALLAX_BACKGROUND_TAG);
        foreach (GameObject bg in parallaxBackgrounds)
        {
            for (int i = 0; i < (int)(Constants.MAP_LENGTH * Constants.CLOUD_DENSITY); i++)
            {
                horizontalPosition = Random.Range(0, Constants.MAP_LENGTH);
                verticalPosition = Random.Range((float)levels[(int)horizontalPosition] + Constants.CLOUD_HEIGHT_MIN,
                    (float)levels[(int)horizontalPosition] + Constants.CLOUD_HEIGHT_MAX);
                GameObject newObject = Instantiate(cloudPrefabs[Random.Range(0, cloudPrefabs.Length)]) as GameObject;
                newObject.transform.SetParent(bg.transform);
                newObject.transform.position = new Vector3(horizontalPosition, verticalPosition, 2);
                newObject.transform.localScale *= Random.Range(Constants.CLOUD_SCALE_MIN, Constants.CLOUD_SCALE_MAX);
            }
        }
    }

    /// <summary>
    /// Randomly generates the level terrain
    /// </summary>
    private void GenerateTerrain()
    {
        // Fill array: Left platform, middle section, right platform
        int mapLengthMinusPlatform = Constants.MAP_LENGTH - Constants.PLATFORM_LENGTH;
        for (int i = 0; i < Constants.PLATFORM_LENGTH; i++)
        { levels[i] = Constants.BASE_LEVEL; }
        for (int i = Constants.PLATFORM_LENGTH; i < mapLengthMinusPlatform; i++)
        { levels[i] = NextHeight(levels[i - 1]); }
        for (int i = mapLengthMinusPlatform; i < Constants.MAP_LENGTH; i++)
        { levels[i] = levels[mapLengthMinusPlatform - 1]; }

        // Create blocks from array
        GameObject groundPrefab = Resources.Load<GameObject>(Constants.PREFAB_FOLDER + Constants.GROUND_PREFAB);
        GameObject groundUnderPrefab = Resources.Load<GameObject>(Constants.PREFAB_FOLDER + Constants.GROUND_UNDER_PREFAB);
        for (int i = 0; i < levels.Length; i++)
        {
            GameObject newObject = Instantiate(groundPrefab) as GameObject;
            newObject.transform.position = new Vector2(i, levels[i]);

            //draws the blocks under the top
            for (int j = 1; j <= Constants.SOIL_HEIGHT; j++)
            {
                newObject = Instantiate(groundUnderPrefab) as GameObject;
                newObject.transform.position = new Vector2(i, levels[i] - j);
            }
        }
    }

    /// <summary>
    /// Gets the next height value based on the previous height, the weight, and randomness
    /// </summary>
    /// <param name="previous">the previous height</param>
    /// <returns>the next height</returns>
    private int NextHeight(int previous)
    {
        //use weight to decide if we should change direction or not.
        if (Random.Range(0.00f, 1.00f) <= elevationWeight)
        { return previous - (int)Mathf.Sign(Random.Range(0.00f, 1.00f) - heightDifferenceWeight); }
        else
        { return previous; }
    }

    /// <summary>
    /// Handles the sky timer finishing
    /// </summary>
    private void SkyTimerFinished()
    {
        // Changes background sound if needed
        if (skyStates[currSkyState].EndAudio != null)
        {
            BGM.Stop();
            BGM.clip = skyStates[currSkyState].EndAudio;
            BGM.Play();
        }

        // Switches state and restarts timer
        currSkyState = skyStates[currSkyState].NextState;
        skyTimer.TotalSeconds = skyStates[currSkyState].TimeInState;
        skyTimer.Start();
    }

    /// <summary>
    /// Handles the defeat darkness timer finishing
    /// </summary>
    private void DefeatDarknessTimerFinished()
    {
        Application.LoadLevel(Constants.MAP_SCENE);
    }

    #endregion
}
