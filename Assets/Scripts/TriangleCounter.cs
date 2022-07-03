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

    public static long CalculateTrianglesWithTessellation(int triangleCount, float tessellation)
    {
        int tessellationRounded = Mathf.CeilToInt(tessellation);
        int tessellationRoundedToCeilUneven = tessellationRounded % 2 == 0 ? tessellationRounded + 1 : tessellationRounded;
        return CalculateTriangleCountAfterTessellation(triangleCount, tessellationRoundedToCeilUneven);
    }
    
    public static long CalculateTriangleCountAfterTessellation(int triangleCount, int Tessellation, int loopCount = 1, int prevNum = 0, int tessCount = 1)
    {
        // First add the loop amount (times this function has run so far) times 4 to the total amount.
        int addAmount = (loopCount - 1) * 4;
        
        // Next add this amount to the total, this will be the final multiplier
        prevNum += addAmount;

        
        // If we did all calculations, meaning we finished adding 2 to the tessellation amount until we reached this tessellation
        if (tessCount == Tessellation) {
            if (Tessellation == 1)
            {
                // If we just have tessellation 1, we might as well just return triangleCount because prevNum would be 0.
                return triangleCount;
            }
            // Prevnum was found by dividing by 3, so re-add that. Then multiply it with the initial triangle count and add the center triangles back.
            return (prevNum * 3) * triangleCount + triangleCount;
        }	
        
        loopCount++; // Add 1 to the loopCounter
        
        // Increase the tessellation amount, as it first starts with the lowest and recurses until the tessCount
        // has reached the desired tessellation amount.
        tessCount += 2; 
        
        // Repeat the method
        return CalculateTriangleCountAfterTessellation(triangleCount, Tessellation, loopCount, prevNum, tessCount);
    }
}
