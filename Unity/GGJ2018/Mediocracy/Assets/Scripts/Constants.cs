using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mediocracy
{
    public static class Constants
    {
        #region Insanity tuning

        public const float INSANITY_BASE_DROP_PER_SECOND_MULTIPLIER = .005f;
        public const float INSANITY_BASE_DROP_PER_SECOND_ADDITION = .002f;

        public const float TOTAL_INSANITY_MULT = 0.05f;
        public const float MOVE_MULT = 3;
        public const float LOW_ROOM_INSANITY = .05f;
        public const float SUCCESS_MULT = 3f;

        #endregion

        #region World

        public const int BUILDING_WIDTH = 3;
        public const int BUILDING_HEIGHT = 10;
        public const float BLOCK_WIDTH = 16;
        public const float BLOCK_HEIGHT = 9;
        public const float CAMERA_SIZE = BLOCK_HEIGHT * (BUILDING_WIDTH + 2);
        public const float MAX_CAMERA_HEIGHT = BLOCK_HEIGHT * (BUILDING_HEIGHT - 1) * 2;

        #endregion

        #region audio constants

        public static string SFX_AUDIO_FILE_LOCATION = "Sound/SFX";
        public static string GREY_AUDIO_FILE_LOCATION = "Sound/Grey";
        public static string CRAZY_AUDIO_FILE_LOCATION = "Sound/Crazy";
        public static string VOICES_AUDIO_FILE_LOCATION = "Sound/Voices";
        public static string AMBIANCE_AUDIO_FILE_LOCATION = "Sound/Ambiance";
        public static string VOICESGREY_AUDIO_FILE_LOCATION = "Sound/GreyVoices";
        public static float AUDIO_BGM_VOLUME_DEFAULT = .5f;

        public const float MIN_AMBIANCE_TIME = 2.0f;
        public const float MAX_AMBIANCE_TIME = 7.0f;

        #endregion

        #region UI Constants

        public const string UI_PREFABS_FILE_LOCATION = "Prefabs/UI";

        #endregion

        #region Rhythm game constants

        //Tuning
        public const int RHYTHM_FLOORS_PER_EVENT_POSITION = 5;
        public const float RHYTHM_NEW_TUPLES_PER_FLOOR = 1; //This is floored, so if this value is .5, you will get a new value every 2 levels.
        public const float RHYTHM_TUPLE_SLIDE_SPEED = 300f;
        public const float RHYTHM_PER_FLOOR_MULTIPLIER = 30f;
        public const int RHYTHM_LEVEL_ONE_TUPLES = 3;
        public const float RHYTHM_CORRECT_RESPONSE_FADE_TIME = .5f;

        //Canvas setup
        public const int RHYTHM_LOCATIONS_IN_PANEL_BAR = 7;
        public const int RHYTHM_GAP_BETWEEN_TUPLES = 5;
        public const float RHYTHM_PERCENTAGE_PANEL_HEIGHT_FOR_TUPPLE = .8f;
        public const int RHYTHM_EXTRA_WIDTH_FOR_RECTANGLE = 40;
        public const float RHYTHM_FAILED_SPEED_MULTIPLIER = 5f;
        #endregion
    }

}
