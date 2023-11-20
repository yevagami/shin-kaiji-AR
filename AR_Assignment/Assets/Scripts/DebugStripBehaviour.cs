using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugStripBehaviour : MonoBehaviour
{

    public TextMeshProUGUI DebugStripTextRef;
    //public GameObject DebugStripRef;
    public CanvasGroup DebugStripCanvasGroup;
    
    public bool isVisible;

    public void whenClickedCallback()
    {
        // Toggle the visibility of the DebugStripRef
        isVisible = !isVisible;
        
        // Set the alpha value to 0 when invisible, 1 when visible
        DebugStripCanvasGroup.alpha = isVisible ? 1f : 0f;
    }
    
    public void log(string printTHIS)
    {
        DebugStripTextRef.text = printTHIS;
    }
    
    void Start()
    {
        
        DebugStripTextRef.text = "enabled";
        isVisible = true;
        DebugStripCanvasGroup.alpha = 1f;
        
    }
    
    
    
}
