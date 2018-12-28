using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mediocracy
{
    public class ButtonLerpScript : MonoBehaviour
    {
        float buttonScale = 1.3f;
        RectTransform rect;

        void Start()
        {
            rect = GetComponent<RectTransform>();
        }

        public void OnMouseOver()
        {
            rect.localScale = Vector3.one * buttonScale;
            AudioManager.Instance.PlayOneShot(SoundEffects.UIButtonHover);
        }

        public void OnMouseExit()
        {
            rect.localScale = Vector3.one;
        }
    }
}
