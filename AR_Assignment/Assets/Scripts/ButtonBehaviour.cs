using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour {
    [Header("References")]
    [Tooltip("a reference to the selected enemy TEXT")]
    public TextMeshProUGUI selectedEnemyRef;
    [Tooltip("a reference to the main menu CANVAS")]
    public GameObject mainMenuCanvasRef;
    [Tooltip("a reference to the pause menu CANVAS")]
    public GameObject pauseMenuCanvasRef;
    [Tooltip("a reference to the object containing GAME")]
    public GameObject gameRef;
    [Header("Debug Reference")]
    [Tooltip("a reference to the debug strip object")]
    public DebugStripBehaviour debugStrip; // debug.log("") but on the top of the screen
    
    [Header("Variables")]
    [Tooltip("is the main menu shown? is it active?")]
    [SerializeField] bool isMainMenuOpen;
    [Tooltip("is the pause menu shown? is it active?")]
    [SerializeField] bool isPauseMenuOpen;
    
    #region abstract THIS
    //  temp 'hard code' (its not a real reference)
    public bool isEnemyScanned;
    //  string containing the default enemy name
    public string defaultEnemy;
    //  string containing the scanned enemy name
    public string scannedEnemy;
    #endregion

    public void PlayButtonCallback() {
        if (gameRef) {
            debugStrip.log("starting game");
            //  run the game loop
            
        } else {
            debugStrip.log("no game reference");
            isMainMenuOpen = !isMainMenuOpen;
        }
     
        
        
    }

    public void ScanEnemyButtonCallback()
    {
        debugStrip.log("scan enemy button clicked");
        //  call the function to allow the player to start tracking the camera in order to figure out what they're aiming at
        
        
        //  stuff
        
        
        isEnemyScanned = true;
    }

    public void LeaveButtonCallback()
    {
        if (Application.isEditor) {
            debugStrip.log("quitting the editor");
            Debug.Log("quitting the editor");
            UnityEditor.EditorApplication.isPlaying = false;
        } else {
            debugStrip.log("quitting game");
            Debug.Log("this is never seen");
            Application.Quit();
        }
    }

    public void ResumeGameButtonCallback() {
        Time.timeScale = 1;
    }

    void Start() {
        isMainMenuOpen = true;
        isPauseMenuOpen = false;
        isEnemyScanned = false;
        selectedEnemyRef.text = defaultEnemy;
    }

    void Update()
    {
        //  enable/disable the main menu
        mainMenuCanvasRef.SetActive(isMainMenuOpen);
        pauseMenuCanvasRef.SetActive(isPauseMenuOpen);

        scannedEnemy = "Freddy Fazbear";
        selectedEnemyRef.text = isEnemyScanned ? scannedEnemy : defaultEnemy;
        selectedEnemyRef.color = isEnemyScanned ? Color.yellow : Color.magenta;
        
    }
}