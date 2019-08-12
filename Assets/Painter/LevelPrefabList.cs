using System.Collections.Generic;
using UnityEngine;

public class LevelPrefabList : ScriptableObject
{
    [System.Serializable]
    public class PrefabData
    {
        public GameObject[] prefabList;
    }

    public List<PrefabData> prefabDataList;

    public List<Vector3> rotationList;
}
