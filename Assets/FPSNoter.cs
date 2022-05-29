using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FPSNoter : MonoBehaviour
{
    [SerializeField] private string fileTitle = "fps_log";
    [SerializeField] private float timeBeforeQuit = 10f;

    [HideInInspector] public List<float> currentFPS_Time = null;

    private string path = "";

    private float deltaTime = 0;
    
    void Start()
    {
        CheckFile();
        StartCoroutine(Timer(timeBeforeQuit));
    }

    private void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        currentFPS_Time.Add(fps);
    }

    private void CheckFile()
    {
        path = $"{Application.dataPath}/{fileTitle}.txt";
        
        if (File.Exists(path))
            File.WriteAllText(path, "");
        else if (!File.Exists(path))
            File.WriteAllText(path, "");
    }

    private void WriteValues()
    {
        File.AppendAllText(path, $"{fileTitle}\n\n");

        int totalFPS = 0;
        int averageFPS = 0;

        foreach (int FPS_Value in currentFPS_Time)
            totalFPS += FPS_Value;

        averageFPS = totalFPS / currentFPS_Time.Count;
        
        File.AppendAllText(path, $"Average FPS: {averageFPS}\n");
        File.AppendAllText(path, $"Total FPS Measured: {currentFPS_Time.Count}\n\n");
        
        File.AppendAllText(path, $"All FPS Values:\n");
        foreach(int FPS_Value in currentFPS_Time)
            File.AppendAllText(path, $"{FPS_Value}\n");
        
        Debug.Log("FPS values are written in the document!");
    }

    private IEnumerator Timer(float pTimeBeforeQuit)
    {
        yield return new WaitForSeconds(pTimeBeforeQuit);
        WriteValues();
        Application.Quit();
    }
}
