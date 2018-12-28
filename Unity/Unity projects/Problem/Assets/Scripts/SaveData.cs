using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Runtime.Serialization;
using System.Reflection;

[Serializable ()]
public class SaveData : ISerializable {
	public SettingsScreen.ControllerLayout controllerLayout;
	public Dictionary<string, LevelSaveInfo> levelInfo = new Dictionary<string, LevelSaveInfo> ();

	public SaveData()
	{
		
	}

	public SaveData(SerializationInfo info, StreamingContext ctxt)
	{
		controllerLayout = (SettingsScreen.ControllerLayout)info.GetValue ("controllerLayout", typeof(SettingsScreen.ControllerLayout));
		levelInfo = (Dictionary<string, LevelSaveInfo>)info.GetValue ("levelInfo", typeof(Dictionary<string, LevelSaveInfo>));
	}

	public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		controllerLayout = CONSTANTS.controllerLayout;
		levelInfo = CONSTANTS.levelInfo;

		info.AddValue ("controllerLayout", controllerLayout);
		info.AddValue ("levelInfo", levelInfo);
	}
}

[Serializable ()]
public class SaveLoad {
	public static string currentFilePath = Application.dataPath + "/Problem.save";

	public static bool CheckIfFileExists()
	{
		return File.Exists (currentFilePath);
	}

	public static void Save()
	{
		Save (currentFilePath);
	}

	public static void Save(string filePath)
	{
		SaveData data = new SaveData ();
		Stream stream = File.Open (filePath, FileMode.Create);
		BinaryFormatter bformatter = new BinaryFormatter ();
		bformatter.Binder = new VersionDeserializationBinder ();
		bformatter.Serialize (stream, data);
		stream.Close ();
	}

	public static void Load()
	{
		Load (currentFilePath);
	}

	public static void Load (string filePath)
	{
		SaveData data = new SaveData ();
		Stream stream = File.Open (filePath, FileMode.Open);
		BinaryFormatter bformatter = new BinaryFormatter ();
		bformatter.Binder = new VersionDeserializationBinder ();
		data = (SaveData)bformatter.Deserialize (stream);
		stream.Close ();


		CONSTANTS.controllerLayout = data.controllerLayout;
		CONSTANTS.levelInfo = data.levelInfo;
	}

}

public sealed class VersionDeserializationBinder : SerializationBinder
{
	public override Type BindToType(string assemblyName, string typeName)
	{
		if (!string.IsNullOrEmpty (assemblyName) && !string.IsNullOrEmpty (typeName)) {
			Type typeToDeserialize = null;
			assemblyName = Assembly.GetExecutingAssembly().FullName;
			typeToDeserialize = Type.GetType (String.Format ("{0}, {1}", typeName, assemblyName));
			return typeToDeserialize;
		}
		return null;
	}
}


[Serializable ()]
public class LevelSaveInfo// : ISerializable
{
	public float time;

	public LevelSaveInfo(float time)
	{
		this.time = time;
		SaveLoad.Save ();
	}
		
	public void SaveNewTime(float time)
	{
		if (time < this.time) {
			this.time = time;
			SaveLoad.Save ();
		}
	}

	public string GetTimeInTextFormat()
	{
		string seconds = ((int)(time % 60)).ToString ();
		string minutes = ((int)(time / 60)).ToString ();
		string milliseconds = (time - (int)time).ToString (".##");
		string returnValue = minutes + ":" + seconds + "" + milliseconds;
		return returnValue;
	}
}
