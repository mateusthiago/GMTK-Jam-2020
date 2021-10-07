using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Composite_Behavour))]
public class CompositeBehaviourEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Composite_Behavour cb = (Composite_Behavour)target;

        if (cb.behaviours == null || cb.behaviours.Length == 0)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox("No behaviours in array!", MessageType.Warning);
            EditorGUILayout.EndHorizontal();            
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Index", GUILayout.MinWidth(50f), GUILayout.MaxWidth(50f));
            EditorGUILayout.LabelField("Behaviours", GUILayout.MinWidth(80f));
            EditorGUILayout.LabelField("Weights", GUILayout.MinWidth(50f), GUILayout.MaxWidth(50f));
            EditorGUILayout.EndHorizontal();

            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < cb.behaviours.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(i.ToString(), GUILayout.MinWidth(50f), GUILayout.MaxWidth(50f));
                cb.behaviours[i] = (Squad_Behaviour)EditorGUILayout.ObjectField(cb.behaviours[i], typeof(Squad_Behaviour), false, GUILayout.MinWidth(80f));
                cb.weights[i] = EditorGUILayout.FloatField(cb.weights[i], GUILayout.MinWidth(50f), GUILayout.MaxWidth(50f));
                EditorGUILayout.EndHorizontal();
            }            
            if (EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(cb);
        }

        EditorGUILayout.Separator();

        if (GUILayout.Button("Add Behaviour"))
        {
            AddBehaviour(cb);
            EditorUtility.SetDirty(cb);
        }

        EditorGUILayout.Separator();

        if (cb.behaviours != null && cb.behaviours.Length > 0)
        {
            if (GUILayout.Button("Remove Behaviour"))
            {
                RemoveBehaviour(cb);
                EditorUtility.SetDirty(cb);
            }
        }
    }

    void AddBehaviour(Composite_Behavour cb)
    {
        int oldCount = (cb.behaviours != null) ? cb.behaviours.Length : 0;
        Squad_Behaviour[] newBehaviours = new Squad_Behaviour[oldCount + 1];
        float[] newWeights = new float[oldCount + 1];

        for (int i = 0; i < oldCount; i++)
        {
            newBehaviours[i] = cb.behaviours[i];
            newWeights[i] = cb.weights[i];
        }
        newWeights[oldCount] = 1f; // don't let new behaviour weight = 0 or it won't appear to be effective
        cb.behaviours = newBehaviours;
        cb.weights = newWeights;
    }

    void RemoveBehaviour(Composite_Behavour cb)
    {
        int oldCount = cb.behaviours.Length;
        if (oldCount == 1)
        {
            cb.behaviours = null;
            cb.weights = null;
            return;
        }

        Squad_Behaviour[] newBehaviours = new Squad_Behaviour[oldCount - 1];
        float[] newWeights = new float[oldCount - 1];

        for (int i = 0; i < oldCount -1; i++)
        {
            newBehaviours[i] = cb.behaviours[i];
            newWeights[i] = cb.weights[i];
        }
        cb.behaviours = newBehaviours;
        cb.weights = newWeights;
    }
}
