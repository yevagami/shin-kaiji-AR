using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clickCard : MonoBehaviour{
    [SerializeField] Button butt;
    GameManager gameManager;
    public card cardInfo;

    private void Awake(){
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        butt.onClick.AddListener(() => gameManager.playCard(cardInfo, gameObject));
    }
}