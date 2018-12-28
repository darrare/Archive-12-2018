using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CONSTANTS{
	public static LevelController levelController;
	public static SoundEffects soundEffects = GameObject.Find("_SFX").GetComponent<SoundEffects>();
	public static BackgroundMusic backgroundMusic = GameObject.Find("_BGM").GetComponent<BackgroundMusic>();
	//public static List<List<string>> controllerSetups = new List<List<string>>();
	public static List<string> retro = new List<string>() {"Camera Pan", "Jump", "", "Sprint", "Interact", "Shoot Laser", "", "", "Shoot Laser", "Movement", "Movement"};
	public static List<string> newAge = new List<string>() {"Camera Pan", "Jump", "", "", "Interact", "Shoot Laser", "Shoot Laser", "Sprint", "", "Movement", "Movement"};
	public static List<string> triggerHappy = new List<string>() {"Camera Pan", "Jump", "", "Sprint", "Interact", "", "Shoot Laser", "Shoot Laser", "", "Movement", "Movement"};
	public static Vector2 levelSelectSpawnPosition = new Vector2(0, 0);

	//save data
	public static Dictionary<string, LevelSaveInfo> levelInfo = new Dictionary<string, LevelSaveInfo>();
	public static SettingsScreen.ControllerLayout controllerLayout = SettingsScreen.ControllerLayout.RETRO;
}
