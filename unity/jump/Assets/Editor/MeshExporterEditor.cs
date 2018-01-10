using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshExporter))]
public class MeshExporterEditor : Editor {

	private SerializedProperty datas;

	void  OnEnable()
	{
		datas = serializedObject.FindProperty("datas");
	}

	public override void OnInspectorGUI()
	{
		MeshExporter exporter = target as MeshExporter;

		DrawDefaultInspector();
		GUILayout.BeginHorizontal();

		if(GUILayout.Button("Convert")){
			exporter.Export ();
		}

		if(GUILayout.Button("Reset")){
			exporter.datas = null;
		}

		GUILayout.EndHorizontal();
	}
}
