using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(State))]
public class StateEditor : Editor
{
    private State state;
    float[,] debug2d;

    private void OnEnable()
    {
        state = target as State;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        debug2d = new float[40, 4];
        // debug2d = new float[96, 4];
        debug2d = state.state.Clone() as float[,];

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("South / East / North / West");
        EditorGUILayout.EndHorizontal();
        for (int i = 0; i < 10; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Time " + (i + 1).ToString() + " South");
            for (int k = 0; k < 4; k++)
                debug2d[i, k] = EditorGUILayout.FloatField(debug2d[i * 4, k]);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Time " + (i + 1).ToString() + " East");
            for (int k = 0; k < 4; k++)
                debug2d[i, k] = EditorGUILayout.FloatField(debug2d[(i * 4) + 1, k]);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Time " + (i + 1).ToString() + " North");
            for (int k = 0; k < 4; k++)
                debug2d[i, k] = EditorGUILayout.FloatField(debug2d[(i * 4) + 2, k]);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Time " + (i + 1).ToString() + " West");
            for (int k = 0; k < 4; k++)
                debug2d[i, k] = EditorGUILayout.FloatField(debug2d[(i * 4) + 3, k]);
            EditorGUILayout.EndHorizontal();
        }
    }
}
