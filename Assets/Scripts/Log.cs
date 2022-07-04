using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Log : MonoBehaviour
{
    [SerializeField] private string fileName = "Log";
    [SerializeField] private float runTime = 10f;
    [SerializeField] private Material shaderMaterial;
    [SerializeField] private int fpsMargin = 10;

    
    private List<float> FPS_List = null;
    private string filePath = "";

    private float deltaTime = 0;

    public bool isRunningTriangle;
    public bool isRunningTessellation;
    public bool isVR;

    public int RunsBeforeStop = 1;
    
    public bool IncreaseTessellationOnNextRun;
    public int StopAtTessellation;
    public int TessellationIncrement = 2;
    
    private int run;
    private float tessellation;
    private long triCount;
    
    
    
    void Start()
    {
        run = PlayerPrefs.GetInt("Run", 1);
        tessellation = shaderMaterial.GetFloat("_Tess");
        print($"Started Run {run} on Tessellation {tessellation}");
        FPS_List = new List<float>();
        
        CheckFile();
        
        StartCoroutine(Timer(runTime));
    }

    private void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        print(fps);
        FPS_List.Add(fps);
    }

    private void CheckFile()
    {
        int num = 0;
        string fileNameWithPrefix = "";
        if (isRunningTessellation) fileNameWithPrefix = $"Tess_R{run}_";
        if (isRunningTriangle) fileNameWithPrefix = $"Tri_R{run}_";
        fileNameWithPrefix += fileName;
        string date = DateTime.Now.ToString("yyyy-dd-M");
        filePath = $"{Application.dataPath}/Logs/{fileNameWithPrefix}_{date}_{num}.txt";
        while (File.Exists(filePath))
        {
            num++;
            filePath = $"{Application.dataPath}/Logs/{fileNameWithPrefix}_{date}_{num}.txt";
        }
        File.WriteAllText(filePath, "");
    }

    private void WriteValues()
    {
        File.AppendAllText(filePath, $"=== FPS Log ===\n\n");

        int FPSCount = FPS_List.Count;
        float allFPS = 0;
        float allFPSNoMargin = 0;

        // Get a median to filter out unwanted FPS spikes
        float median = FPS_List[FPSCount / 2];
        
        
        foreach (float FPS_Value in FPS_List)
        {
            if (Mathf.Abs(FPS_Value - median) < fpsMargin) allFPS += FPS_Value;
            else FPSCount--;

            allFPSNoMargin += FPS_Value;
        }
        float averageFPS = allFPS / FPSCount;
        float averageFPSNoMargin = allFPSNoMargin / FPS_List.Count;
        triCount = 0;

        File.AppendAllText(filePath, $"Average FPS: {averageFPS}\n");
        File.AppendAllText(filePath, $"Average FPS Without Margin: {averageFPSNoMargin}\n");
        File.AppendAllText(filePath, $"Total FPS Measured: {FPSCount}\n\n");
        if (isVR) File.AppendAllText(filePath, $"Running VR!\n\n");
        if (isRunningTessellation)
        {
            File.AppendAllText(filePath, $"Logged data for Tessellation run!\n");
            File.AppendAllText(filePath, $"Total Tessellation Amount: {tessellation}\n");
            triCount = TriangleCounter.CalculateTrianglesWithTessellation(UnityEditor.UnityStats.triangles 
                                                                          - 2, tessellation);
        }
        if (isRunningTriangle)
        {
            File.AppendAllText(filePath, $"Logged data for Triangle run!\n");
            triCount = UnityEditor.UnityStats.triangles - 2;
        }

        File.AppendAllText(filePath, $"Total Triangle Count: {triCount}\n\n");
        File.AppendAllText(filePath, $"===============\n\n");
        
        File.AppendAllText(filePath, $"All FPS Values:\n");
        foreach (int FPS_Value in FPS_List)
        {
            string fps = Mathf.Abs(FPS_Value - median) < fpsMargin ? $"{FPS_Value}" : $"-{FPS_Value}-";
            File.AppendAllText(filePath, $"{fps}\n"); 
        }
        Debug.Log("File Generated");
        CreateMultiLogFile(averageFPS);
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


    private void OnDestroy()
    {
        if (PlayerPrefs.GetInt("Stop", 0) == 1) return;
        if (run < RunsBeforeStop)
        {
            PlayerPrefs.SetInt("Run", run + 1);

            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = true;
            #endif
        }
        else
        {
            
            if (IncreaseTessellationOnNextRun && tessellation < StopAtTessellation)
            {
                tessellation = Mathf.Min(tessellation + TessellationIncrement, 69);
                
                shaderMaterial.SetFloat("_Tess", tessellation);
                
                if (isRunningTriangle)
                {
                    MeshBuilder.CreatePlaneWithTessellation(tessellation);
                }
                
                PlayerPrefs.SetInt("Run", 1);
                //StartNewRun();
                
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = true;
#endif
            }
        }
    }

    public void StartTest()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = true;
        #endif
        PlayerPrefs.SetInt("Run", 1);
        PlayerPrefs.SetInt("Stop", 0);
        
        if (isRunningTriangle)
        {
            MeshBuilder.CreatePlaneWithTessellation(shaderMaterial.GetFloat("_Tess"));
        }
    }

    public void StopTest()
    {
        PlayerPrefs.SetInt("Run", RunsBeforeStop);
        PlayerPrefs.SetInt("Stop", 1);
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    private void StartNewRun()
    {
        string path = $"{Application.dataPath}/Logs/MultiLog.txt";
        string mode = "";
        if (isRunningTessellation) mode = "Tessellation";
        if (isRunningTriangle) mode = "Triangle";
        DateTime date = DateTime.Now;
        if (isVR)  File.AppendAllText(path, $"\n=== VR - FPS Log - {mode} - Tri: {triCount} - Tess: {tessellation} - {date.Day}/{date.Month} {date.Hour}:{date.Minute}:{date.Second} ===\n");
        else File.AppendAllText(path, $"\n=== FPS Log - {mode} - Tri: {triCount} - Tess: {tessellation} - {date.Day}/{date.Month} {date.Hour}:{date.Minute}:{date.Second} ===\n");
    }

    private void CreateMultiLogFile(float averageFPS)
    {
        if (run == 1) StartNewRun();
        string path = $"{Application.dataPath}/Logs/MultiLog.txt";
        //File.AppendAllText(filePath, $"=== FPS Log - {mode} - Tri: {triCount} - Tess: {tess} - {date.Day}/{date.Month} {date.Hour}:{date.Minute}:{date.Second} ===\n");
        File.AppendAllText(path, $"Run {run}: Average FPS: {averageFPS}\n");
    }
}
