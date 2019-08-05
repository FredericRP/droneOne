// CustomEditor tool example.
// Shows a billboard at each vertex position on a selected mesh.

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine.Rendering;

// By passing `typeof(MeshFilter)` as the second argument, we register VertexTool as a CustomEditor tool to be presented
// when the current selection contains a MeshFilter component.
[EditorTool("Show Vertices", typeof(MeshFilter))]
class VertexTool : EditorTool
{
    struct TransformAndPositions
    {
        public Transform transform;
        public IList<Vector3> positions;
    }

    IEnumerable<TransformAndPositions> m_Vertices;
    GUIContent m_ToolbarIcon;

    public override GUIContent toolbarIcon
    {
        get
        {
            if (m_ToolbarIcon == null)
            {
                // Usually you'll want to use an icon (19x18 px to match Unity's icons)
                var icon19x18 = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Examples/Icons/VertexTool.png");

                if (icon19x18 != null)
                    m_ToolbarIcon = new GUIContent(icon19x18, "Vertex Visualization Tool");
                else
                    m_ToolbarIcon = new GUIContent("Vertex Tool", "Vertex Visualization Tool");
            }

            return m_ToolbarIcon;
        }
    }

    // OnEnable/Disable should be used to load and unload resources
    void OnEnable()
    {
        EditorTools.activeToolChanging += OnActiveToolWillChange;
        EditorTools.activeToolChanged += OnActiveToolDidChange;
    }

    void OnDisable()
    {
        EditorTools.activeToolChanging -= OnActiveToolWillChange;
        EditorTools.activeToolChanged -= OnActiveToolDidChange;
    }

    void OnActiveToolWillChange()
    {
        if (EditorTools.IsActiveTool(this))
            OnDeactivate();
    }

    void OnActiveToolDidChange()
    {
        if (EditorTools.IsActiveTool(this))
        {
            Debug.Log("Activating SomeTool");
            OnActivate();
        }
    }

    // Called when an EditorTool is made the active tool.
    public void OnActivate()
    {
        Selection.selectionChanged += RebuildVertexPositions;
        RebuildVertexPositions();
    }

    // Called when the active tool is changed.
    public void OnDeactivate()
    {
        Selection.selectionChanged -= RebuildVertexPositions;
    }

    void RebuildVertexPositions()
    {
        m_Vertices = targets.Select(x =>
        {
            return new TransformAndPositions()
            {
                transform = ((MeshFilter)x).transform,
                positions = ((MeshFilter)x).sharedMesh.vertices
            };
        }).ToArray();
    }

    // If you've implemented scene tools before, think of this like the `OnSceneGUI` method. This is where you put the
    // implementation of your tool.
    public override void OnToolGUI(EditorWindow window)
    {
        var evt = Event.current;

        var zTest = Handles.zTest;
        Handles.zTest = CompareFunction.LessEqual;

        foreach (var entry in m_Vertices)
        {
            var size = HandleUtility.GetHandleSize(entry.transform.position) * .05f;
            DrawHandleCaps(entry.transform.localToWorldMatrix, entry.positions, size);
        }

        Handles.zTest = zTest;
    }

    static void DrawHandleCaps(Matrix4x4 matrix, IList<Vector3> positions, float size)
    {
        if (Event.current.type != EventType.Repaint)
            return;

        Vector3 sideways = (Camera.current == null ? Vector3.right : Camera.current.transform.right) * size;
        Vector3 up = (Camera.current == null ? Vector3.up : Camera.current.transform.up) * size;
        Color col = Handles.color * new Color(1, 1, 1, 0.99f);

        // After drawing the first dot cap, the handle material and matrix are set up, so there's no need to keep
        // resetting the state.
        Handles.DotHandleCap(0, matrix.MultiplyPoint(positions[0]), Quaternion.identity,
            HandleUtility.GetHandleSize(matrix.MultiplyPoint(positions[0])) * .05f, EventType.Repaint);

        GL.Begin(GL.QUADS);

        for (int i = 1, c = positions.Count; i < c; i++)
        {
            var position = matrix.MultiplyPoint(positions[i]);

            GL.Color(col);
            GL.Vertex(position + sideways + up);
            GL.Vertex(position + sideways - up);
            GL.Vertex(position - sideways - up);
            GL.Vertex(position - sideways + up);
        }

        GL.End();
    }
}