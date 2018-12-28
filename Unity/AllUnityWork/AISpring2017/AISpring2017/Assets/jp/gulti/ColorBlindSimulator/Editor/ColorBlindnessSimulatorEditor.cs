using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace jp.gulti.ColorBlind
{
	[CustomEditor(typeof(ColorBlindnessSimulator))]
	public class ColorBlindnessSimulatorEditor : Editor
	{
		private ColorBlindnessSimulator Target
		{
			get
			{
				if (__Target == null) __Target = (ColorBlindnessSimulator)target;
				return __Target;
			}
		}
		private ColorBlindnessSimulator __Target;
	
		public override void OnInspectorGUI()
		{
			Game_Manager.Instance.BlindMode =  (Game_Manager.ColorBlindMode)EditorGUILayout.EnumPopup("Color Blind Type:", Game_Manager.Instance.BlindMode);
			switch (Game_Manager.Instance.BlindMode)
			{
				case Game_Manager.ColorBlindMode.Protanope:
					GUILayout.BeginVertical("box");
					GUILayout.Label("Protanope(Type I):\nRed Weakness.\n1% of males, 0.01% of females.");
					GUILayout.EndVertical();
					break;
				case Game_Manager.ColorBlindMode.Deuteranope:
					GUILayout.BeginVertical("box");
					GUILayout.Label("Deuteranope(Type II):\nGreen Weakness.\nmost common—6% of males, 0.4% of females");
					GUILayout.EndVertical();
					break;
				default:
					break;
			}
			

			Target.BlindIntensity = EditorGUILayout.Slider("Simulate Intensity", Target.BlindIntensity, 0.0f, 1.0f);

			if(GUI.changed)
			{
				EditorUtility.SetDirty(Target);
			}
		}

		//Menu Item
		[MenuItem("GULTI/Attach Color Blindness Simulator")]
		public static void AttachCBSimulatorFromMenu()
		{
			var sel = Selection.activeGameObject;
			if (sel != null)
				sel.AddComponent<ColorBlindnessSimulator>();
			else
				Debug.LogError("Select any GameObject.");
		}
	}
}