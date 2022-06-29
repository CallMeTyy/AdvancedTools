using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MeshBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = new Mesh();

        Vector3[] newVertices = new Vector3[4];
        newVertices[0] = new Vector3(-0.5f, 0,-0.5f);
        newVertices[1] = new Vector3(0.5f, 0,-0.5f);
        newVertices[2] = new Vector3(-0.5f, 0,0.5f);
        newVertices[3] = new Vector3(0.5f, 0,0.5f);
        
        Vector2[] newUV = new Vector2[4];
        newUV[0] = new Vector3(0, 0);
        newUV[1] = new Vector3(1, 0);
        newUV[2] = new Vector3(0, 1);
        newUV[3] = new Vector3(1, 1);

        Vector3[] newNormals = new Vector3[4];
        newNormals[0] = Vector3.up;
        newNormals[1] = Vector3.up;
        newNormals[2] = Vector3.up;
        newNormals[3] = Vector3.up;

        int[] newTriangles = new int[] {2, 1, 0, 1,2,3};
        
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newTriangles;
        mesh.normals = newNormals;
        
        AssetDatabase.CreateAsset(mesh, "Assets/LowVertexPlane.asset");
        AssetDatabase.SaveAssets();
    }
}
