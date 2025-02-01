using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Hussien;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridManager myScript = (GridManager)target;
        if (GUILayout.Button("Build Grid"))
        {
            myScript.BuildGrid();
        }
    }
}
