using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleCounter : MonoBehaviour
{
    private float count;
    [SerializeField] private Material _shader;
    
    void Awake()
    {
        /*int vertexCount = GetComponent<MeshFilter>().mesh.triangles.Length;
        int tessellationRoundedToCeilUneven;
        int tessellationRounded = Mathf.CeilToInt(tessellation);
        tessellationRoundedToCeilUneven = tessellationRounded % 2 == 0 ? tessellationRounded + 1 : tessellationRounded;
        //print(tessellationRoundedToCeilUneven);
        CalculateVertexCountAfterTessellation(vertexCount, tessellationRoundedToCeilUneven);
        print($"Total Triangles - {count}");*/
    }

    public static int CalculateTrianglesWithTessellation(int triangleCount, float tessellation)
    {
        int tessellationRounded = Mathf.CeilToInt(tessellation);
        int tessellationRoundedToCeilUneven = tessellationRounded % 2 == 0 ? tessellationRounded + 1 : tessellationRounded;
        return CalculateTriangleCountAfterTessellation(triangleCount, tessellationRoundedToCeilUneven);
    }
    
    public static int CalculateTriangleCountAfterTessellation(int triangleCount, int Tessellation, int loopCount = 1, int prevNum = 0, int tessCount = 1)
    {
        int addAmount = (loopCount - 1) * 4;
        print($"Added Amount - {addAmount}");
        prevNum += addAmount;

        if (tessCount == Tessellation) {
            if (Tessellation == 1)
            {
                return triangleCount;
            }
            print($"Total Multiplier - {prevNum}");
            return (prevNum) * triangleCount + 1;
        }	
        
        loopCount++;
        tessCount += 2;
        return CalculateTriangleCountAfterTessellation(triangleCount, Tessellation, loopCount, prevNum, tessCount);
    }
}