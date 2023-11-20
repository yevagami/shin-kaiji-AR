using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class DebugStripBehaviour : MonoBehaviour {

    [Header("References")]
    [Tooltip("a reference to the debug strip TEXT")]
    [SerializeField] private TextMeshProUGUI DebugStripTextRef;
    [Tooltip("a reference to object containing the CANVAS GROUP component")]
    [SerializeField] private CanvasGroup DebugStripCanvasGroup;

   [Header("Variables")] [SerializeField] private bool IsVisible;

    public void whenClickedCallback() {
        toggleVisibility();
    }

    public bool toggleVisibility() {
        IsVisible = !IsVisible;
        
        // set the alpha value to 0 when invisible, 1 when visible
        DebugStripCanvasGroup.alpha = IsVisible ? 1f : 0f;
        
        return IsVisible;
    }
    
    //  set the text of the debug strip text with the provided string, ensuring the string is not null
    //  if the string is null, an exception is thrown
    public void log([NotNull] string printTHIS)
    {
        Debug.Log(printTHIS);
        DebugStripTextRef.text = printTHIS ?? throw new ArgumentNullException(nameof(printTHIS));
    }
    
    void Start()
    {
        DebugStripTextRef.text = "enabled";
        IsVisible = true;
        DebugStripCanvasGroup.alpha = 1f;
        
    }
    
    
    
}
