using UnityEngine;
using System.Collections;

public class RulesSwipe : SwipeScript {
	public Rules rulesScript;
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	}
	
	public override void SwipeRight()
	{
		rulesScript.CloseRules ();
	}
	
	public override void SwipeLeft()
	{

	}
	
	protected override void SwipeUp()
	{
		
	}
	
	protected override void SwipeDown()
	{
		
	}
}
