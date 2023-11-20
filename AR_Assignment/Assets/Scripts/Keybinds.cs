using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Keybinds : MonoBehaviour
{
    [Header("References")] 
    public GameObject pauseMenuRef;
    public CanvasGroup debugStripRef;
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Pause)) {
            //  if the time is running... (if the game is running)
            if (Time.timeScale == 1) {
                pauseMenuRef.SetActive(true);
                Time.timeScale = 0;

            } else {
                pauseMenuRef.SetActive(false);
                Time.timeScale = 1;

            }
        } 
        else if (Input.GetKeyDown(KeyCode.Insert)) {
            debugStripRef.GetComponent<DebugStripBehaviour>().toggleVisibility();
        }
    }
    
    
    
    
}