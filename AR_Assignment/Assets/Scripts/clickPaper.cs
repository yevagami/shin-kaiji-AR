using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clickPaper : MonoBehaviour{

    [SerializeField] Button butt;
    GameManager gameManager;
    private void Awake(){
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        butt.onClick.AddListener(() => gameManager.playPaperCard());
    }
}
