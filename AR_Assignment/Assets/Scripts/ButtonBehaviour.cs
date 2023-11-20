using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    public TextMeshProUGUI selectedEnemyRef;


    private bool isMainMenuOpen;
    public GameObject mainMenuCanvasReference;
    public bool isDebugStripActive;
    
    #region abstract THIS
    //  string containing the default enemy name
    public string defaultEnemy;
    //  temp 'hard code' (its not a real reference)
    public bool isEnemyScanned;
    #endregion

    public void PlayButtonCallback()
    {
        //  run the game loop
    }

    public void ScanEnemyButtonCallback()
    {
        //  call the function to allow the player to start tracking the camera in order to figure out what they're aiming at
        isEnemyScanned = true;
    }

    public void LeaveButtonCallback()
    {
        if (Application.isEditor)
        {
            Debug.Log("quitting the editor");
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Debug.Log("this is never seen");
            Application.Quit();
        }
    }

    void Start()
    {
        isMainMenuOpen = true;
        isEnemyScanned = false;
        selectedEnemyRef.text = defaultEnemy;
    }

    void Update()
    {
        //  enable/disable the main menu
        mainMenuCanvasReference.SetActive(isMainMenuOpen);


        selectedEnemyRef.text = isEnemyScanned ? "*scanned enemy name*" : defaultEnemy;
        selectedEnemyRef.color = isEnemyScanned ? Color.yellow : Color.magenta;
        
    }
}