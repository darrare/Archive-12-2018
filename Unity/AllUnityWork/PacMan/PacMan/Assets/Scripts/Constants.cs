using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to store all constant data
/// </summary>
public static class Constants
{
    public const string RUNTIME_PREFABS_LOCATION = "Runtime/";
    public const float TIME_TO_TRAVEL_BETWEEN_NODES = .15f;
    public const float GHOST_SPEED_NORMAL = TIME_TO_TRAVEL_BETWEEN_NODES * 1.15f; //85% of pacmans movement speed
    public const float GHOST_SPEED_FRIGHTENED = TIME_TO_TRAVEL_BETWEEN_NODES * 1.75f; //50% of pacmans movement speed
    public const float GHOST_SPEED_RETURN_TO_BASE = TIME_TO_TRAVEL_BETWEEN_NODES * .75f; //25% faster than pacman

    //Using this for now because I know eventually I'm going to want to fine tune it based on different things.
    public static readonly Dictionary<Ghost, Dictionary<GhostBehaviourState, float>> GHOST_MOVEMENT_SPEEDS = new Dictionary<Ghost, Dictionary<GhostBehaviourState, float>>()
    {
        { Ghost.Blinky, new Dictionary<GhostBehaviourState, float>()
            {
                { GhostBehaviourState.Chase, GHOST_SPEED_NORMAL },
                { GhostBehaviourState.Run, GHOST_SPEED_NORMAL },
                { GhostBehaviourState.ReturnToBase, GHOST_SPEED_RETURN_TO_BASE },
                { GhostBehaviourState.IdleInBase, GHOST_SPEED_NORMAL },
                { GhostBehaviourState.Frightened, GHOST_SPEED_FRIGHTENED },
                { GhostBehaviourState.FrightenedFading, GHOST_SPEED_FRIGHTENED },
            } },
        { Ghost.Pinky, new Dictionary<GhostBehaviourState, float>()
            {
                { GhostBehaviourState.Chase, GHOST_SPEED_NORMAL },
                { GhostBehaviourState.Run, GHOST_SPEED_NORMAL },
                { GhostBehaviourState.ReturnToBase, GHOST_SPEED_RETURN_TO_BASE },
                { GhostBehaviourState.IdleInBase, GHOST_SPEED_NORMAL },
                { GhostBehaviourState.Frightened, GHOST_SPEED_FRIGHTENED },
                { GhostBehaviourState.FrightenedFading, GHOST_SPEED_FRIGHTENED },
            } },
        { Ghost.Inky, new Dictionary<GhostBehaviourState, float>()
            {
                { GhostBehaviourState.Chase, GHOST_SPEED_NORMAL },
                { GhostBehaviourState.Run, GHOST_SPEED_NORMAL },
                { GhostBehaviourState.ReturnToBase, GHOST_SPEED_RETURN_TO_BASE },
                { GhostBehaviourState.IdleInBase, GHOST_SPEED_NORMAL },
                { GhostBehaviourState.Frightened, GHOST_SPEED_FRIGHTENED },
                { GhostBehaviourState.FrightenedFading, GHOST_SPEED_FRIGHTENED },
            } },
        { Ghost.Clyde, new Dictionary<GhostBehaviourState, float>()
            {
                { GhostBehaviourState.Chase, GHOST_SPEED_NORMAL },
                { GhostBehaviourState.Run, GHOST_SPEED_NORMAL },
                { GhostBehaviourState.ReturnToBase, GHOST_SPEED_RETURN_TO_BASE },
                { GhostBehaviourState.IdleInBase, GHOST_SPEED_NORMAL },
                { GhostBehaviourState.Frightened, GHOST_SPEED_FRIGHTENED },
                { GhostBehaviourState.FrightenedFading, GHOST_SPEED_FRIGHTENED },
            } },
    };

    public static readonly Dictionary<Direction, float> PACMAN_ROTATION = new Dictionary<Direction, float>()
    {
        { Direction.Up, 90 },
        { Direction.Left, 180 },
        { Direction.Down, 270 },
        { Direction.Right, 0 },
    };

    public const string AUDIO_FILE_LOCATION = "Audio/";
    public static readonly Dictionary<string, SoundEffect> SOUND_EFFECT_STRINGS = new Dictionary<string, SoundEffect>()
    {
        { "Eating cherry", SoundEffect.Bonus },
        { "Powerup", SoundEffect.Powerup },
        { "Eating Ghosts", SoundEffect.EatGhost },
        { "Extra Life", SoundEffect.ExtraLife },
        { "Intermission", SoundEffect.Intermission },
        { "Pac man dies", SoundEffect.Death },
        { "Siren", SoundEffect.Siren },
        { "Start Game", SoundEffect.StartGame },
        { "Waka1", SoundEffect.Waka1 },
        { "Waka2", SoundEffect.Waka2 },
    };

    public static readonly Dictionary<Ghost, Vector2> GHOST_HOME_LOCATIONS = new Dictionary<Ghost, Vector2>()
    {
        { Ghost.Blinky, new Vector2(12, 14) },
        { Ghost.Pinky, new Vector2(-12, 14) },
        { Ghost.Inky, new Vector2(12, -14) },
        { Ghost.Clyde, new Vector2(-12, -14) },
    };

    //Pinky constants
    public const int PINKY_DISTANCE_OFFSET = 4;
    public const int PINKY_DOTS_NEEDED_TO_SPAWN = 0;
    public static readonly Dictionary<Direction, Vector2> PINKY_BEHAVIOUR_DIRECTION = new Dictionary<Direction, Vector2>()
    {
        { Direction.Up, new Vector2(-1, 1) }, //Overflow error causes pinky to want to be 4 units left and up when pacman is going up
        { Direction.Down, Vector2.down },
        { Direction.Left, Vector2.left },
        { Direction.Right, Vector2.right },
    };

    //Inky constants
    public const int INKY_DISTANCE_OFFSET = 2;
    public const int INKY_DOTS_NEEDED_TO_SPAWN = 30;
    public static readonly Dictionary<Direction, Vector2> INKY_BEHAVIOUR_DIRECTION = new Dictionary<Direction, Vector2>()
    {
        { Direction.Up, new Vector2(-1, 1) }, //Overflow error causes pinky to want to be 4 units left and up when pacman is going up
        { Direction.Down, Vector2.down },
        { Direction.Left, Vector2.left },
        { Direction.Right, Vector2.right },
    };
}
