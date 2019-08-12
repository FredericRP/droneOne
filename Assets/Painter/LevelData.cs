using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : ScriptableObject
{
    [System.Serializable]
    public class LevelCell
    {
        public int x, y, z;
        public int prefabIndex;
        public Quaternion rotation;
    }

    [SerializeField]
    List<LevelCell> levelCellList;

    public int CellCount { get { return levelCellList.Count; } }

    public LevelCell GetCell(int x, int y, int z)
    {
        int index = levelCellList.FindIndex(element => (element.x == x) && (element.y == y) && (element.z == z));
        if (index > -1 && index < levelCellList.Count)
            return levelCellList[index];
        return null;
    }

    public LevelCell GetCell(int index)
    {
        return levelCellList[index];
    }

    public void DeleteCell(int x, int y, int z)
    {
        int index = levelCellList.FindIndex(element => (element.x == x) && (element.y == y) && (element.z == z));
        if (index > -1 && index < levelCellList.Count)
            levelCellList.RemoveAt(index);
    }

    public void SetCell(int x, int y, int z, int level, Quaternion rotation)
    {
        if (levelCellList == null)
            levelCellList = new List<LevelCell>();
        LevelCell cell = levelCellList.Find(element => (element.x == x) && (element.y == y) && (element.z == z));
        if (cell == default)
        {
            cell = new LevelCell();
            cell.x = x;
            cell.y = y;
            cell.z = z;
            cell.rotation = rotation;
            levelCellList.Add(cell);
        }
        cell.prefabIndex = level;
    }

    public bool DoesExists(int x, int y, int z)
    {
        bool result = levelCellList.Find(element => (element.x == x) && (element.y == y) && (element.z == z)) != null;
        //Debug.Log("DoesExists " + x + ", " + y + ", " + z + " => " + result);
        return result;
    }
}
