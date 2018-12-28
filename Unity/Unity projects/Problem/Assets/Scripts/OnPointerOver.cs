using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class OnPointerOver : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler {

	public int menuState;
	public MainMenuController menuController;

	public void OnPointerEnter(PointerEventData eventData)
	{
		menuController.ChangeState (menuState);
	}

	public void OnPointerExit(PointerEventData eventData)
	{

	}
}
