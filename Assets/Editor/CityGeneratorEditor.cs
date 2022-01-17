using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(City), true)]
public class CityGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        City mapPreview = (City)target;

        if (GUILayout.Button("Generate"))
        {
            mapPreview.GenerateCity();
        }
    }
}
