using UnityEngine;

namespace Mediocracy
{
    /// <summary>Script that controls room selection.</summary>
    public class RoomSelectionScript : MonoBehaviour
    {
        #region Fields



        #endregion

        #region Properties



        #endregion

        #region Public Methods



        #endregion

        #region Protected Methods



        #endregion

        #region Private Methods

        void OnMouseDown()
        {
            if (UIManager.Instance.IsAnyMenuOpen())
                return;
            GameManager.Instance.RoomClickedEvent.Invoke(gameObject);
            GetComponent<Collider2D>().enabled = false;
            GameManager.Instance.CurRoom = gameObject;
            gameObject.GetComponent<RoomStatusScript>().EnableAudioSources();
            gameObject.GetComponent<RoomStatusScript>().GetRandomVoice(gameObject.GetComponent<RoomStatusScript>().Insanity);
        }

        #endregion
    }
}