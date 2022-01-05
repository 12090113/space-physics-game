using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(AsteroidGenerator))]
public class AsteroidEditor : Editor
{
    public override void OnInspectorGUI() {
        AsteroidGenerator astrGen = (AsteroidGenerator)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Generate"))
        {
            astrGen.Generate();
        }
    }
}
