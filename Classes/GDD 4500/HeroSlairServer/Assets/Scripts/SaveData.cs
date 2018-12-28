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
	public string levelData;

	public SaveData()
	{

	}

	public SaveData(string content)
	{
		levelData = content;
	}

	public SaveData(SerializationInfo info, StreamingContext ctxt)
	{
		levelData = (string)info.GetValue ("levelData", typeof(string));
	}

	public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		info.AddValue ("levelData", levelData);
	}
}

public class SaveLoad {
	public static string currentFilePath = Application.dataPath + "/Levels/";

	public static void Save()
	{
		Save (currentFilePath, "asdAsd");
	}

	public static void Save(string fileName, string content)
	{
		SaveData data = new SaveData (content);
		Stream stream = File.Open (currentFilePath + fileName + ".lvl", FileMode.Create);
		BinaryFormatter bformatter = new BinaryFormatter ();
		bformatter.Binder = new VersionDeserializationBinder ();
		bformatter.Serialize (stream, data);
		stream.Close ();
	}

	public static void Load()
	{
		Load (currentFilePath);
	}

	public static string Load (string fileName)
	{
		SaveData data = new SaveData ();
		Stream stream = File.Open (currentFilePath + fileName, FileMode.Open);
		BinaryFormatter bformatter = new BinaryFormatter ();
		bformatter.Binder = new VersionDeserializationBinder ();
		data = (SaveData)bformatter.Deserialize (stream);
		stream.Close ();

		return data.levelData;
//		GAMECONSTANTS.activeQuests = data.activeQuests;
//		GAMECONSTANTS.completedQuests = data.completedQuests;
//		GAMECONSTANTS.playerLocation = data.playerLocation;
//		Application.LoadLevel (data.mapName);
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



