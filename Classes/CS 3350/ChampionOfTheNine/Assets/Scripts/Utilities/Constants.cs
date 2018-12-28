using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Class that holds the game's constants
/// </summary>
public static class Constants
{
    public const int NUM_KINGDOMS = 9;
    public const string ENEMY_TAG = "Enemy";
    public const string PLAYER_TAG = "Player";
    public const string ATTACK_TAG = "Attack";
    public const float PITCH_CHANGE = 0.2f;
    public const float DARKNESS_TIMER = 3f;
    public const float PROJ_MAX_DIST = 20;
    public const float PLAYER_HEALTH_MULT = 3;
    public static int GROUND_LAYER = LayerMask.NameToLayer("Ground");
    public static Color ENEMY_COLOR = new Color(1, 0.51f, 0.51f);

    // AI
    public const float AI_SCAN_DELAY = 4;
    public const float AI_MIN_SPAWN_TIME = 4.5f;
    public const float AI_MAX_SPAWN_TIME = 9f;

    #region Ranger

    public const string RANGER_PLAYER_PREFAB = "RangerPlayer";
    public const string RANGER_AI_PREFAB = "RangerEnemy";
    public const float RANGER_AI_RANGE = 9;
    public const float RANGER_AI_ANGLE_RANGE = 15;
    public const int RANGER_AI_SHOT_THRESHOLD = 5;
    public const float RANGER_HEALTH = 100;
    public const float RANGER_ENERGY = 100;
    public const float RANGER_MOVE_SPEED = 5;
    public const float RANGER_JUMP_SPEED = 11;
    public const float RANGER_REGEN = 20;
    public const float RANGER_GCD = 0.37f;
    public const string RANGER_SHOOT_SND = "arrowShoot";
    public const string ARROW_HIT_SND = "arrowHit";

    // Basic arrow
    public const float BASIC_ARROW_SPEED = 13;
    public const float BASIC_ARROW_DAMAGE = 14;
    public const float BASIC_ARROW_COST = 6;

    // Piercing arrow
    public const float PIERCE_ARROW_SPEED = 25;
    public const float PIERCE_ARROW_DAMAGE = 17;
    public const float PIERCE_ARROW_COST = 13;
    public const float PIERCE_SHOOT_WINDOW = 0.7f;
    public const float PIERCE_SHOOT_CD = 0.1f;
    public const float PIERCE_ABILITY_CD = 8f;

    // Exploding arrow
    public const float EXP_ARROW_SPEED = 10;
    public const float EXP_ARROW_DAMAGE = 20;
    public const float EXP_ARROW_COST = 30;
    public const float EXP_ARROW_CD = 1.5f;
    public const string EXP_ARROW_SND = "exArrowHit";

    // Boost
    public const float RANGER_BOOST_CD = 20f;
    public const float RANGER_BOOST_TIME = 6f;
    public const float RANGER_BOOST_MOVE_MULT = 1.5f;
    public const float RANGER_BOOST_JUMP_MULT = 1.2f;
    public const float RANGER_BOOST_CD_MULT = 0.6f;
    public const float RANGER_BOOST_ARROW_SPEED_MULT = 1.2f;
    public const float RANGER_BOOST_ARROW_DAMAGE_MULT = 1.6f;
    public const float RANGER_BOOST_ENERGY_REGEN_MULT = 1.8f;
    public const string RANGER_BOOST_SND = "boost";
    public const string RANGER_BOOST_PART = "RangerBoostParticle";

    #endregion

    #region Mage

    public const string MAGE_PLAYER_PREFAB = "MagePlayer";
    public const string MAGE_AI_PREFAB = "MageEnemy";
    public const float MAGE_AI_RANGE = 9;
    public const float MAGE_AI_ANGLE_RANGE = 10;
    public const int MAGE_AI_SHOT_THRESHOLD = 5;
    public const float MAGE_HEALTH = 90;
    public const float MAGE_ENERGY = 550;
    public const float MAGE_MOVE_SPEED = 4;
    public const float MAGE_JUMP_SPEED = 10;
    public const float MAGE_REGEN = 10;
    public const float MAGE_GCD = 0.4f;

    // Ice
    public const float ICE_SPEED = 20;
    public const float ICE_DAMAGE = 7;
    public const float ICE_COST = 3;
    public const string ICE_CAST_SND = "iceShoot";

    // Meteor
    public const float METEOR_SPEED = 20;
    public const float METEOR_DAMAGE = 35;
    public const float METEOR_COST = 95;
    public const float METEOR_CD = 0.45f;
    public const float METEOR_ROT_SPEED = 360;
    public const string METEOR_CAST_SND = "meteorShoot";
    public static Vector2 METEOR_START_LOC = new Vector2(0, 9);

    // Lightning
    public const float LIGHTNING_DAMAGE = 3f;
    public const float LIGHTNING_COST_PER_SEC = 250;
    public const float LIGHTNING_CD = MAGE_GCD;
    public const float LIGHTNING_CAST_TIME = 0.1f;
    public const string LIGHTNING_CAST_SND = "lightning";

    // Drain
    public const float DRAIN_TIME = MAGE_GCD;
    public const float DRAIN_DAMAGE = 30f;
    public const float DRAIN_MANA_PER_TARGET = MAGE_ENERGY * 0.3f;
    public const float DRAIN_CD = 7f;
    public const float DRAIN_RANGE = 12f;
    public const float DRAIN_MIN_HEIGHT = 1f;
    public const float DRAIN_MAX_HEIGHT = 6f;
    public const float DRAIN_FLASH_TIME = 0.1f;
    public const int DRAIN_SEGMENTS = 6;
    public const string DRAIN_SND = "boost";
    public static Color BEAM_COLOR_1 = new Color(1, 1, 1, 1);
    public static Color BEAM_COLOR_2 = new Color(1, 1, 1, 0.4f);

    #endregion

    #region Warrior

    public const string WARRIOR_PLAYER_PREFAB = "WarriorPlayer";
    public const string WARRIOR_AI_PREFAB = "WarriorEnemy";
    public const float WARRIOR_AI_RANGE = 1.2f;
    public const float WARRIOR_AI_ANGLE_RANGE = 5;
    public const int WARRIOR_AI_SHOT_THRESHOLD = 5;
    public const float WARRIOR_HEALTH = 120;
    public const float WARRIOR_ENERGY = 100;
    public const float WARRIOR_MOVE_SPEED = 3.5f;
    public const float WARRIOR_JUMP_SPEED = 10;
    public const float WARRIOR_REGEN = 8;
    public const float WARRIOR_GCD = 0.4f;
    public const float WARRIOR_MAX_DAMAGE_BOOST = 1f;
    public const float WARRIOR_RECHARGE_TIME = 2f;
    public const float WARRIOR_ADR_LOSS = 4.2f;
    public const float WARRIOR_ADR_RECHARGE_LOSS = WARRIOR_ENERGY / WARRIOR_RECHARGE_TIME;
    public const string RECHARGE_PART = "RechargeParticle";

    // Slash
    public const float SLASH_HALF_ANGLE = 60;
    public const float SLASH_SWORD_ANGLE = 10;
    public const float SLASH_DAMAGE = 1;
    public const float SLASH_ADR = 0.25f;

    // Throwing axe
    public const float AXE_CD = WARRIOR_GCD;
    public const float AXE_SPEED = 15;
    public const float AXE_DAMAGE = 65;
    public const float AXE_ENERGY = 0;
    public const float AXE_ROT_SPEED = -1000;
    public const float AXE_ADR = 15;
    public const string AXE_PICKUP_TAG = "AxePickup";

    // Leap attack
    public const float LEAP_CD = 6;
    public const float LEAP_SPEED = 25;
    public const float LEAP_DAMAGE = 60;
    public const float LEAP_ADR = 15;
    public const string LEAP_SND = "leap";
    public const string LEAP_FLAG = "Slam";

    // Boost
    public const string WARRIOR_BOOST_SND = "warriorBoost";
    public const string WARRIOR_BOOST_PART = "WarriorBoostParticle";
    public const float WARRIOR_BOOST_TIME = 6f;
    public const float WARRIOR_BOOST_CD = 18f;

    #endregion

    #region Castles

    public const float CASTLE_HEALTH = 650;
    public const string CASTLE_HIT_SND = "hit";
    public const string CASTLE_DEATH_SND = "explosion";
    public const string CASTLE_FOLDER = "Castles/";
    public const string ASIAN_CASTLE_PREFAB = "Asian";
    public const string BANDIT_CASTLE_PREFAB = "Bandit";
    public const string CRYSTAL_CASTLE_PREFAB = "Crystal";
    public const string DARK_CASTLE_PREFAB = "Dark";
    public const string DESERT_CASTLE_PREFAB = "Desert";
    public const string MED_ONE_CASTLE_PREFAB = "MedOne";
    public const string MED_TWO_CASTLE_PREFAB = "MedTwo";
    public const string VIKING_CASTLE_PREFAB = "Viking";
    public const string VILLAGE_CASTLE_PREFAB = "Village";

    #endregion

    #region Particles

    public const string BLOOD_PART = "BloodParticle";
    public const string ICE_PART = "IceParticle";
    public const string LIGHTNING_PART = "LightningParticle";
    public const string SLAM_PART = "SlamParticle";

    #endregion

    #region Character

    public const string CHAR_HIT_SND = "hurt";
    public const string CHAR_DEATH_SND = "death";
    public const string CHAR_JUMP_SND = "jump";
    public const string CHAR_LAND_SND = "land";
    public const string CHAR_WALK_SND = "Walking";
    public const string GROUNDED_FLAG = "Grounded";
    public const string XVELOCTIY_FLAG = "XVelocity";
    public const float CHAR_GRAV_SCALE = 4;
    public const float GROUND_CHECK_RADIUS = 0.05f;
    public const float DEATH_FALL_TIME = 0.25f;
    public const float DEATH_FADE_TIME = 1.5f;

    #endregion

    #region Dynamic Map

    public static float[] PARALLAX_LEVELS = {1.3f, 3};
    public const float CLOUD_HEIGHT_MIN = 3.5f;
    public const float CLOUD_HEIGHT_MAX = 11;
	public const float CLOUD_DENSITY = 0.2f;
	public const float CLOUD_SCALE_MIN = 0.5f;
	public const float CLOUD_SCALE_MAX = 1.5f;
	public const float ELEVATION_CHANGE_WEIGHT = 0.35f; // low value = flatter map, high value = more change. 0 - 1
	public const float ELEVATION_CHANGE_OFFSET = 0.1f;
	public const float HEIGHT_DIFFERENCE_WEIGHT = 0.5f; // low value = downward direction, high value = upward direction. 0 - 1
	public const float HEIGHT_DIFFERENCE_OFFSET = 0.4f;
	public const float PARALLAX_SCALE = 0.01f;
    public const int PLATFORM_LENGTH = 18;
    public const int MAP_LENGTH = 100;
    public const int BASE_LEVEL = 5;
    public const int SOIL_HEIGHT = 8;
    public const string GROUND_PREFAB = "ground";
    public const string GROUND_UNDER_PREFAB = "groundUnder";
    public const string PARALLAX_BACKGROUND_TAG = "ParallaxBack";

    #endregion

    #region Time of Day

    // Cycle time is the time in seconds it takes for a full light/darkness cycle to occur
    // Percentages need to add to 0.5 with x2 orange times and x4 fade times
	public const float CYCLE_TIME = 90; 
    public const float DAY_TIME_PCT = 0.158f;
    public const float ORANGE_TIME_PCT = 0.025f;
    public const float FADE_TIME_PCT = 0.025f;
    public const float NIGHT_TIME_PCT = 0.5f - DAY_TIME_PCT - (ORANGE_TIME_PCT * 2) - (FADE_TIME_PCT * 4);
    public const float SKY_START_ROT = (DAY_TIME_PCT * 180);
	public const float BGM_MAX_VOLUME = 0.5f;
    public static Color DAY_SKY_COLOR = new Color(0.23f, 0.71f, 1, 1);
    public static Color ORANGE_SKY_COLOR = new Color(1, 0.61f, 0.19f, 1);
    public static Color NIGHT_SKY_COLOR = new Color(0, 0, 0, 0);
    public static Color DAY_DARKNESS_COLOR = new Color(0, 0, 0, 0);
    public static Color ORANGE_DARKNESS_COLOR = new Color(0, 0, 0, 0.2f);
    public static Color NIGHT_DARKNESS_COLOR = new Color(0, 0, 0, 0.5f);

    #endregion

    #region Scenes

    public const string LEVEL_SCENE = "DynamicLevel";
    public const string MAP_SCENE = "Map";
    public const string MAIN_MENU_SCENE = "MainMenu";
    public const string CHAR_CREATE_SCENE = "CharacterCreation";
    public const string TUTORIAL_SCENE = "Tutorial";
    public const string CREDITS_SCENE = "Credits";
    public const string CONTROLS_SCENE = "Controls";

    #endregion

    #region Files

    public const string FILE_SUFFIX = ".cotn";
    public const string SAVES_FILE = "finalsaves";
    public const string SND_FOLDER = "Sounds/";
    public const string PREFAB_FOLDER = "Prefabs/";
    public const string PART_FOLDER = "Particles/";
    public const string CLICK_SND = "Menubutton";
    public const string TUT_SAVE = "Tutorial";

    #endregion
}
