using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Update_Money : MonoBehaviour
{
    #region Update
       
    // Update is called once per frame
    void Update ()
    {
        GetComponent<Text>().text = "$ " + Game_Manager.Instance.money;
	}

    #endregion
}
