using UnityEngine;
using System.Collections;

public class ScoreKeepingSwipe : SwipeScript {
	public Control controlScript;

	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	}

	public override void SwipeRight()
	{
		controlScript.BackToMenu ();
	}
	
	public override void SwipeLeft()
	{
		controlScript.OpenRules ();
	}
	
	protected override void SwipeUp()
	{
		
	}
	
	protected override void SwipeDown()
	{
		
	}
}
