using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Delagate that represents a significant something in the game
/// in which we need multiple things to react to
/// </summary>
public delegate void GameEvent(EventType type = EventType.None);

/// <summary>
/// This enumeration contains all the possible types of events that will ever be triggered
/// </summary>
public enum EventType
{
    None,
    InitStart,
    GameBegin,
    PowerupGain, //Whenever player picks up a large pelet
    PowerupFall, //Whenever the ghosts resume to their normal states.
    PowerupQuicken, //Whenever the ghosts start blinking faster.
    GhostModeRun, //Whenever ghosts start to go to their corners
    GhostModePursue, //Whenever ghosts start to pursue
    PacManDeath, //whenever pacman dies
    GameReset,
};

/// <summary>
/// Manager for different timers and events throughout the game.
/// </summary>
public class TimerAndEventManager
{
    #region Fields

    static TimerAndEventManager instance;
    
    //Initialize timer list
    List<Timer> timers = new List<Timer>();

    //initialize delegates
    Dictionary<EventType, GameEvent> events = new Dictionary<EventType, GameEvent>();

    #endregion

    #region Constructor

    /// <summary>
    /// Private constructor
    /// </summary>
    private TimerAndEventManager()
    {
        //Any event you intialize should be done by EventManager.Instance.RegisterForEvent();
        //If you need to add a new event type then add it in the enumeration at the top of this file.
        RegisterToEvent(EventType.PowerupGain, PowerupGainTimerStart);
        RegisterToEvent(EventType.PowerupQuicken, PowerupQuickenTimerStart);
        RegisterToEvent(EventType.GhostModeRun, ChangeGhostMode);
        RegisterToEvent(EventType.GhostModePursue, ChangeGhostMode);
        RegisterToEvent(EventType.InitStart, InitializeGameStart);
        RegisterToEvent(EventType.GameBegin, BeginGame);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the singleton instance of the event manager
    /// </summary>
    public static TimerAndEventManager Instance
    {
        get { return instance ?? (instance = new TimerAndEventManager()); }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Updated from the game manager
    /// </summary>
    public void Update()
    {
        //Updates all the timers that are currently running, and removes all that are finished.
        timers.RemoveAll(t =>
        {
            if (t.IsRunning && (GameManager.Instance.IsPaused ? t.RunWhenPaused : true))
            {
                t.Update();
                return false;
            }
            else if (t.IsFinished)
            {
                return true;
            }
            return false;
        });
    }

    #region Event handling methods

    /// <summary>
    /// Registers a method to be triggered whenever an event is fired
    /// </summary>
    /// <param name="eventType">The type of event you want to register the method to</param>
    /// <param name="method">The method that you want called whenever the event is fired</param>
    public void RegisterToEvent(EventType eventType, GameEvent method)
    {
        //if the dictionary already contains the event type
        if (events.ContainsKey(eventType))
        {
            events[eventType] -= method; //Prevents adding the same method twice. 
            events[eventType] += method;
        }
        //if the event type has yet to have something registered to it
        else
        {
            events.Add(eventType, method);
        }
    }

    /// <summary>
    /// Removes a method so that it does not get called whenever the event is fired
    /// </summary>
    /// <param name="eventType">The type of event that you want to remove it from</param>
    /// <param name="method">The method you want to remove from the event</param>
    public void UnregisterFromEvent(EventType eventType, GameEvent method)
    {
        //if the dictionary already contains the event type
        if (events.ContainsKey(eventType))
        {
            events[eventType] -= method;
            if (events[eventType] == null)
            {
                events.Remove(eventType);
            }
        }
        //if the event type has yet to have something registered to it
        else
        {
            Debug.Log("No event of this type has been registered");
        }
    }

    /// <summary>
    /// Fires an event
    /// </summary>
    /// <param name="eventType">The type of event we want to fire</param>
    public void FireEvent(EventType eventType)
    {
        if (events.ContainsKey(eventType))
        {
            try
            {
                //sends the type of event that was just fired to the method so the method knows how to react
                events[eventType](eventType);
            }
            catch (MissingReferenceException e)
            {
                Debug.LogError("You forgot to unregister an object to an event when it got deleted. Double click me and read the comment below...");

                // Somewhere, somehow, an object registered to an event and then the object was deleted without unregistering itself.
                // Unfortunately I don't think it is possible to automatically detect this and remove the method in the script attached to the object you deleted from the event.
                // Because of this unfortunate circumstance, you will need to manually unregister everything you registered on the object as it is deleted.
                // The best practice to do so is as follows:
                //
                // if your script registers to an event, at any given time ever, create a "void OnDestroy()" method. If this script is a monobehaviour this method will automatically be called when it is destroyed.
                // you have to unsubscribe in this "OnDestroy" method to every event this object can possibly subscribe to
                //
                // EX:
                //void Start()
                //{
                //    EventManager.Instance.RegisterToEvent(EventType.PlayerDeath, CalledWhenPlayerDies);
                //    EventManager.Instance.RegisterToEvent(EventType.LevelWin, CalledWhenLevelIsWon);
                //}

                //void OnDestroy()
                //{
                //    EventManager.Instance.UnregisterFromEvent(EventType.PlayerDeath, CalledWhenPlayerDies);
                //    EventManager.Instance.UnregisterFromEvent(EventType.LevelWin, CalledWhenLevelIsWon);
                //}
            }
        }
        else
        {
            Debug.Log("Attempting to fire an event that has nothing registered to it. Did you forget to call EventManager.Instance.RegisterToEvent?");
        }
    }

    /// <summary>
    /// Performs a clean sweep on the event manager so that no events have been registered
    /// </summary>
    public void ClearEvents()
    {
        events.Clear();
    }

    #endregion

    #region Timer handling methods

    /// <summary>
    /// Pauses a timer that is associated with a specific event type
    /// </summary>
    /// <param name="type">The event types we want to pause</param>
    public void PauseTimer(params EventType[] type)
    {
        timers.Where(t =>
        {
            if (type.Contains(t.Type))
            {
                t.PauseTimer(true);
                return true;
            }
            return false;
        });
    }

    /// <summary>
    /// Resumes a timer that was recently paused
    /// </summary>
    /// <param name="type">The event types we want to resume</param>
    public void ResumeTimer(params EventType[] type)
    {
        timers.Where(t =>
        {
            if (type.Contains(t.Type))
            {
                t.PauseTimer(false);
                return true;
            }
            return false;
        });
    }

    /// <summary>
    /// Starts the powerup Gain timer
    /// </summary>
    void PowerupGainTimerStart(EventType type)
    {
        //Cleanup any powerup related timers that are currently in progress so they don't cause the ghosts to return to normal too early.
        timers.RemoveAll(t => t.Type == EventType.PowerupGain || t.Type == EventType.PowerupQuicken || t.Type == EventType.PowerupFall);

        //TODO: Do some calculations here to figure out how long the event shoudl last.
        float time = 5; //5 seconds to test, will need to do TODO: line above to figure how long it really should last.
        timers.Add(new Timer(time, EventType.PowerupQuicken));
    }

    /// <summary>
    /// Starts the powerup quicken timer.
    /// </summary>
    void PowerupQuickenTimerStart(EventType type)
    {
        //TODO: Do some calculations here to figure out how long the event shoudl last.
        float time = 5; //5 seconds to test, will need to do TODO: line above to figure how long it really should last.
        timers.Add(new Timer(time, EventType.PowerupFall));
    }

    /// <summary>
    /// Change the ghost mode and start the timer that is going to switch it back.
    /// </summary>
    /// <param name="type">The type of event that was most recently fired.</param>
    void ChangeGhostMode(EventType type)
    {
        switch (type)
        {
            case EventType.GhostModeRun:
                timers.Add(new Timer(7, EventType.GhostModePursue));
                break;

            case EventType.GhostModePursue:
                timers.Add(new Timer(20, EventType.GhostModeRun));
                break;
        }
    }

    /// <summary>
    /// Fired when the game is initialy started.
    /// </summary>
    /// <param name="type">Type of event</param>
    void InitializeGameStart(EventType type)
    {
        AudioManager.Instance.PlayOneShot(SoundEffect.StartGame);
        timers.Add(new Timer(.5f, EventType.GameBegin, true)); //TODO: Change this back to 4.5 when finished
    }

    /// <summary>
    /// Fired after the intro music is done playing.
    /// </summary>
    /// <param name="type"></param>
    void BeginGame(EventType type)
    {
        GameManager.Instance.IsPaused = false;
        FireEvent(EventType.GhostModePursue);
    }

    #endregion

    #endregion

    #region internal timer class

    /// <summary>
    /// Timer class that fires an event when finished
    /// </summary>
    class Timer
    {
        #region properties

        /// <summary>
        /// Whether or not the timer is running. To modify, use Timer.PauseTimer.
        /// </summary>
        public bool IsRunning
        { get; private set; }

        /// <summary>
        /// The current count on the timer in seconds
        /// </summary>
        public float CurTimer
        { get; set; }
        
        /// <summary>
        /// The time this timer needs to reach before firing the event in seconds
        /// </summary>
        public float GoalTime
        { get; set; }

        /// <summary>
        /// The type of event that will get fired when this timer is finished.
        /// </summary>
        public EventType Type
        { get; set; }

        /// <summary>
        /// whether or not the timer has run its course and triggered its event
        /// </summary>
        public bool IsFinished
        { get; private set; }

        /// <summary>
        /// Whether or not to update when the game is paused.
        /// </summary>
        public bool RunWhenPaused
        { get; set; }
        #endregion

        #region constructors

        /// <summary>
        /// Default constructor for the timer.
        /// </summary>
        public Timer()
        {
            CurTimer = 0;
        }

        /// <summary>
        /// Takes in all the parameters and starts the timer.
        /// </summary>
        /// <param name="time">Time in seconds that you want to have a timer for.</param>
        /// <param name="type">The type of event that gets fired when this is finished.</param>
        /// <param name="runWhenPaused">"Whether or not to Update when GameManager.Instance.IsPaused = true;"</param>
        public Timer(float time, EventType type, bool runWhenPaused = false)
        {
            CurTimer = 0;
            GoalTime = time;
            Type = type;
            RunWhenPaused = runWhenPaused;
            StartTimer();
        }

        #endregion

        #region public methods

        /// <summary>
        /// Updates the timer
        /// </summary>
        public void Update()
        {
            if (IsRunning)
            {
                CurTimer += Time.deltaTime;
                if (CurTimer >= GoalTime)
                {
                    TimerFinished();
                }
            }
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void StartTimer()
        {
            IsRunning = true;
        }

        /// <summary>
        /// Pauses the timer
        /// </summary>
        /// <param name="value">Whether to pause or not to pause</param>
        public void PauseTimer(bool value)
        {
            IsRunning = value;
        }

        /// <summary>
        /// Resets the timer back to zero
        /// </summary>
        /// <param name="pauseTimer">Default: True. Either pauses or does not pause the timer after reset</param>
        public void ResetTimer(bool pauseTimer = true)
        {
            IsRunning = pauseTimer;
            CurTimer = 0;
        }

        #endregion

        #region private methods

        /// <summary>
        /// Called when the timer is finished. This executes the timers primary function.
        /// </summary>
        void TimerFinished()
        {
            Instance.FireEvent(Type);
            IsRunning = false;
            CurTimer = 0;
            IsFinished = true;
        }

        #endregion

    }

    #endregion
}
