using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FredericRP.DroneEngine
{
  [CustomPropertyDrawer(typeof(DroneConfiguration.RotationAngleClamp))]
  public class RotationAngleClampDrawer : PropertyDrawer
  {
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      return EditorGUIUtility.singleLineHeight;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      // always act as if expanded
      property.isExpanded = true;
      position = EditorGUI.PrefixLabel(position, label);
      SerializedProperty clampProperty = property.FindPropertyRelative("clamp");
      SerializedProperty minProperty = property.FindPropertyRelative("min");
      float min = minProperty.floatValue;
      SerializedProperty maxProperty = property.FindPropertyRelative("max");
      float max = maxProperty.floatValue;
      bool clamped = clampProperty.boolValue;
      EditorGUI.BeginChangeCheck();
      if (clamped)
      {
        // Show clamp button, but on 1/10th
        Rect buttonRect = new Rect(position);
        buttonRect.width = 64;
        clamped = GUI.Toggle(buttonRect, clamped, "clamp", EditorStyles.miniButton);
        // Show min/max slider with min/max fields
        Rect minMaxRect = new Rect(position);
        minMaxRect.width = position.width - buttonRect.width - 68;
        minMaxRect.x += buttonRect.width + 36; // +4 +32
        // put min rect on the left side
        Rect minRect = new Rect(minMaxRect);
        minRect.width = 32;
        minRect.x -= 32; // 32 left from the minMaxRect
        // set maxRect to be after minmax slider
        Rect maxRect = new Rect(minMaxRect);
        maxRect.width = 32;
        maxRect.x += minMaxRect.width;
        min = EditorGUI.FloatField(minRect, min);
        EditorGUI.MinMaxSlider(minMaxRect, ref min, ref max, -180, 180);
        max = EditorGUI.FloatField(maxRect, max);
      } else
      {
        // Show clamp button, on 90%
        clamped = GUI.Toggle(position, clamped, "clamp", EditorStyles.miniButton);
      }
      if (EditorGUI.EndChangeCheck())
      {
        clampProperty.boolValue = clamped;
        minProperty.floatValue = min;
        maxProperty.floatValue = max;
      }
    }

    
  }
}