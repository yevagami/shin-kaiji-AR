using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//card data struct
struct card{
    public GameObject cardObject;
    public int type;
}

public class GameManager : MonoBehaviour{
    // Start is called before the first frame update
    enum cardType { rock = 0, paper = 1, scissors = 2 };
    [SerializeField] GameObject[] cards;
    [SerializeField] GameObject[] playerCardPositions;
    [SerializeField] GameObject[] enemyCardPositions;
    [SerializeField] GameObject canvasUIParent;

    //Array containing the cards of the player and enemy
    card[] playerCards = new card[5];
    card[] enemyCards = new card[5];

    //The score of the player and the enemy
    int playerScore = 0;
    int enemyScore = 0;


    void Start(){
        shuffleCards();
    }

    // Update is called once per frame
    void Update(){
        
    }

    
    public void shuffleCards(){
        //Shuffle player cards
        for(int i = 0; i < playerCardPositions.Length; i++) {
            if(playerCards[i].cardObject != null){
                Destroy(playerCards[i].cardObject);
            }

            int cardIndex = Random.Range(0, 3);
            playerCards[i].cardObject = Instantiate(cards[cardIndex], playerCardPositions[i].transform.position, Quaternion.identity);
            playerCards[i].cardObject.transform.SetParent(canvasUIParent.transform);
            playerCards[i].type = cardIndex;
        }

        //Shuffle the enemy's cards
        for (int i = 0; i < enemyCardPositions.Length; i++)
        {
            if (enemyCards[i].cardObject != null){
                Destroy(playerCards[i].cardObject);
            }

            int cardIndex = Random.Range(0, 3);
            enemyCards[i].cardObject = Instantiate(cards[cardIndex], playerCardPositions[i].transform.position, Quaternion.identity);
            enemyCards[i].type = cardIndex;
        }
    }
    
    //Button methods
    public void playRockCard() {
        Debug.Log("Click on rock");
    }
    public void playScissorsCard() {
        Debug.Log("Click on scissors");
    }
    public void playPaperCard() {
        Debug.Log("Click on paper");
    }
}
