using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Profiling;
using System.Text;

public class DebugInterface : MonoBehaviour
{
    public TextMeshProUGUI FPSText;
    public TextMeshProUGUI UsedMemory;

    public Canvas DebugCanvas;
    public bool isDisplayed = false;

    private float pollingTime = 1f;
    private float time;
    private int frameCount;

    ProfilerRecorder _TotalUsedMemoryRecorder;

    private void Start()
    {
        DebugCanvas.enabled = false;
    }

    private void OnEnable()
    {
        _TotalUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Used Memory");
    }

    private void OnDisable()
    {
        _TotalUsedMemoryRecorder.Dispose();
    }

    void Update()
    {
        time += Time.deltaTime;

        frameCount++;

        if(time >= pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            FPSText.text = "FPS: " + frameRate.ToString();

            UsedMemory.text = "Memory Used: " + _TotalUsedMemoryRecorder.LastValue.ToString();

            time -= pollingTime;
            frameCount = 0;
        }

        CheckShouldDisplay();
    }

    public void CheckShouldDisplay()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isDisplayed)
            {
                DebugCanvas.enabled = true;
                isDisplayed = true;
            }
            else
            {
                DebugCanvas.enabled = false;
                isDisplayed = false;
            }
        }
    }
}