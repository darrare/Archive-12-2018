using System.Collections;
using System.Collections.Generic;
using Mediocracy;
using UnityEngine;

public class TransposerMinigame : MonoBehaviour
{
    BoxCollider2D col;
    private void Start()
    {
        col = gameObject.AddComponent<BoxCollider2D>();
        GetComponent<ColliderUpdater>().UpdateCollider();
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.CurRoom)
        {
            UIManager.Instance.OpenUI(MenuType.RhythmGame);
            AudioManager.Instance.PlayOneShot(SoundEffects.TransponderClick);
        }
    }
}