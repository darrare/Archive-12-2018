using System;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using AUTools;
using UnityEngine;
using AUTools.Unity;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using UnityEngine.SceneManagement;

namespace Mediocracy
{
    /// <summary>Singleton manager.</summary>
    public class GameManager
    {
        #region Fields

        static GameManager instance;

        readonly Dictionary<WorldPrefab, GameObject> worldPrefabs;

        float totalInsanity;

        List<PsychedelicShaderDriver> buildingShaders = new List<PsychedelicShaderDriver>();

        RoomStatusScript finalRoom;

        #endregion

        #region Constructors

        /// <summary>Private constructor.</summary>
        GameManager()
        {
            Object.DontDestroyOnLoad(new GameObject("Updater", typeof(Updater)));
            worldPrefabs = Resources.LoadAll<GameObject>("Prefabs/World").ToDictionary(g => (WorldPrefab)Enum.Parse(typeof(WorldPrefab), g.name));
            RoomClearedEvent.Add(room =>
            {
                if (finalRoom.Insanity <= Constants.LOW_ROOM_INSANITY && Rooms.All(r => r.IsUnlocked))
                {
                    SceneManager.LoadScene("GameWin");
                }
            });
        }

        #endregion

        #region Properties

        /// <summary>Gets the singleton instance.</summary>
        [NotNull]
        public static GameManager Instance => instance ?? (instance = new GameManager());

        /// <summary>
        /// 
        /// </summary>
        public bool IsPaused
        {
            get { return PauseObjScript.AllPaused; }
            set { PauseObjScript.AllPaused = value; }
        }

        /// <summary>
        /// A list of the rooms on the map
        /// </summary>
        public List<RoomStatusScript> Rooms
        { get; set; } = new List<RoomStatusScript>();

        /// <summary>
        /// Current difficulty essentially
        /// </summary>
        public int CurFloor
        { get; set; } = 0;

        public SafeEvent<GameObject> RoomClickedEvent { get; } = new SafeEvent<GameObject>();

        public SafeEvent<GameObject> RoomClearedEvent { get; } = new SafeEvent<GameObject>();

        public GameObject CurRoom
        { get; set; }

        public float TotalInsanity
        {
            get { return totalInsanity; }
            set
            {
                totalInsanity = Mathf.Clamp(value, 0, Rooms.Count);
                if (value >= Rooms.Count && Rooms.Count != 0)
                {
                    SceneManager.LoadScene("GameLose");
                }
                float localInsanity = value / Rooms.Count;
                foreach (PsychedelicShaderDriver shader in buildingShaders)
                {
                    shader.insanity = localInsanity;
                }
            }
        }

        #endregion

        #region updater

        /// <summary>
        /// Updates the game manager class
        /// </summary>
        class Updater : MonoBehaviour
        {
            void Update()
            {
                instance.Update();
            }
        }

        #endregion

        #region Public Methods

        public void CreateBuilding()
        {
            Rooms.Clear();
            TotalInsanity = 0;
            List<RoomStatusScript> prevRow = new List<RoomStatusScript>();
            List<RoomStatusScript> curRow = new List<RoomStatusScript>();
            buildingShaders.Clear();
            RoomStatusScript prevNode = null;

            for (int y = 0; y < Constants.BUILDING_HEIGHT; y++)
            {
                float yPos = y * Constants.BLOCK_HEIGHT * 2;
                GameObject obj = Object.Instantiate(worldPrefabs[WorldPrefab.BuildingLeft], new Vector3((-Constants.BUILDING_WIDTH - 1) * Constants.BLOCK_WIDTH, yPos),
                    Quaternion.identity);
                buildingShaders.Add(obj.GetComponent<PsychedelicShaderDriver>());
                for (int x = -Constants.BUILDING_WIDTH + 1; x <= Constants.BUILDING_WIDTH; x += 2)
                {
                    RoomStatusScript room = Object.Instantiate(worldPrefabs[WorldPrefab.BuildingMain], new Vector3(x * Constants.BLOCK_WIDTH, yPos), Quaternion.identity)
                        .GetComponent<RoomStatusScript>();
                    room.Initialize();
                    room.Level = y;
                    Rooms.Add(room);
                    buildingShaders.Add(room.GetComponent<PsychedelicShaderDriver>());
                    curRow.Add(room);

                    if (prevNode != null)
                    {
                        room.GetComponent<RoomStatusScript>().Neighbors.Add(prevNode);
                        prevNode.Neighbors.Add(room.GetComponent<RoomStatusScript>());
                    }

                    if (prevRow.Count >= curRow.Count)
                    {
                        room.GetComponent<RoomStatusScript>().Neighbors.Add(prevRow[curRow.Count - 1]);
                        prevRow[curRow.Count - 1].Neighbors.Add(room.GetComponent<RoomStatusScript>());
                    }

                    prevNode = room;
                }
                prevRow = curRow;
                prevNode = null;
                curRow = new List<RoomStatusScript>();

                obj = Object.Instantiate(worldPrefabs[WorldPrefab.BuildingRight], new Vector3((Constants.BUILDING_WIDTH + 1) * Constants.BLOCK_WIDTH, yPos), Quaternion.identity);
                buildingShaders.Add(obj.GetComponent<PsychedelicShaderDriver>());
            }

            finalRoom = Object.Instantiate(worldPrefabs[WorldPrefab.BuildingTop], new Vector3(0, Constants.BUILDING_HEIGHT * Constants.BLOCK_HEIGHT * 2), Quaternion.identity)
                .GetComponent<RoomStatusScript>();
            finalRoom.Initialize();
            finalRoom.Level = Constants.BUILDING_HEIGHT + 3;
            Rooms.Add(finalRoom);
            buildingShaders.Add(finalRoom.GetComponent<PsychedelicShaderDriver>());
            prevRow[1].Neighbors.Add(finalRoom);
            finalRoom.Neighbors.Add(prevRow[1]);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the game manager
        /// </summary>
        void Update()
        {
            AudioManager.Instance.Update();
            TotalInsanity += Rooms.Where(r => r.IsUnlocked).Sum(r => r.Insanity) * Time.deltaTime * Constants.TOTAL_INSANITY_MULT;
        }

        #endregion
    }

}