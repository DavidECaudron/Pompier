using UnityEngine;

[CreateAssetMenu(fileName = "City", menuName = "ElementCityParameter")]
public class ElementCityParameter : ScriptableObject
{
    public Material Material;
    public GameObject[] Prefabs;
    public GameObject[] Meshes;
    public bool UseCustomScale = false;
    public Vector3 Scale = new Vector3(1f, 1f, 1f);
    public bool GeneratePlane = true;
}
