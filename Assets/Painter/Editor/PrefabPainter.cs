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

    int prefabIndex = 0;
    int rotationIndex = 0;
    float scrollRatio = 0.3f;
    // Math object to the grid with this offset
    Vector3 objectOffset = Vector3.one * 5;

    Plane m_Plane = new Plane(Vector3.up, Vector3.zero);
    LevelData levelData;
    LevelPrefabList prefabConstraint;
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
        prefabConstraint = (LevelPrefabList)EditorGUILayout.ObjectField(prefabConstraint, typeof(LevelPrefabList), false);
        if (GUILayout.Button("Recreate"))
        {
            CreateObjectsFromData();
        }
        paint = EditorGUILayout.Toggle(new GUIContent("Paint"), paint);

        GUILayout.Label(new GUIContent("Prefab #" + prefabIndex, AssetPreview.GetAssetPreview(prefabConstraint.prefabDataList[prefabIndex].prefabList[0])));
        GUILayout.Label("Rotation " + prefabConstraint.rotationList[rotationIndex]);
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
        prefabConstraint = AssetDatabase.LoadAssetAtPath<LevelPrefabList>("Assets/prefabs.asset");
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
        prefabConstraint = ScriptableObject.CreateInstance<LevelPrefabList>();
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

        // Shift + Mouse wheel : change prefab index
        if ((evt.modifiers & EventModifiers.Shift) != 0 && evt.isScrollWheel)
        {
            prefabIndex += Mathf.RoundToInt(evt.delta.y * scrollRatio);
            // roll around the list
            if (prefabIndex < 0)
                prefabIndex += prefabConstraint.prefabDataList.Count;
            prefabIndex = prefabIndex % prefabConstraint.prefabDataList.Count;
            Debug.Log("Scroll by " + evt.delta + " to:" + prefabIndex + " with " + Mathf.RoundToInt(evt.delta.y * scrollRatio) + " with " + prefabConstraint.prefabDataList.Count + " prefab(s)");
            evt.Use();
            this.Repaint();
            return;
        }

        // Alt + Mouse wheel : change prefab rotation
        if ((evt.modifiers & EventModifiers.Alt) != 0 && evt.isScrollWheel)
        {
            rotationIndex += Mathf.RoundToInt(evt.delta.y * scrollRatio);
            // roll around the list
            if (rotationIndex < 0)
                rotationIndex += prefabConstraint.rotationList.Count;
            rotationIndex = rotationIndex % prefabConstraint.rotationList.Count;
            Debug.Log("Scroll by " + evt.delta + " to:" + rotationIndex + " with " + Mathf.RoundToInt(evt.delta.y * scrollRatio) + " with " + prefabConstraint.rotationList.Count + " rotation(s)");
            evt.Use();
            this.Repaint();
            return;
        }

        // Create a ray from the Mouse click position
        float enter = 0.0f;
        int x = 0, y = 0, z = 0;
        bool found = false;

        // Check if pointing at something or nothing
        // Unity 2019.2.0f1 invert height in inputPosition
        Vector3Int direction = Vector3Int.zero;
        Vector2 screenPosition = new Vector2(evt.mousePosition.x, sceneView.camera.pixelHeight - evt.mousePosition.y);
        Ray ray = sceneView.camera.ScreenPointToRay(screenPosition);
        //Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 5);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.gameObject.layer == 30)
        {
            // Touch something : take its position and normal
            x = Mathf.RoundToInt((hitInfo.collider.gameObject.transform.position.x - objectOffset.x) / 10);
            y = Mathf.RoundToInt((hitInfo.collider.gameObject.transform.position.y - objectOffset.y) / 10);
            z = Mathf.RoundToInt((hitInfo.collider.gameObject.transform.position.z - objectOffset.z) / 10);
            direction = new Vector3Int((int)hitInfo.normal.x, (int)hitInfo.normal.y, (int)hitInfo.normal.z);
            found = true;
        }
        else if (m_Plane.Raycast(ray, out enter))
        {
            // If nothing : add a room there
            // take intersection with 0 height plane
            //Get the point that is clicked
            Vector3 hitPoint = ray.GetPoint(enter);
            // Cube side is 10 units - move by 5 units so the round fits perfectly the grid sides
            x = Mathf.RoundToInt((hitPoint.x - 5) / 10);
            y = 0;
            z = Mathf.RoundToInt((hitPoint.z - 5) / 10);

            found = true;
        }

        if (found)
        {
            // Draw gizmos
            Vector3 handlePosition = (new Vector3(x, y, z)) * 10 + objectOffset;
            Handles.color = Color.yellow;
            Handles.DrawWireCube(handlePosition, Vector3.one * 10);
            Quaternion rotation = Quaternion.identity;

            // Point direction where prefab will be instantiated if outside the centered cube
            if (direction != Vector3Int.zero)
            {
                Handles.color = Color.cyan;
                rotation = Quaternion.FromToRotation(Vector3.forward, direction);
                Handles.ArrowHandleCap(1, handlePosition, rotation, 5, evt.type);
                // restrict prefab rotation on Y axis only
                if (direction.y != 0)
                    rotation = Quaternion.identity;
            }

            // If clicking on the left button, create block
            if (evt.button == 0 && evt.type == EventType.MouseDown)
            {
                // Create block
                if ((evt.modifiers & EventModifiers.Control) != 0)
                {
                    // Delete block
                    if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == 30)
                        DestroyImmediate(hitInfo.collider.gameObject);
                    if (levelData.DoesExists(x, y, z))
                    {
                        levelData.DeleteCell(x, y, z);
                    }
                    evt.Use();
                }
                else if (evt.modifiers == 0)
                {
                    evt.Use();
                    if (!levelData.DoesExists(x, y, z))
                    {
                        // Delete existing block if that was not a full block
                        //if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == 30)
                        //DestroyImmediate(hitInfo.collider.gameObject);
                        CreateBlockAt(x, y, z);
                    }
                    // Put one at the top if first one can not be placed
                    else if (!levelData.DoesExists(x + direction.x, y + direction.y, z + direction.z))
                        CreateBlockAt(x + direction.x, y + direction.y, z + direction.z);
                }
            }
        }

        // Force repaint to update wireframe cube position
        SceneView.RepaintAll();
    }

    void CreateBlockAt(int x, int y, int z)
    {
        CreateObjectAt(x, y, z, Quaternion.Euler(prefabConstraint.rotationList[rotationIndex]), prefabConstraint.prefabDataList[prefabIndex].prefabList[0]);
        if (levelData == null)
            InitializeData();
        levelData.SetCell(x, y, z, prefabIndex, Quaternion.Euler(prefabConstraint.rotationList[rotationIndex]));
        EditorUtility.SetDirty(levelData);
    }

    void CreateObjectAt(int x, int y, int z, Quaternion rotation, GameObject prefab = null)
    {
        if (prefab == null)
            prefab = prefabConstraint.prefabDataList[0].prefabList[0];
        GameObject go = GameObject.Instantiate(prefab, parent);
        // go.hideFlags = HideFlags.HideAndDontSave;
        go.transform.position = new Vector3(x, y, z) * 10 + objectOffset;
        go.transform.rotation = rotation;
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
            CreateObjectAt(cell.x, cell.y, cell.z, cell.rotation, prefabConstraint.prefabDataList[cell.prefabIndex].prefabList[0]);
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

}
