using UnityEngine;
using System.Collections;
using UtilitySystems.XmlDatabase.Editor;
using System;
using UnityEditor;

public class RPGStatScalerLinearExtension : EditorExtension {
    public override bool CanHandleType(Type type) {
        return typeof(RPGStatScalerLinearAsset).IsAssignableFrom(type);
    }

    public override void OnGUI(object asset) {
        var linear = (RPGStatScalerLinearAsset)asset;

        linear.Slope = EditorGUILayout.FloatField("Slope", linear.Slope);
        linear.Offset = EditorGUILayout.FloatField("Offset", linear.Offset);
    }
}
