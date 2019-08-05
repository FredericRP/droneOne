using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrefabPainter : EditorWindow
{
    [SerializeField]
    GameObject[] prefabList;
    [SerializeField]
    bool paint;

    int index = 0;

    Plane m_Plane = new Plane(Vector3.up, Vector3.zero);
    LevelData levelData;
    PrefabConstraint prefabConstraint;
    Transform parent;

    [MenuItem("Window/FredericRP/Prefab Painter")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        PrefabPainter window = (PrefabPainter)EditorWindow.GetWindow(typeof(PrefabPainter));
        window.Show();
    }

    private void OnGUI()
    {
        prefabConstraint = (PrefabConstraint)EditorGUILayout.ObjectField(prefabConstraint, typeof(PrefabConstraint), false);
        if (GUILayout.Button("Recreate"))
        {
            CreateObjectsFromData();
        }
        paint = EditorGUILayout.Toggle(new GUIContent("Paint"), paint);
    }

    void OnEnable()
    {
        SceneView.duringSceneGui += this.OnSceneGUI;
        EditorSceneManager.activeSceneChangedInEditMode += RefreshObjectFromScene;
        levelData = AssetDatabase.LoadAssetAtPath<LevelData>("Assets/level.asset");
        if (levelData == null)
        {
            InitializeData();
        }
        prefabConstraint = AssetDatabase.LoadAssetAtPath<PrefabConstraint>("Assets/prefabs.asset");
        if (prefabConstraint == null)
        {
            InitializePrefabData();
        }
        CreateObjectsFromData();
    }

    void InitializeData()
    {
        Debug.Log("New level data asset");
        // Create asset, save empty one and refresh database to show it to the user
        levelData = ScriptableObject.CreateInstance<LevelData>();
        AssetDatabase.CreateAsset(levelData, "Assets/level.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void InitializePrefabData()
    {
        Debug.Log("New prefab constraint data asset");
        // Create asset, save empty one and refresh database to show it to the user
        prefabConstraint = ScriptableObject.CreateInstance<PrefabConstraint>();
        AssetDatabase.CreateAsset(prefabConstraint, "Assets/prefabs.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
        EditorSceneManager.activeSceneChangedInEditMode -= RefreshObjectFromScene;
        AssetDatabase.SaveAssets();
    }

    // This is called for each window that your tool is active in. Put the functionality of your tool here.
    public void OnSceneGUI(SceneView sceneView)
    {
        if (!paint)
            return;
        var evt = Event.current;
        // If clicking...
        if (!evt.isMouse || evt.button != 0 || evt.type != EventType.MouseDown || evt.modifiers != 0)
            return;

        //Create a ray from the Mouse click position
        float enter = 0.0f;

        // Check if pointing at something or nothing
        // Unity 2019.2.0f1 invert height in inputPosition
        Vector2 screenPosition = new Vector2(evt.mousePosition.x, sceneView.camera.pixelHeight - evt.mousePosition.y);
        Ray ray = sceneView.camera.ScreenPointToRay(screenPosition);
        //Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 5);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            Debug.Log("Touch existing one");
            // If something is hit : add a room above the one pointed, if there are no room yet
            if (hitInfo.collider.gameObject.layer == 30)
            {
                // If not block, put one there
                int x = Mathf.RoundToInt(hitInfo.collider.gameObject.transform.position.x / 10);
                int y = Mathf.RoundToInt(hitInfo.collider.gameObject.transform.position.y / 10);
                int z = Mathf.RoundToInt(hitInfo.collider.gameObject.transform.position.z / 10);
                if (!levelData.DoesExists(x, y, z))
                    CreateBlockAt(x, y, z);
                // Put one at the top otherwise
                else if (!levelData.DoesExists(x, y+1, z))
                    CreateBlockAt(x, y+1, z);
            }
        }
        else if (m_Plane.Raycast(ray, out enter))
        {
            // If nothing : add a room there
            // take intersection with 0 height plane
            //Get the point that is clicked
            Vector3 hitPoint = ray.GetPoint(enter);
            // Cube side is 10 units
            int x = Mathf.RoundToInt(hitPoint.x / 10);
            int z = Mathf.RoundToInt(hitPoint.z / 10);

            CreateBlockAt(x, 0, z);
            evt.Use();
        }
        //EditorGUI.BeginChangeCheck();

        //if (EditorGUI.EndChangeCheck())
        //{
        //}
    }

    void CreateBlockAt(int x, int y, int z)
    {
        CreateObjectAt(x, y, z);
        if (levelData == null)
            InitializeData();
        levelData.SetCell(x, y, z, 1);
        Debug.Log("Set Cell @" + x + ", " + y + ", " + z);
        EditorUtility.SetDirty(levelData);
    }

    void CreateObjectAt(int x, int y, int z, GameObject prefab = null)
    {
        if (prefab == null)
            prefab = prefabConstraint.prefabDataList[0].prefab;
        GameObject go = GameObject.Instantiate(prefab, parent);
        //go.hideFlags = HideFlags.HideAndDontSave;
        //go.transform.SetParent(parent, true);
        go.transform.position = new Vector3(x, y, z) * 10;
        //go.transform.localScale = Vector3.one * 10;
        go.layer = 30;
    }

    void CreateObjectsFromData()
    {
        GameObject parentGO = GameObject.Find("FredericRP_LevelDesignData");
        if (parentGO == null)
        {
            parentGO = new GameObject("FredericRP_LevelDesignData");
            //parentGO.hideFlags = HideFlags.HideAndDontSave;
            parentGO.transform.position = Vector3.zero;
            parentGO.transform.rotation = Quaternion.identity;
        }
        parent = parentGO.transform;

        // Delete children first
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(parent.GetChild(i).gameObject);
        }

        // Add objects from level data list
        for (int i = 0; i < levelData.CellCount; i++)
        {
            LevelData.LevelCell cell = levelData.GetCell(i);
            CreateObjectAt(cell.x, cell.y, cell.z);
        }
        // Add auto neighbours
            int index = prefabConstraint.prefabDataList.FindIndex(element => element.blockedCells.Count > 0);
        Debug.Log("Side @" + index);
        for (int i = 0; i < levelData.CellCount; i++)
        {
            LevelData.LevelCell cell = levelData.GetCell(i);
            if (!levelData.DoesExists(cell.x, cell.y, cell.z + 1))
                CreateObjectAt(cell.x, cell.y, cell.z + 1, prefabConstraint.prefabDataList[index].prefab);
        }
    }

    /// <summary>
    /// Checks if we need to re instantiate hidden cubes
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="scene"></param>
    void RefreshObjectFromScene(Scene sceneIn, Scene sceneOut)
    {
        if (levelData == null)
            InitializeData();
        if (parent == null || parent.childCount != levelData.CellCount)
        {
            CreateObjectsFromData();
        }
    }


    /*
    Vector3 position = Tools.handlePosition;

    using (new Handles.DrawingScope(Color.green))
    {
        position = Handles.Slider(position, Vector3.right);
    }

    if (EditorGUI.EndChangeCheck())
    {
        Vector3 delta = position - Tools.handlePosition;

        Undo.RecordObjects(Selection.transforms, "Move Platform");

        foreach (var transform in Selection.transforms)
            transform.position += delta;
    }
    // */

}
