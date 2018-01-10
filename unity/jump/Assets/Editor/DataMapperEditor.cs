using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DataMapperEditor : Editor {

	private SerializedProperty dataFile;

	void  OnEnable()
	{
		dataFile = serializedObject.FindProperty("dataFile");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUILayout.PropertyField(dataFile);
		serializedObject.ApplyModifiedProperties();
	}
}
