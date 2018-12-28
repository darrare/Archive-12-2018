using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Movement : MonoBehaviour
{
    public GameObject water;
    public float speed = 5f;
    public float startScale;
    public Vector3 updatePosition;

	// Use this for initialization
	void Start ()
    {
        startScale = water.transform.localScale.y;
        transform.position = new Vector3(transform.position.x, transform.position.y, -2f);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        //if (!Game_Manager.Instance.gameOver)
        //{
        //    updatePosition.x = Input.GetAxis("CharacterJoystickHorizontal");
        //    updatePosition.y = Input.GetAxis("CharacterJoystickVertical");

        //    if (Input.GetKey(Game_Manager.Instance.Controls[Game_Manager.Instance.currentControls][Game_Manager.Direction.UP]))
        //    {
        //        updatePosition = Vector3.up * speed * Time.deltaTime;
        //    }

        //    if (Input.GetKey(Game_Manager.Instance.Controls[Game_Manager.Instance.currentControls][Game_Manager.Direction.LEFT]))
        //    {
        //        updatePosition = Vector3.left * speed * Time.deltaTime;
        //    }

        //    if (Input.GetKey(Game_Manager.Instance.Controls[Game_Manager.Instance.currentControls][Game_Manager.Direction.DOWN]))
        //    {
        //        updatePosition = Vector3.down * speed * Time.deltaTime;
        //    }

        //    if (Input.GetKey(Game_Manager.Instance.Controls[Game_Manager.Instance.currentControls][Game_Manager.Direction.RIGHT]))
        //    {
        //        updatePosition = Vector3.right * speed * Time.deltaTime;
        //    }

        //    transform.position += new Vector3(updatePosition.x, updatePosition.y, 0);

        water.GetComponent<Image>().fillAmount = Game_Manager.Instance.waterLevel / 100f;
        //}
    }
}
