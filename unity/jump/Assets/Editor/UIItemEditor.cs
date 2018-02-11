﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(UIItem))]
public class UIItemEditor : Editor {

    public float ToFixed(float r){
        return r - r % 0.1f;
    }

    public override void OnInspectorGUI() {
        UIItem item = target as UIItem;


        RectTransform rect = item.GetComponent<RectTransform>();
        RectTransform root = rect.root.GetComponent<RectTransform>();
        Vector2 size     = root.sizeDelta;
        Vector2 itemSize = rect.sizeDelta;
        CanvasScaler cs  = root.GetComponent<CanvasScaler>();

        Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(root, rect);

        Vector2 center = new Vector2(
            bounds.center.x + cs.referenceResolution.x / 2, 
            cs.referenceResolution.y/2 - bounds.center.y  );
        
        Vector2 pivot = rect.pivot - new Vector2(0.5f,0.5f);
        pivot.y =  - pivot.y;
        center  = center + new Vector2(pivot.x * itemSize.x, pivot.y * itemSize.y);

        center.x = ToFixed(center.x);
        center.y = ToFixed(center.y);

        EditorGUILayout.Vector2Field("position",center);

        EditorGUILayout.Vector2Field("min",new Vector2(
            ToFixed(cs.referenceResolution.x / 2 + bounds.min.x),
            ToFixed(cs.referenceResolution.y / 2 - bounds.min.y - itemSize.y)
        ));

        EditorGUILayout.Vector2Field("max",new Vector2(
            ToFixed(cs.referenceResolution.x / 2 + bounds.max.x),
            ToFixed(cs.referenceResolution.y / 2 - bounds.max.y + itemSize.y)
        ));


       

        //if (GUILayout.Button("LookAtOrigin")) {
            
        //}

        //GUILayout.EndHorizontal();
    }
}
