using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum Ghost { Blinky, Pinky, Inky, Clyde };

public class LevelManager
{
    #region fields

    static LevelManager instance;

    #endregion

    #region properties

    /// <summary>
    /// The current levels graph data.
    /// </summary>
    public GraphData Graph
    { get; set; }

    /// <summary>
    /// A dictionary of all of the little dots
    /// </summary>
    public Dictionary<Vector2, GameObject> SmallDots
    { get; set; }

    /// <summary>
    /// A dictionary of all of the large dots
    /// 
    public Dictionary<Vector2, GameObject> LargeDots
    { get; set; }

    /// <summary>
    /// A dictionary of all of the ghosts
    /// </summary>
    public Dictionary<Ghost, GhostScript> Ghosts
    { get; set; }

    /// <summary>
    /// The current state of the ghosts
    /// </summary>
    public GhostBehaviourState CurrentGhostBehaviour
    { get; set; }

    /// <summary>
    /// The stored state of the ghosts when they go into another mode.
    /// </summary>
    public GhostBehaviourState StoredGhostBehaviour
    { get; set; }

    #endregion

    #region Singleton and constructor

    /// <summary>
    /// Singleton for the levelmanager class
    /// </summary>
    public static LevelManager Instance
    {
        get { return instance ?? (instance = new LevelManager()); }
    }

    /// <summary>
    /// Constructor for the level manager class
    /// </summary>
    private LevelManager()
    {
        SmallDots = new Dictionary<Vector2, GameObject>();
        LargeDots = new Dictionary<Vector2, GameObject>();
        Ghosts = new Dictionary<Ghost, GhostScript>();
        TimerAndEventManager.Instance.RegisterToEvent(EventType.GhostModePursue, SwitchGhostMode);
        TimerAndEventManager.Instance.RegisterToEvent(EventType.GhostModeRun, SwitchGhostMode);
        TimerAndEventManager.Instance.RegisterToEvent(EventType.PowerupGain, SwitchGhostMode);
        TimerAndEventManager.Instance.RegisterToEvent(EventType.PowerupQuicken, SwitchGhostMode);
        TimerAndEventManager.Instance.RegisterToEvent(EventType.PowerupFall, SwitchGhostMode);
    }

    #endregion

    #region public methods

    /// <summary>
    /// Initializes all the entities of the game
    /// </summary>
    public void InitializeGame()
    {
        DrawDots();
        SpawnPacMan();
        SpawnGhosts();
        ReleaseAGhost(Ghost.Blinky);
        ReleaseAGhost(Ghost.Pinky);
    }

    /// <summary>
    /// Updates the level manager
    /// </summary>
    public void Update()
    {

    }

    /// <summary>
    /// Switches the ghost mode
    /// </summary>
    public void SwitchGhostMode(EventType type)
    {
        if (type == EventType.GhostModeRun) //This was the event that just finished, switch back to running
        {
            CurrentGhostBehaviour = GhostBehaviourState.Run;
            StoredGhostBehaviour = GhostBehaviourState.Run;
            foreach (KeyValuePair<Ghost, GhostScript> ghost in Ghosts)
            {
                if (ghost.Value.CurState != GhostBehaviourState.IdleInBase && ghost.Value.CurState != GhostBehaviourState.ReturnToBase && ghost.Value.CurState != GhostBehaviourState.Frightened)
                {
                    ghost.Value.CurState = GhostBehaviourState.Run;
                }
            }
        }
        else if (type == EventType.GhostModePursue) //This was the event that just finished, switch back to pursuing
        {
            CurrentGhostBehaviour = GhostBehaviourState.Chase;
            StoredGhostBehaviour = GhostBehaviourState.Chase;
            foreach (KeyValuePair<Ghost, GhostScript> ghost in Ghosts)
            {
                if (ghost.Value.CurState != GhostBehaviourState.IdleInBase && ghost.Value.CurState != GhostBehaviourState.ReturnToBase && ghost.Value.CurState != GhostBehaviourState.Frightened)
                {
                    ghost.Value.CurState = GhostBehaviourState.Chase;
                }
            }
        }
        else if (type == EventType.PowerupGain)
        {
            CurrentGhostBehaviour = GhostBehaviourState.Frightened;
            TimerAndEventManager.Instance.PauseTimer(EventType.GhostModePursue, EventType.GhostModeRun);
            foreach (KeyValuePair<Ghost, GhostScript> ghost in Ghosts)
            {
                if (ghost.Value.CurState != GhostBehaviourState.IdleInBase && ghost.Value.CurState != GhostBehaviourState.ReturnToBase)
                {
                    ghost.Value.CurState = GhostBehaviourState.Frightened;
                }
            }
        }
        else if (type == EventType.PowerupQuicken)
        {
            CurrentGhostBehaviour = GhostBehaviourState.FrightenedFading;
            TimerAndEventManager.Instance.PauseTimer(EventType.GhostModePursue, EventType.GhostModeRun);
            foreach (KeyValuePair<Ghost, GhostScript> ghost in Ghosts)
            {
                if (ghost.Value.CurState == GhostBehaviourState.Frightened)
                {
                    ghost.Value.CurState = GhostBehaviourState.FrightenedFading;
                }
            }
        }
        else if (type == EventType.PowerupFall)
        {
            CurrentGhostBehaviour = StoredGhostBehaviour;
            TimerAndEventManager.Instance.ResumeTimer(EventType.GhostModePursue, EventType.GhostModeRun);
            foreach (KeyValuePair<Ghost, GhostScript> ghost in Ghosts)
            {
                if (ghost.Value.CurState != GhostBehaviourState.IdleInBase && ghost.Value.CurState != GhostBehaviourState.ReturnToBase)
                {
                    ghost.Value.CurState = CurrentGhostBehaviour;
                }
            }
        }
    }

    /// <summary>
    /// Call whenever a dot is eaten and send its transform.position
    /// </summary>
    /// <param name="location">transform.position of the dot we just ate</param>
    public void EatDot(Vector2 location)
    {
        if (SmallDots.ContainsKey(location))
        {
            Object.Destroy(SmallDots[location]);
            SmallDots.Remove(location);
            AudioManager.Instance.PlayWaka();
            UIManager.Instance.AddScore(10);
            if (SmallDots.Count == 210)
            {
                ReleaseAGhost(Ghost.Inky);
            }
            else if(SmallDots.Count == 168)
            {
                ReleaseAGhost(Ghost.Clyde);
            }
        }
        else if (LargeDots.ContainsKey(location))
        {
            Object.Destroy(LargeDots[location]);
            LargeDots.Remove(location);
            AudioManager.Instance.PlayOneShot(SoundEffect.Powerup);
            TimerAndEventManager.Instance.FireEvent(EventType.PowerupGain);
            UIManager.Instance.AddScore(50);
        }
        else
        {
            AudioManager.Instance.WakaToggle = true;
        }
        
    }

    /// <summary>
    /// Gets whether or not the node is on the declared edge
    /// </summary>
    /// <param name="node">The node we want to check</param>
    /// <param name="dir">The direction we want to check to see if there is something there</param>
    /// <returns>Whether or not the node is on the edge</returns>
    public bool CanWeGoInThisDirection(Node node, Direction dir)
    {
        if (node.Neighbors.ContainsKey(dir) ? (node.Neighbors[dir].Type == NodeType.Available || node.Neighbors[dir].Type == NodeType.Teleporter) : false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region private methods

    /// <summary>
    /// Inky and Clyde stay in the ghost house until 30 dots and 168 dots respectively have been eaten
    /// </summary>
    /// <param name="ghost">The ghost to release</param>
    void ReleaseAGhost(Ghost ghost)
    {
        Ghosts[ghost].CurState = GhostBehaviourState.Chase;
        Debug.Log("Released a ghost");
    }

    /// <summary>
    /// Spawns all of the pelets on to the screen.
    /// </summary>
    void DrawDots()
    {
        foreach (Transform child in GameManager.Instance.DataObject.transform)
        {
            if (child.name == "NormalDot")
            {
                GameObject newDot = Object.Instantiate(GameManager.Instance.RuntimePrefabs["NormalDot"]);
                newDot.transform.SetParent(GameManager.Instance.DataObject.transform, false);
                newDot.transform.position = new Vector2(child.transform.position.x + .5f, child.transform.position.y + .5f);
                newDot.name = newDot.transform.position.x + ", " + newDot.transform.position.y;
                SmallDots.Add(new Vector2(child.transform.position.x + .5f, child.transform.position.y + .5f), newDot);
                Object.Destroy(child.gameObject);
            }
            else if (child.name == "PowerDot")
            {
                GameObject newDot = Object.Instantiate(GameManager.Instance.RuntimePrefabs["LargeDot"]);
                newDot.transform.SetParent(GameManager.Instance.DataObject.transform, false);
                newDot.transform.position = new Vector2(child.transform.position.x + .5f, child.transform.position.y + .5f);
                LargeDots.Add(new Vector2(child.transform.position.x + .5f, child.transform.position.y + .5f), newDot);
                Object.Destroy(child.gameObject);
            }
        }
    }

    /// <summary>
    /// Spawns pacman in the right location.
    /// </summary>
    void SpawnPacMan()
    {
        foreach (Transform child in GameManager.Instance.DataObject.transform)
        {
            if (child.name == "PacManSpawn")
            {
                GameObject pacMan = Object.Instantiate(GameManager.Instance.RuntimePrefabs["PacMan"]);
                pacMan.GetComponent<PacManScript>().Initialize(child.transform.position);
                GameManager.Instance.PacMan = pacMan.GetComponent<PacManScript>();
                break; //break because we only need to find one pac man spawn
            }
        }
    }

    /// <summary>
    /// Spawns the ghosts in their proper locations
    /// </summary>
    void SpawnGhosts()
    {
        bool ranOnce = false;
        List<GameObject> otherGhostSpawn = new List<GameObject>();
        foreach (Transform child in GameManager.Instance.DataObject.transform)
        {
            if (child.name == "BlinkySpawn" && !ranOnce)
            {
                GameObject blinky = Object.Instantiate(GameManager.Instance.RuntimePrefabs["Blinky"]);
                blinky.GetComponent<GhostScript>().Initialize(child.transform.position);
                Ghosts.Add(Ghost.Blinky, blinky.GetComponent<GhostScript>());
                ranOnce = true;
            }
            else if (child.name == "OtherGhostSpawn")
            {
                otherGhostSpawn.Add(child.gameObject);
            }
        }
        GameObject pinky = Object.Instantiate(GameManager.Instance.RuntimePrefabs["Pinky"]);
        pinky.GetComponent<GhostScript>().Initialize(otherGhostSpawn[0].transform.position);
        Ghosts.Add(Ghost.Pinky, pinky.GetComponent<GhostScript>());

        //inky
        GameObject inky = Object.Instantiate(GameManager.Instance.RuntimePrefabs["Inky"]);
        inky.GetComponent<GhostScript>().Initialize(otherGhostSpawn[1].transform.position);
        Ghosts.Add(Ghost.Inky, inky.GetComponent<GhostScript>());

        //clyde
        GameObject clyde = Object.Instantiate(GameManager.Instance.RuntimePrefabs["Clyde"]);
        clyde.GetComponent<GhostScript>().Initialize(otherGhostSpawn[2].transform.position);
        Ghosts.Add(Ghost.Clyde, clyde.GetComponent<GhostScript>());
    }

    #endregion
}
