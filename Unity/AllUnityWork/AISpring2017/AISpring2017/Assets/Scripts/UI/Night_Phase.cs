using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Night_Phase : MonoBehaviour
{
    public GameObject dayNightWheel;
    Color Night = new Color(0, 0, 0, .49f);
    Color Day = new Color(0, 0, 0, 0);

    // Update is called once per frame
    void Update ()
    {
		if(Game_Manager.Instance.currentPhase == Game_Manager.Phase.NIGHT)
        {
            GetComponent<Image>().color = Night;
            dayNightWheel.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90 + (((float)Game_Manager.Instance.spawnCount / (float)Game_Manager.Instance.totalWaveEnemies) * 270f));
        }
        else
        {
            GetComponent<Image>().color = Day;
            dayNightWheel.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + ((((float)Game_Manager.Instance.dayTimerConstant - (float)Game_Manager.Instance.dayTimer) / (float)Game_Manager.Instance.dayTimerConstant) * 90f));
        }
    }
}
