using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialRanger : Tutorial 
{
    [SerializeField]RectTransform hudParent;
    [SerializeField]GameObject rangerHUD;
    [SerializeField]Image healthBar;
    [SerializeField]Image energyBar;

	protected override void Start()
	{
		base.Start ();
        // Creates the player and HUD
        GameObject player = (GameObject)Instantiate(GameManager.Instance.PlayerPrefabs[CharacterType.Ranger], new Vector2(0, 1), transform.rotation);
        GameObject hud = Instantiate<GameObject>(rangerHUD);
        hud.transform.SetParent(hudParent, false);
        HUDScript hudScript = hud.GetComponent<HUDScript>();
        player.GetComponent<PlayerScript>().Initialize(healthBar, energyBar, hudScript.GcdBars, hudScript.TimerBars, hudScript.SecondaryCDBar,
            hudScript.PowerCDBar, hudScript.SpecialCDBar);
		StageOne ();
	}

	public override void ButtonTextChange ()
	{
		base.ButtonTextChange ();
	}

//	public void NextStage()
//	{
//		isCompleted [(int)stage] = true;
//		stage++;
//		switch ((int)stage) 
//		{
//		case 0:
//			StageOne ();
//			break;
//		case 1:
//			StageTwo ();
//			break;
//		case 2:
//			StageThree ();
//			break;
//		case 3:
//			StageFour ();
//			break;
//		case 4:
//			StageFive ();
//			break;
//		}
//	}

	void StageOne(){
		ChangeText (new string[] {"<b>Ranger tutorial</b> " +
			"\nHere you will learn how to control the ranger well enough to survive the trials put before you.",
			"<b>Objectives:</b> " +
			"\nMovement" +
			"\nRanger Abilities" +
			"\nInterface", 
			"<b>Movement</b> " +
			"\n[A] Move left" +
			"\n[D] Move right" +
			"\n[SPACE] Jump" +
			"\nTry these actions."});
		TutorialDummy.PlayerMoved += StageTwo;
	}

	void StageTwo()
	{
		stage++;
		TutorialDummy.PlayerMoved -= StageTwo;
        ChangeText(new string[] {"Nice work. Let's move on to your abilities.",
			"<b>Basic Ability</b>" +
			"\nThe " + GameManager.Instance.CurrentSave.Inputs[InputType.Main].NameWithType + 
            " will make you fire a basic arrow. Use it to attack the dummy."});
		TutorialDummy.BasicAttack += StageThree;
	}

	void StageThree()
	{
		stage++;
		TutorialDummy.BasicAttack -= StageThree;
		ChangeText (new string[] {"Good work." +
			"\nAttacks like arrows can also be used to hit attacks from enemies in the air and block them.",
			"<b>Secondary Ability</b>" +
			"\nThe " + GameManager.Instance.CurrentSave.Inputs[InputType.Secondary].NameWithType + 
            " will make you fire an explosive arrow. Blow up the dummy."});
		TutorialDummy.SecondaryAttack += StageFour;
	}

	void StageFour()
	{
		stage++;
		TutorialDummy.SecondaryAttack -= StageFour;
		ChangeText (new string[] {"Woah! That was an epic explosion." +
			"\nLet's get more advanced here, shall we?",
			"<b>Special Ability</b>" +
			"\nHolding down the " + GameManager.Instance.CurrentSave.Inputs[InputType.Power].NameWithType + 
            " will fire a barrage of penetrating arrows. Test it on these dummies."});
		TutorialDummy.SpecialAttack += StageFive;
	}

	void StageFive()
	{
		stage++;
		TutorialDummy.SpecialAttack -= StageFive;
		ChangeText (new string[] {"Devastating! If you keep this up, you will become champion in no time. " +
			"Every champion needs their boost ability, though.",
			"<b>Boost Ability</b>" +
			"\nPress the " + GameManager.Instance.CurrentSave.Inputs[InputType.Special].NameWithType + 
            " to gain increased mobility, power, and efficiency for a short period. Give it a shot."});
		TutorialDummy.Booster += StageSix;
	}

	void StageSix()
	{
		stage++;
		TutorialDummy.Booster -= StageSix;
		ChangeText (new string[] {
			"Look at how fast and strong you are right now! Use this at the right time to throw the advantage your way.",
			"Don't think you are going to spend all of your time attacking dummies. Your enemies will be actively trying to end your life.",
			"In the bottom left corner you can see the current status of your abilities and health.",
			"The red bar is your health. If this reaches zero you lose, so be careful.",
			"The yellow bar is your energy bar. Each of your abilities use up some energy, the more powerful abilities costing more.",
			"The ability bars display the cooldowns of each of your abilities; watch them to see when your abilities are available.",
			"Press the Escape key to open the pause menu. Use it to exit the tutorial."
		});
	}
}
