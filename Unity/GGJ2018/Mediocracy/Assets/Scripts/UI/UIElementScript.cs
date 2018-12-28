using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mediocracy
{
    public abstract class UIElementScript : MonoBehaviour
    {
        public abstract MenuType Type
        { get; }

        /// <summary>
        /// Closes the UI element
        /// </summary>
        public void CloseUIElement()
        {
            UIManager.Instance.CloseUI(Type);
        }
    }
}