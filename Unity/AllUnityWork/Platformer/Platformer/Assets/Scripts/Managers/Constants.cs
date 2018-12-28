using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public static string AUDIO_FILE_LOCATION = "Sound/SoundEffects";

    #region Game balance constants

    //Audio
    public const float AUDIO_SOUND_EFFECT_VOLUME_MULTIPLIER = .75f;

    //Player stuff
    public const float PLAYER_MAX_JUMP_HEIGHT = 4f; //if "jump" is held, this is the maximum height the player will reach before the jump cancles itself
    public const float PLAYER_MIN_JUMP_HEIGHT = 1f; //if "jump" is pressed for a single frame the player will go this high
    public const float PLAYER_TIME_TO_JUMP_APEX = .4f; //How long does it take the player to jump to the minimum jump height
    public const float PLAYER_ACCELERATION_TIME_AIRBORNE = .2f; //The acceleration rate when the player is airborne
    public const float PLAYER_ACCELERATION_TIME_GROUNDED = .1f; //The acceleration rate when the player is grounded
    public const float PLAYER_MOVE_SPEED = 15f; //Multiplier for the velocity when the player moves
    public const float PLAYER_INITIAL_BOOST_MULTIPLIER = 30f; //Initial boost multiplier for when the player uses boost
    public const int PLAYER_MAX_BOOST_COUNT = 3; //Amount of boosts the player can use while in the air.
    public const float PLAYER_BOOST_DURATION = .3f; //How long the boost lasts
    public const float PLAYER_REACTING_TO_DAMAGE_DURATION = .25f; //How long the player loses control when they take damage
    public const float PLAYER_REACTING_TO_DAMAGE_KNOCKBACK_MULTIPLIER = 20f; //Multipler for when the player runs into something bad
    public const float PLAYER_IMMUNE_TO_DAMAGE_DURATION = .25f; //Duration that the player is immune to damage after reacting to damage has fallen
    public const float PLAYER_SLOWED_TIME_SPEED = .5f; //When the player is in slo-mo
    public const float PLAYER_NORMAL_TIME_SPEED = 1f; //When the player is not in slo-mo
    public const float PLAYER_TIME_LERP_DURATION = .35f; //Duration of time slowing down and speeding up
    public const float PLAYER_FAST_FALL_MULTIPLIER = 2f; //Multiplied by gravity to determine how fast the player falls when holding down.
    public const float PLAYER_INPUT_BOOST_INIT_MODIFIER = .01f; //How much of an effect the player has on motion at the begining of the boost
    public const float PLAYER_INPUT_BOOST_END_MODIFIER = .2f; //How much of an effect the player has on motion at the end of the boost
    public const float PLAYER_BOOST_END_MODIFIER = .3f; //velocity is multiplied this after the boost has ended to slow the player down

    //Camera stuff
    public const float CAMERA_POSITION_SMOOTH_TIME = .4f; //rate at which the camera smooths itself into its position
    public const float CAMERA_SIZE_SMOOTH_TIME = .5f; //rate at which the camera smooths its size
    public const float CAMERA_ORTHO_SIZE_MIN = 10f; //the minimum orthographic size of the camera (zoom in)
    public const float CAMERA_ORTHO_SIZE_MAX = 25f; //the maximum orthographic size of the camera (zoom out)
    public const float CAMERA_VELOCITY_WEIGHT_MULTIPLIER = .35f; //how much of the players velocity weighs on where the camera should position itself. (.33 would be a 3rd of the players velocity away from the player)
    public const float CAMERA_DISTANCE_TO_BE_REMOVED_FROM_TARGETS_LIST = 30f; //How close to other objects need to be in order to effect the cameras targeted position
    public const float CAMERA_DISTANCE_MAX_FOR_ORTHO_CAM = 14f; //if the cameras targeted position is this far away from the player, then the cameras orthographical size will be at its maximum
    public const float CAMERA_MAGNITUDE_MAX_FOR_ORTHO_CAM = 1f; //something about magnitude... Idk
    public const float CAMERA_SHAKE_MULTIPLIER = .75f; //Overall constant that we can test screenshake with
    public const float CAMERA_SHAKE_DAMPENER_MULTIPLIER = .99f; //The shake intensity is multiplied by this after every shake so that it can dampen itself over time.

    //Gun stuff
    public const float GUN_DELAY_BETWEEN_SHOTS = .05f;
    public const float GUN_BULLET_SPEED = 50f;
    public const float GUN_CAMERA_SHAKE_INTENSITY = .25f;
    public const float GUN_DAMAGE = 10f;

    #endregion


    #region Effects constants

    public const float CASING_INIT_VELOCITY_MULTIPLIER = 10f;
    public const float CASING_MAX_ROTATIONAL_VELOCITY = 500f;
    public const float CASING_TIME_UNTIL_DELETION = 2f;
    public const float CASING_TIME_UNTIL_FADE = 1.5f;

    public const float SHRAPNEL_TIME_UNTIL_DELETION = .15f;
    public const float SHRAPNEL_TIME_UNTIL_FADE = -1f; //-1 means that it does not fade
    public const int SHRAPNEL_NUM_TO_CREATE = 10;

    public const int BLOOD_SPLATTER_COUNT = 5;
    public const float BLOOD_SPLATTER_RANDOM_ANGLE_RANGE = 20f;
    public const float BLOOD_SPLATTER_INIT_SPEED = 20f;

    public const float EXPLOSION_FIRE_DURATION = 2f;
    public const float EXPLOSION_FIRE_TIME_UNTIL_FADE = 1.5f;
    public const float EXPLOSION_FIRE_DAMAGE = 10f;

    #endregion

    #region Enemy Constants

    public const float DASHING_ENEMY_HEALTH = 30f;
    public const float DASHING_ENEMY_CHARGE_TIME = 0f; //Time the enemy sits there idle waiting to charge
    public const float DASHING_ENEMY_INIT_SPEED = 20f; //the initial velocity multiplier when charging.
    public const float DASHING_ENEMY_DECELERATION_TIME = .75f; //The time it takes to go from top speed to 0
    public const float DASHING_ENEMY_DECELERATION_TIME_VARIANCE = .1f; // +- on the deceleration time for uniqueness
    public const float DASHING_ENEMY_MAGNITUDE_TO_START_CHARGING = 6f; //When the enemy decides to stop moving and starts charging up
    public const float DASHING_ENEMY_DEATH_CAMERA_SHAKE_INTENSITY = .5f; //when the enemy dies, it shakes the screen this hard
    public const float DASHING_ENEMY_DEATH_CAMERA_SHAKE_DURATION = .5f; //when the enemy dies, it shakes the screen for this duration
    public const float DASHING_ENEMY_CAMERA_WEIGHT = .33f;
    public const float DASHING_ENEMY_AGGRO_DISTANCE = 20f;
    public const float DASHING_ENEMY_DAMAGE = 10f;
    public const float DASHING_ENEMY_FRAMES_TO_PREDICT_LOCATION = 30f; //Assume the players position in this amount of frames, and dash there

    #endregion

    #region Boss Constants

    public const float SHIP_BOSS_HEALTH = 5000;
    public const float SHIP_BOSS_TIME_BETWEEN_STATES = 3f;
    public const float SHIP_BOSS_SPEED = 20f;
    public const float SHIP_BOSS_TIME_TO_GET_TO_CENTER = 1f;
    public const float SHIP_BOSS_CAMERA_WEIGHT = 1f;
    public const float SHIP_BOSS_AGGRO_DISTANCE = 30f;
    public const float SHIP_BOSS_DEATH_CAMERA_SHAKE_INTENSITY = 1f;
    public const float SHIP_BOSS_DEATH_CAMERA_SHAKE_DURATION = 5f;
    public const float SHIP_BOSS_CANNON_FREE_SHOOT_DELAY = .25f;
    public const float SHIP_BOSS_CANNON_SPIN_SHOOT_DELAY = .05f;
    public const float SHIP_BOSS_CANNON_SPIN_DURATION = 5f;
    public const float SHIP_BOSS_CANNON_PROJECTILE_SPEED = 20f;
    public const float SHIP_BOSS_CANNON_DAMAGE = 10f;
    public const float SHIP_BOSS_CANNON_ROTATION_SPEED = 150f; //max amount of angles per second
    public const float SHIP_BOSS_NUKE_CAMERA_SHAKE_INTENSITY = .75f;
    public const float SHIP_BOSS_NUKE_CAMERA_SHAKE_DURATION = 2.5f;
    public const float SHIP_BOSS_MINI_NUKE_DROP_DELAY = .25f;
    public const float SHIP_BOSS_MINI_NUKE_HP = 30f;
    public const float SHIP_BOSS_MINI_NUKE_EXPLOSION_DURATION = 2f;
    public const float SHIP_BOSS_MINI_NUKE_EXPLOSION_FADE_TIME = 1.5f;
    public const float SHIP_BOSS_NUKE_HP = 100f;
    public const float SHIP_BOSS_NUKE_EXPLOSION_DURATION = 2f;
    public const float SHIP_BOSS_NUKE_EXPLOSION_FADE_TIME = 1.5f;
    public const float SHIP_BOSS_MIN_DISTANCE_TO_SWITCH_STAGE = 1f;
    public static readonly Vector3 SHIP_BOSS_NUKE_DROP_ACCELERATION = new Vector3(0, -10, 0);


    public const float EYEBALL_BOSS_TIME_FOR_PHASE_TRANSITION = 2f;
    public const float EYEBALL_BOSS_TELEGRAPH_DURATION = 2f; //The time the boss will telegraph its attacks, in seconds
    public const float EYEBALL_BOSS_ATTACKING_DURATION = 5f;
    public const int EYEBALL_BOSS_NUM_ADDS_TO_SPAWN = 5; //The number of adds that get spawned when doing the spawn attack
    public const float EYEBALL_BOSS_SHOOT_DELAY = .75f;
    public const int EYEBALL_BOSS_NUM_PLATFORMS_TO_DISABLE_PER_EYE = 2;
    public const float EYEBALL_BOSS_GLOW_DURATION = .5f; //the time it takes to go from black to color, and then same time back
    public static readonly Dictionary<int, Color> EYEBALL_BOSS_TELEGRAPH_COLORS = new Dictionary<int, Color>()
    {
        { 0, Color.yellow }, //spawn
        { 1, Color.red }, //DisablePlatform
        { 2, Color.green }, //FireShots
        { 3, Color.blue }, //EnergyBeam
    };
    public const float EYEBALL_BOSS_TENDRIL_DROP_TIME = 3f; //Time it takes for the tendrils to delete themselves after dropping
    public const float EYEBALL_BOSS_TENDRIL_GRAVITY_LERP_DURATION = 1f; //time it takes gravity to go from -1 to 1
    public const float EYEBALL_BOSS_TENDRIL_GRAVITY_MAX = 1f;
    public const float EYEBALL_BOSS_TENDRIL_RETRACT_DELAY = .02f;
    public const int EYEBALL_BOSS_TENDRILS_PER_PLATFORM = 3;
    public const float EYEBALL_BOSS_INDICATOR_BEAM_TIME_UNTIL_FADE_START = EYEBALL_BOSS_TELEGRAPH_DURATION;
    public const float EYEBALL_BOSS_INDICATOR_BEAM_DURATION = EYEBALL_BOSS_INDICATOR_BEAM_TIME_UNTIL_FADE_START + 2f;
    public const float EYEBALL_BOSS_LAZER_BEAM_DAMAGE = 50f;
    public const float EYEBALL_BOSS_LAZER_BEAM_TIME_UNTIL_FADE_START = EYEBALL_BOSS_LAZER_BEAM_DURATION * .7f;
    public const float EYEBALL_BOSS_LAZER_BEAM_DURATION = EYEBALL_BOSS_ATTACKING_DURATION - EYEBALL_BOSS_LAZER_BEAM_CHARGE_TIME;
    public const float EYEBALL_BOSS_LAZER_BEAM_CHARGE_TIME = 1.5f;

    public const float EYEBALL_BOSS_PROJECTILE_EXPLOSION_DURATION = .75f;
    public const float EYEBALL_BOSS_PROJECTILE_EXPLOSION_TIME_UNTIL_FADE = .5f;
    public const float EYEBALL_BOSS_PROJECTILE_EXPLOSION_DAMAGE = 10f;
    public const float EYEBALL_BOSS_PROJECTILE_EXPLOSION_SCALE = .75f;
    public const float EYEBALL_BOSS_PROJECTILE_TIME_BETWEEN_SHOTS = 1.5f;
    public const float EYEBALL_BOSS_PROJECTILE_SPEED = .25f;
    public const float EYEBALL_BOSS_PROJECTILE_DURATION = 5f;
    public const float EYEBALL_BOSS_PROJECTILE_DISTANCE_FROM_PLAYER_TO_EXPLODE = 10f;

    public const float EYEBALL_BOSS_SPAWN_RANDOMIZATION_MULTIPLIER = .3f; //All variables used for movement will be randomized by this amount. (.1 = 10%)
    public const float EYEBALL_BOSS_SPAWN_CHASE_SPEED = 10f; //Randomzied
    public const float EYEBALL_BOSS_SPAWN_ACCELERATION_MODIFIER = 500f; //randomized
    public const float EYEBALL_BOSS_SPAWN_TURN_SPEED_MODIFIER = .2f; //randomized, while turning, go this speed of normal (.5 is 50%)
    public const float EYEBALL_BOSS_SPAWN_TIME_BETWEEN_SPAWNS = .5f;
    public const float EYEBALL_BOSS_SPAWN_EXPLOSION_SCALE = .25f;
    public const float EYEBALL_BOSS_SPAWN_EXPLOSION_DAMAGE = 10f;
    public const float EYEBALL_BOSS_SPAWN_EXPLOSION_DURATION = .5f;
    public const float EYEBALL_BOSS_SPAWN_EXPLOSION_FADE_TIME = .25f;

    public const float EYEBALL_BOSS_PHASE_ONE_EYE_DESTROY_CAMERA_SHAKE_INTENSITY = .5f;
    public const float EYEBALL_BOSS_PHASE_ONE_EYE_DESTROY_CAMERA_SHAKE_DURATION = .5f;
    public const float EYEBALL_BOSS_PHASE_TWO_EYE_DESTROY_CAMERA_SHAKE_INTENSITY = 1f;
    public const float EYEBALL_BOSS_PHASE_TWO_EYE_DESTROY_CAMERA_SHAKE_DURATION = 3f;
    public const float EYEBALL_BOSS_PHASE_ONE_DESTRUCTION_BLOOD_SPAWN_INTERVAL = .1f;
    public const float EYEBALL_BOSS_PHASE_ONE_DESTRUCTION_BLOOD_SPAWN_DURATION = 1f;

    #endregion

}
