using UnityEngine;

[System.Serializable]
public struct Position
{
    public int X;
    public int Y;
}


[System.Serializable]
public class CellMap
{
    public EnumElementCity CellType;
    public GameObject Mesh;
}

public class CellData : MonoBehaviour
{
    public CellMap CellMap;
}
