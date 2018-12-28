using UnityEngine;

namespace Mediocracy
{
    /// <summary>Script that controls the level in general.</summary>
    public class LevelControlScript : MonoBehaviour
    {
        #region Fields



        #endregion

        #region Properties



        #endregion

        #region Public Methods



        #endregion

        #region Private Methods

        /// <summary>Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.</summary>
        void Start()
        {
            GameManager.Instance.CreateBuilding();
        }

        #endregion
    }
}