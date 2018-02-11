using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraUtil))]
public class CameraUtilEditor : Editor {

    public override void OnInspectorGUI() {
        CameraUtil cameraUtil = target as CameraUtil;


        DrawDefaultInspector();
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("ResetCamera")) {
            Reset();
        }

        if (GUILayout.Button("LookAtOrigin")) {
            LookAtOrigin();
        }

        GUILayout.EndHorizontal();
    }

    public void LookAtOrigin(){
        CameraUtil cameraUtil = target as CameraUtil;
        Transform origin = cameraUtil.origin;
        if(origin != null){
            cameraUtil.transform.LookAt(origin);
        }
    }

    public void Reset(){
        LookAtOrigin();
        CameraUtil cameraUtil = target as CameraUtil;
        cameraUtil.transform.position = new Vector3(-17, 30, -26);
    }

}
