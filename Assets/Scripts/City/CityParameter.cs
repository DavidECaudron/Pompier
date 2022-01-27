using UnityEngine;

[CreateAssetMenu(fileName = "City", menuName = "CityParameter")]
public class CityParameter : ScriptableObject
{
    [Min(10)] public int Size = 10;
    [Min(1)] public int Scale = 1;
}
