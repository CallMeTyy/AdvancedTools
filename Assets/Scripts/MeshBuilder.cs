using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MeshBuilder : MonoBehaviour
{
    public int TriangleCount;
    public bool debug;
    public bool isTerrainMesh;

    [SerializeField] private Material _mat;

    // Start is called before the first frame update
    void Start()
    {
        //CreatePlaneWithTriangleCount(TriangleCount);
    }

    public void CalculateTriCount()
    {
        float tessellation = _mat.GetFloat("_Tess");
        int tessellationRounded = Mathf.CeilToInt(tessellation);
        int tessellationRoundedToCeilUneven = tessellationRounded % 2 == 0 ? tessellationRounded + 1 : tessellationRounded;
        long triCount = TriangleCounter.CalculateTrianglesWithTessellation(20000, tessellation);
        print($"Tessellation {tessellation} has {triCount} triangles");
    }

    public void CreatePlaneWithTessellationAmount(float tessellation = -1)
    {
        long triCount;
        int fromTriangles = isTerrainMesh ? 5000 : 2;
        if (tessellation == -1)
        {
            tessellation = _mat.GetFloat("_Tess");
            triCount = TriangleCounter.CalculateTrianglesWithTessellation(fromTriangles,
                tessellation);
        }
        else
        {
            triCount = TriangleCounter.CalculateTrianglesWithTessellation(fromTriangles,
                tessellation);
        }
        print(tessellation);
        print(triCount);
        CreatePlaneWithTriangleCount(triCount);
    }
    public void CreatePlaneWithTriangleCount(long Count = -1)
    {
        // The width & height can be taken by taking the count,
        // dividing it by two (as two triangles create a small plane)
        // Taking the square root, and rounding it off to the top.
        if (Count == -1) Count = TriangleCount;
        int trianglesCreated = 0;
        int wh = Mathf.CeilToInt(Mathf.Sqrt(Count/2f));
        print(wh);
        
        //Create all lists
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uv = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();
        List<int> triangles = new List<int>();
        
        //Create Vertices, uvs & normals
        for (float z = 0; z <= wh; z++)
        {
            for (float x = 0; x <= wh; x++)
            {
                vertices.Add(new Vector3(x/wh-0.5f, 0, z/wh-0.5f));
                uv.Add(new Vector2(x / wh, z / wh));
                normals.Add(Vector3.up);
            }
        }

        // Add the actual triangles & stop if we reached the triangle count
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
        
        // Create the mesh & add the date
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.uv = uv.ToArray();
        mesh.triangles = triangles.ToArray();
        if (debug) print(triangles.Count);
        mesh.normals = normals.ToArray();
        
        //Save the mesh
        AssetDatabase.CreateAsset(mesh, $"Assets/Meshes/CustomPlaneWithSetTriangles.asset");
        AssetDatabase.SaveAssets();
        print("Saved Mesh");

        //Apply the mesh to all triangle objects in the scene
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
    
    public static void CreatePlaneWithTessellation(float tessellation)
    {
        long Count = TriangleCounter.CalculateTrianglesWithTessellation(2,
            tessellation);
        
        int trianglesCreated = 0;
        int wh = Mathf.CeilToInt(Mathf.Sqrt(Count/2f));
        print(wh);
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

        mesh.normals = normals.ToArray();
        
        AssetDatabase.CreateAsset(mesh, $"Assets/Meshes/CustomPlaneWithSetTriangles.asset");
        AssetDatabase.SaveAssets();
        print("Saved Mesh");

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Triangle"))
        {
            g.GetComponent<MeshFilter>().mesh = mesh;
        }
    }
}
