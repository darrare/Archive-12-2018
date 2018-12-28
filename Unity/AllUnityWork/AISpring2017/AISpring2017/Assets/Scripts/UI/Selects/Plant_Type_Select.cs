using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Type_Select : MonoBehaviour
{
    public Game_Manager.PlantType thisPlant;
    public GameObject buildSelect;
    public GameObject fireSelect;
    public GameObject iceSelect;
    public GameObject voidSelect;
    bool mouseHover = false;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if((gameObject.name != "Build_Select") &&Input.GetMouseButtonUp(0) && mouseHover)
        {
            buildSelect.transform.position = transform.position;
            Game_Manager.Instance.currentPlantSelection = thisPlant;
        }

        if((gameObject.name == "Build_Select") && Input.GetButtonUp("XButton"))
        {
            ChangeSelect();
        }
	}

    public void ChangeSelect()
    {
        if (buildSelect.transform.position == fireSelect.transform.position)
        {
            buildSelect.transform.position = iceSelect.transform.position;
            Game_Manager.Instance.currentPlantSelection = Game_Manager.PlantType.ICE;
        }
        else if (buildSelect.transform.position == iceSelect.transform.position)
        {
            buildSelect.transform.position = voidSelect.transform.position;
            Game_Manager.Instance.currentPlantSelection = Game_Manager.PlantType.VOID;
        }
        else
        {
            buildSelect.transform.position = fireSelect.transform.position;
            Game_Manager.Instance.currentPlantSelection = Game_Manager.PlantType.FIRE;
        }
    }

    private void OnMouseEnter()
    {
        mouseHover = true;
    }

    private void OnMouseExit()
    {
        mouseHover = false;
    }
}
