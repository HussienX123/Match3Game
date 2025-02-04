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

        GridManager MyScript = (GridManager)target;

        if (GUILayout.Button("Build Grid"))
        {
            MyScript.BuildGrid();
        }

        if (GUILayout.Button("Randomize Grid Items"))
        {
            MyScript.RandomizeBlocks();
        }
    }
}
