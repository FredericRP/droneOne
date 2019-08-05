// Example of a global tool.
// This is a "super" transform handle that shows the position handles for all 6 directions, as well as a rotation
// handle.

using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

[EditorTool("Super Transform Tool")]
public class SimpleGlobalTool : EditorTool
{
    GUIContent m_ToolbarIcon;

    public override GUIContent toolbarIcon
    {
        get
        {
            if (m_ToolbarIcon == null)
                m_ToolbarIcon = new GUIContent(
                    AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Examples/Icons/SimpleIcon.png"),
                    "Simple Global Tool");
            return m_ToolbarIcon;
        }
    }

    public override void OnToolGUI(EditorWindow window)
    {
        var sceneView = window as SceneView;

        if (sceneView == null)
            return;

        foreach (var trs in Selection.transforms)
        {
            EditorGUI.BeginChangeCheck();

            var rot = trs.rotation;
            var pos = trs.position;

            Handles.color = Color.green;
            pos = Handles.Slider(pos, trs.up);
            pos = Handles.Slider(pos, -trs.up);

            Handles.color = Color.red;
            pos = Handles.Slider(pos, trs.right);
            pos = Handles.Slider(pos, -trs.right);

            Handles.color = Color.blue;
            pos = Handles.Slider(pos, trs.forward);
            pos = Handles.Slider(pos, -trs.forward);

            rot = Handles.RotationHandle(rot, pos);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(trs, "Simple Transform Tool");
                trs.position = pos;
                trs.rotation = rot;
            }
        }
    }
}