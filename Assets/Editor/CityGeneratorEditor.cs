using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(City), true)]
public class CityGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        City mapPreview = (City)target;

        if (GUILayout.Button("Generate"))
        {
            Debug.ClearDeveloperConsole();
            mapPreview.GenerateCity();
        }

        if (GUILayout.Button("Delete"))
        {
            Debug.ClearDeveloperConsole();
            mapPreview.ResetCity();
        }
    }
}
