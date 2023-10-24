using UnityEngine;
using UnityEngine.UI;

public class DebugDisplay : MonoBehaviour
{
    public bool showDebug = false;
    
    public Canvas debugCanvas;
    public Text fpsText;
    
    private float deltaTime;
    void Update ()
    {
        if (!showDebug)
        {
            debugCanvas.enabled = false;
            return;
        }
        debugCanvas.enabled = true;
        ShowFPS();
    }

    public void ShowFPS()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        var fps = 1.0f / deltaTime;
        fpsText.text = $"FPS : {Mathf.Ceil(fps)}";
    }
}
