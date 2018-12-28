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
	public string mapName = "";
	public float[] playerLocation = new float[2];
	public List<Quest> activeQuests;
	public List<Quest> completedQuests;

	public SaveData()
	{

	}

	public SaveData(SerializationInfo info, StreamingContext ctxt)
	{
		mapName = (string)info.GetValue ("mapName", typeof(string));
		playerLocation = (float[])info.GetValue ("playerLocation", typeof(float[]));
		activeQuests = (List<Quest>)info.GetValue ("activeQuests", typeof(List<Quest>));
		completedQuests = (List<Quest>)info.GetValue ("completedQuests", typeof(List<Quest>));
	}

	public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		mapName = Application.loadedLevelName;
		playerLocation = new float[2] {
			GameObject.Find ("Character").transform.position.x,
			GameObject.Find ("Character").transform.position.y
		};
		activeQuests = GAMECONSTANTS.activeQuests;
		completedQuests = GAMECONSTANTS.completedQuests;


		info.AddValue ("mapName", mapName);
		info.AddValue ("playerLocation", playerLocation);
		info.AddValue ("activeQuests", activeQuests);
		info.AddValue ("completedQuests", completedQuests);
	}
}

public class SaveLoad {
	public static string currentFilePath = Application.dataPath + "/SaveData.ftw";

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


		GAMECONSTANTS.activeQuests = data.activeQuests;
		GAMECONSTANTS.completedQuests = data.completedQuests;
		GAMECONSTANTS.playerLocation = data.playerLocation;
		Application.LoadLevel (data.mapName);
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



