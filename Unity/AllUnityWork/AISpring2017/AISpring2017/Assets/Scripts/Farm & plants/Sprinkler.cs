using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprinkler : MonoBehaviour
{
    #region Variables

    public GameObject thisFarmTile;
    public bool playAnimation;
    public Sprite startFrame;
    public Sprite endFrame;
    public int timer = 300;
    public Animator thisAnimation;

    #endregion

    #region Start

    // Use this for initialization
    void Start ()
    {
        thisAnimation = GetComponent<Animator>();
	}

    #endregion

    #region Update

    // Update is called once per frame
    void Update ()
    {
        if (!Game_Manager.Instance.pauseGame)
        {
            if (timer > 0)
            {
                timer--;
            }

            if ((GetComponent<SpriteRenderer>().sprite.name == endFrame.name) && playAnimation)
            {
                playAnimation = false;
                thisFarmTile.GetComponent<Farm_Controller>().waterLevel += 20;
            }

            if ((timer == 0) && (thisFarmTile.GetComponent<Farm_Controller>().waterLevel <= 50))
            {
                playAnimation = true;
                timer = 300;
            }

            if (playAnimation)
            {
                thisAnimation.speed = 2;
            }
            else
            {
                thisAnimation.speed = 0;
            }
        }
	}

    #endregion
}