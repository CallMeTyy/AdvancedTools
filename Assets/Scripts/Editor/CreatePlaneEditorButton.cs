using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MeshBuilder))]
public class CreatePlaneEditorButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MeshBuilder myScript = (MeshBuilder)target;
        if (GUILayout.Button("Generate Mesh"))
        {
            myScript.CreatePlaneWithTriangleCount();
        }
        
        
        if (GUILayout.Button("Generate Mesh From Tessellation"))
        {
            myScript.CreatePlaneWithTessellationAmount();
        }
    }

}
