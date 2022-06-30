using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MeshBuilder : MonoBehaviour
{
    public int TriangleCount;

    // Start is called before the first frame update
    void Start()
    {
        CreatePlaneWithTriangleCount(TriangleCount);
    }
    

    public void CreatePlaneWithTriangleCount(int Count = -1)
    {
        if (Count == -1) Count = TriangleCount;
        int trianglesCreated = 0;
        int wh = Mathf.CeilToInt(Mathf.Sqrt(Count/2));
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uv = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();
        List<int> triangles = new List<int>();
        //Create Vertices
        for (float z = 0; z <= wh; z++)
        {
            for (float x = 0; x <= wh; x++)
            {
                vertices.Add(new Vector3(x/wh-0.5f, 0, z/wh-0.5f));
                uv.Add(new Vector2(x / wh, z / wh));
                normals.Add(Vector3.up);
            }
        }

        for (int i = 0; i < vertices.Count; i++)
        {
            if ((i+1) % (wh+1) != 0 && trianglesCreated < Count && i < (wh*(wh+1)-1))
            {
                int row = wh + 1;
                trianglesCreated++;
                triangles.Add(i+1);
                triangles.Add(i);
                triangles.Add(i+row);
                if (trianglesCreated < Count)
                {
                    trianglesCreated++;
                    triangles.Add(i+1+row);
                    triangles.Add(i+1);
                    triangles.Add(i+row); 
                }
            }
        }
        
        Mesh mesh = new Mesh();
        //GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = vertices.ToArray();
        mesh.uv = uv.ToArray();
        mesh.triangles = triangles.ToArray();
        print(triangles.Count);
        mesh.normals = normals.ToArray();
        
        AssetDatabase.CreateAsset(mesh, $"Assets/Meshes/CustomPlaneWithSetTriangles.asset");
        AssetDatabase.SaveAssets();
        print("Saved Mesh");

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Triangle"))
        {
            g.GetComponent<MeshFilter>().mesh = mesh;
        }
    }

    private void CreateSmallPlane()
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

        int[] newTriangles = new int[] {2, 1, 0};
        
        //GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newTriangles;
        mesh.normals = newNormals;
        
        AssetDatabase.CreateAsset(mesh, "Assets/Meshes/LowVertexTriangle.asset");
        AssetDatabase.SaveAssets();
    }
}
