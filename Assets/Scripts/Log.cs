using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Log : MonoBehaviour
{
    [SerializeField] private string fileName = "FPS_Log";
    [SerializeField] private float runTime = 10f;
    [SerializeField] private Material shaderMaterial;

    
    private List<float> FPS_List = null;
    private string filePath = "";

    private float deltaTime = 0;
    
    
    
    void Start()
    {
        FPS_List = new List<float>();
        
        CheckFile();
        
        StartCoroutine(Timer(runTime));
    }

    private void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        FPS_List.Add(fps);
    }

    private void CheckFile()
    {
        int num = 0;
        string date = DateTime.Now.ToString("yyyy-dd-M");
        filePath = $"{Application.dataPath}/Logs/{fileName}_{date}_{num}.txt";
        while (File.Exists(filePath))
        {
            num++;
            filePath = $"{Application.dataPath}/Logs/{fileName}_{date}_{num}.txt";
        }
        File.WriteAllText(filePath, "");
    }

    private void WriteValues()
    {
        File.AppendAllText(filePath, $"=== FPS Log ===\n\n");

        int FPSCount = FPS_List.Count;
        float allFPS = 0;

        foreach (float FPS_Value in FPS_List)
            allFPS += FPS_Value;

        float averageFPS = allFPS / FPSCount;

        File.AppendAllText(filePath, $"Average FPS: {averageFPS}\n");
        File.AppendAllText(filePath, $"Total FPS Measured: {FPSCount}\n\n");
        float tessellation = shaderMaterial.GetFloat("_Tess");
        File.AppendAllText(filePath, $"Total Tessellation Amount: {tessellation}\n");
        File.AppendAllText(filePath, $"Total Triangle Count: {TriangleCounter.CalculateTrianglesWithTessellation(UnityEditor.UnityStats.triangles-2, tessellation)}\n\n");
        File.AppendAllText(filePath, $"===============\n\n");
        
        File.AppendAllText(filePath, $"All FPS Values:\n");
        foreach(int FPS_Value in FPS_List)
            File.AppendAllText(filePath, $"{FPS_Value}\n");
        
        Debug.Log("File Generated");
    }

    private IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        WriteValues();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
