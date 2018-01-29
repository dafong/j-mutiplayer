using UnityEngine;
using System.Collections;
using UnityEditor;


public class EditorExtend : MonoBehaviour{
    [MenuItem("Tools/LookAt")]
    public static void LookAtGameObject(){
        Transform trans = Selection.activeTransform;        
        trans.LookAt(new Vector3(13, 0, -4));
    }

}
