using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//card data struct
enum condition{ win = 1, lose = -1, draw = 0 }
enum cardType { rock = 0, paper = 1, scissors = 2 };
public struct card{
    public GameObject cardObject;
    public int type;
    public int cardPos;
}

public class GameManager : MonoBehaviour {
    // Start is called before the first frame update
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


    void Start() {
        shuffleCards();
    }


    // Update is called once per frame
    void Update() {

    }


    public void shuffleCards() {
        //Shuffle player cards
        for (int i = 0; i < playerCardPositions.Length; i++) {
            if (playerCards[i].cardObject != null) {
                Destroy(playerCards[i].cardObject);
            }

            //Creating the card
            int cardIndex = Random.Range(0, 3);
            playerCards[i].cardObject = Instantiate(cards[cardIndex], playerCardPositions[i].transform.position, Quaternion.identity);
            playerCards[i].cardObject.transform.SetParent(canvasUIParent.transform);
            playerCards[i].type = cardIndex;
            playerCards[i].cardPos = i;

            //Setting the card info
            playerCards[i].cardObject.GetComponent<clickCard>().cardInfo = playerCards[i];
        }

        /*
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
        */
    }

    condition compareCards(card card1, card card2){
        //Guard clause to check if it's a draw 
        //Exits early
        if(card1.type == card2.type){
            return condition.draw;
        }

        //rock vs scissors
        if(card1.type == (int)cardType.rock && card2.type == (int)cardType.scissors)
        {
            return condition.win;
        }

        //scissors vs paper
        if (card1.type == (int)cardType.scissors && card2.type == (int)cardType.paper)
        {
            return condition.win;
        }

        //paper vs rock
        if (card1.type == (int)cardType.paper && card2.type == (int)cardType.rock)
        {
            return condition.win;
        }

        //otherwise you lose bozo
        return condition.lose;
    }

    //Button methods
    public void playCard(card card_, GameObject object_){
        Destroy(playerCards[card_.cardPos].cardObject);
    }
}
