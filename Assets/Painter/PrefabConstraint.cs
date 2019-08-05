using System.Collections.Generic;
using UnityEngine;

public class PrefabConstraint : ScriptableObject
{
    [System.Serializable]
    public class PrefabData
    {
        public GameObject prefab;
        /// <summary>
        /// Relative coordinates that must not be empty to use this prefab
        /// </summary>
        public List<Vector3> blockedCells;
    }

    public List<PrefabData> prefabDataList;
}
