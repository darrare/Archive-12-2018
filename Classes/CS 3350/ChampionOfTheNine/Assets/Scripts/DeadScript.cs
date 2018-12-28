using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls a dead character
/// </summary>
public class DeadScript : MonoBehaviour
{
    #region Fields

    [SerializeField]Sprite deadImage;
    Timer fallTimer;
    Timer fadeTimer;
    Color startColor;
    Color endColor;
    Quaternion startRot;
    Quaternion endRot;
    SpriteRenderer sprite;

    #endregion

    #region Public Methods

    /// <summary>
    /// Initializes the dead character
    /// </summary>
    /// <param name="color">the character color</param>
    /// <param name="scale">the character scale</param>
    /// <param name="centerPosition">the position of the center of the feet of the character</param>
    public void Initialize(Color color, Vector3 scale, Vector3 centerPosition)
    {
        // Sets up timers
        fallTimer = new Timer(Constants.DEATH_FALL_TIME);
        fallTimer.Register(FallTimerFinished);
        fallTimer.Start();
        fadeTimer = new Timer(Constants.DEATH_FADE_TIME);
        fadeTimer.Register(FadeTimerFinished);

        // Sets up colors
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = color;
        startColor = color;
        endColor = new Color(color.r, color.g, color.b, 0);

        // Sets up rotations
        transform.position = centerPosition + new Vector3(sprite.sprite.bounds.extents.x, 0);
        transform.localScale = scale;
        startRot = Quaternion.Euler(Vector3.zero);
        endRot = Quaternion.Euler(0, 0, 90 * Mathf.Sign(scale.x));
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        if (fallTimer.IsRunning)
        {
            fallTimer.Update();
            transform.rotation = Quaternion.Lerp(startRot, endRot, fallTimer.ElapsedSeconds / fallTimer.TotalSeconds);
        }
        else if (fadeTimer.IsRunning)
        {
            fadeTimer.Update();
            sprite.color = Color.Lerp(startColor, endColor, fadeTimer.ElapsedSeconds / fadeTimer.TotalSeconds);
        }
    }

    /// <summary>
    /// Handles the fall timer finishing
    /// </summary>
    private void FallTimerFinished()
    {
        fadeTimer.Start();
        sprite.sprite = deadImage;
    }

    /// <summary>
    /// Handles the fade timer finishing
    /// </summary>
    private void FadeTimerFinished()
    {
        Destroy(gameObject);
    }

    #endregion
}
