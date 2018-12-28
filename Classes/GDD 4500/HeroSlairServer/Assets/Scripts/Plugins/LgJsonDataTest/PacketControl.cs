using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LgOctEngine.CoreClasses;

public class PacketControl : LgBaseClass {

	Level simpleArrayClass;

	public PacketControl()
	{
		this.simpleArrayClass = LgJsonNode.Create<Level>();
	}

	public void AddToArray(string id, int row, int column, float rotation, int status)
	{
		LevelObject levelObject = CreateLevelObject(id, row, column, rotation, status);
		simpleArrayClass.LevelObjectArray.Add(levelObject);
	}

	public LevelMessage SerializePacket()
	{
		//return simpleArrayClass.Serialize ();
		LevelMessage newMessage = new LevelMessage ();
		newMessage.Message = simpleArrayClass.Serialize ();
		return newMessage;
	}

	public Level DeserializePacket(string serialized)
	{
		return LgJsonNode.CreateFromJsonString<Level>(serialized);
	}

	public class LevelObject : LgJsonDictionary
	{
		public string id { get { return GetValue<string>("id", ""); } set { SetValue<string>("id", value); } }       // The unique string identifier that corresponds to the Prefab to load
		public int row { get { return GetValue<int>("row", 0); } set { SetValue<int>("row", value); } }        // The row of the "grid" that the object occupies
		public int column { get { return GetValue<int>("column", 0); } set { SetValue<int>("column", value); } }      // The column of the "grid" that the object occupies
		public float rotation { get { return GetValue<float>("rotation", 0); } set { SetValue<float>("rotation", value); } }  // The rotation of the object, in degrees, in a clockwise manner.  A zero rotation would be "upright".
		public int status { get { return GetValue<int>("status", 0); } set { SetValue<int>("status", value); } }     // The status of the object.  If an object does not 
		
	}

	public class Level : LgJsonDictionary
	{
		public LgJsonArray<LevelObject> LevelObjectArray {
			get { return GetNode<LgJsonArray<LevelObject>>("Level"); }
			set { SetNode<LgJsonArray<LevelObject>>("Level", value); } }
	}

	private static LevelObject CreateLevelObject(string id, int row, int column, float rotation, int status)
	{
		LevelObject levelObject = LgJsonNode.Create<LevelObject>();
		
		levelObject.id = id;
		levelObject.row = row;
		levelObject.column = column;
		levelObject.rotation = rotation;
		levelObject.status = status;
		
		return levelObject;
	}
}
