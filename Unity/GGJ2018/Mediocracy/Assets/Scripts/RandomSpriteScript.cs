using AUTools.Unity;
using UnityEngine;

namespace Mediocracy
{
    /// <summary>Script that picks a random sprite for the renderer.</summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class RandomSpriteScript : MonoBehaviour
    {
        [SerializeField] Sprite[] sprites;

        /// <summary>Awake is called when the script instance is being loaded.</summary>
        void Awake()
        {
            GetComponent<SpriteRenderer>().sprite = sprites.GetRandom();
            Destroy(this);
        }
    }
}