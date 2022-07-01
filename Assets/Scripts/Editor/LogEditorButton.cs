using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Log))]
public class LogEditorButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Log myScript = (Log)target;
        if (GUILayout.Button("Start Test"))
        {
            myScript.StartTest();
        }
        
        if (GUILayout.Button("Stop Test"))
        {
            myScript.StopTest();
        }
    }

}
