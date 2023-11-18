using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//card data struct
enum condition{ win = 1, lose = -1, draw = 0 }
enum cardType { rock = 0, paper = 1, scissors = 2 };
enum states { start, play, compare, end}
public struct card{
    public GameObject cardObject;
    public int type;
    public int cardPos;
}

public class GameManager : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] GameObject[] cards; //card prefabs
    [SerializeField] Transform[] playerCardPositions; //Where the player's cards are gonna be 
    [SerializeField] Transform[] enemyCardPositions; //Where the enemy's cards are gonna be 
    [SerializeField] Transform playerPlayPosition; //Where the player's selected cards are gonna be
    [SerializeField] Transform enemyPlayPosition; //Where the enemy's selected cards are gonna be 
    [SerializeField] GameObject canvasUIParent; //Reference to the UI canvas 

    //Array containing the cards of the player and enemy
    card[] playerCards = new card[5];
    card[] enemyCards = new card[5];

    //Temp variable holding the cards that were selected
    card playerSelected;
    card enemySelected;

    //The score of the player and the enemy
    int playerScore = 0;
    int enemyScore = 0;

    //State machine
    states currentState = states.start;

    void Start() {
    }


    // Update is called once per frame
    void Update() {
        switch(currentState) { 
            case states.start:
                shuffleCards();
                currentState = states.play;
                break;

            case states.play:
                break;


            case states.compare:
                Debug.Log(compareCards(playerSelected, enemySelected));
                break;
        }
    }


    public void shuffleCards() {
        //Shuffle player cards
        for (int i = 0; i < playerCardPositions.Length; i++) {
            if (playerCards[i].cardObject != null) {
                Destroy(playerCards[i].cardObject);
            }

            //Creating the card
            int cardIndex = Random.Range(0, 3);
            playerCards[i].cardObject = Instantiate(cards[cardIndex], playerCardPositions[i].position, Quaternion.identity);
            playerCards[i].cardObject.transform.SetParent(canvasUIParent.transform);
            playerCards[i].type = cardIndex;
            playerCards[i].cardPos = i;

            //Setting the card info
            playerCards[i].cardObject.GetComponent<clickCard>().cardInfo = playerCards[i];
        }

        
        //Shuffle the enemy's cards
        for (int i = 0; i < enemyCardPositions.Length; i++)
        {
            if (enemyCards[i].cardObject != null){
                Destroy(enemyCards[i].cardObject);
            }

            int cardIndex = Random.Range(0, 3);
            enemyCards[i].cardObject = Instantiate(cards[cardIndex], enemyCardPositions[i].position, Quaternion.identity);
            enemyCards[i].type = cardIndex;
            enemyCards[i].cardPos = i;

            //note, the enemy's card is not set to the canvas ui
            //this is so that it doesn't render after shuffling, cause that will be cheating
        }
        
    }

    condition compareCards(card card1, card card2){
        //Guard clause to check if it's a draw 
        //Exits early
        if(card1.type == card2.type){ return condition.draw; }

        //rock vs scissors
        if(card1.type == (int)cardType.rock && card2.type == (int)cardType.scissors) { return condition.win; }

        //scissors vs paper
        if (card1.type == (int)cardType.scissors && card2.type == (int)cardType.paper){ return condition.win; }

        //paper vs rock
        if (card1.type == (int)cardType.paper && card2.type == (int)cardType.rock) { return condition.win; }

        //otherwise you lose bozo
        return condition.lose;
    }

    //Button methods
    public void playCard(card card_, GameObject object_){
        //Selecting cards are only allowed on the play state
        if(currentState != states.play){
            Debug.Log("Not the time!");
            return;
        }

        //Moves the player's card to the designated position
        object_.transform.position = playerPlayPosition.position;
        object_.transform.localScale = playerPlayPosition.localScale;
        playerSelected = card_;

        //Moves the enemy's card to the designated position
        int selectedCard = Random.Range(0, 5); //Randomly pick which card for the enemy to play

        //Set the position of the card to be on its designated position
        enemyCards[selectedCard].cardObject.transform.position = enemyPlayPosition.position;
        enemyCards[selectedCard].cardObject.transform.localScale = enemyPlayPosition.localScale;

        //Set the parent of the card to be the canvas ui
        //otherwise it will not render
        enemyCards[selectedCard].cardObject.transform.SetParent(canvasUIParent.transform);
        enemySelected = enemyCards[selectedCard];
        currentState = states.compare;
    }
}
