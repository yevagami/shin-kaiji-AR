using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//card data struct
enum condition{ win = 1, lose = -1, draw = 0 }
enum cardType { rock = 0, paper = 1, scissors = 2 };
enum states { start, idle, play, compare, end, destroy}
public struct card{
    public GameObject cardObject;
    public int type;
    public int cardPos;
}

public class GameManager : MonoBehaviour {
    // Start is called before the first frame update
    [Header("Card objects")]
    [SerializeField] GameObject[] cards; //card prefabs

    [Header("Card positions")]
    [SerializeField] Transform[] playerCardPositions; //Where the player's cards are gonna be 
    [SerializeField] Transform[] enemyCardPositions; //Where the enemy's cards are gonna be 
    [SerializeField] Transform playerPlayPosition; //Where the player's selected cards are gonna be
    [SerializeField] Transform enemyPlayPosition; //Where the enemy's selected cards are gonna be 

    [Header("Canvas stuff")]
    [SerializeField] GameObject canvasUIParent; //Reference to the UI canvas 
    [SerializeReference] TextMeshProUGUI leftText;
    [SerializeReference] TextMeshProUGUI rightText;
    [SerializeReference] TextMeshProUGUI middleText;

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

    //Debuging stuff
    int count = 0;

    void Start() {
    }


    // Update is called once per frame
    void Update() {
        switch(currentState) { 
            case states.start:
                shuffleCards();
                currentState = states.idle;
                break;

            case states.idle:
                break;

            case states.play:
                if(playerSelected.cardObject != null){
                    if (enemyPlayCard()) {
                        renderPlayCards();
                        currentState = states.compare;
                    }
                }
                break;

            case states.compare:
                compareCardsState();
                break;

            case states.destroy:
                StartCoroutine(destroyCards());
                break;
        }

        //Update ui stuff
        leftText.text = "Player " + playerScore;
        rightText.text = enemyScore + " Enemy";
        //Debug.Log(currentState);
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

            //for debug purposes
            //prints the card now renders on the screen
            enemyCards[i].cardObject.transform.SetParent(canvasUIParent.transform);
            enemyCards[i].cardObject.transform.localScale = enemyCardPositions[i].transform.localScale;
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
    public void playCard(card card_){
        //Selecting cards are only allowed on the play state
        if (currentState != states.idle){
            return;
        }

        count++;
       
        playerSelected = card_;

        currentState = states.play;
    }

    //Play the enemy card
    public bool enemyPlayCard(){
        //Moves the enemy's card to the designated position
        int selectedCard = Random.Range(0, enemyCards.Length); //Randomly pick which card for the enemy to play

        //Picks another random card if that specific card has been played
        if(!enemyCards[selectedCard].cardObject){ 
            return false; 
        }

        if (count == 2)
        {
            Debug.Log("");
        }

        enemySelected = enemyCards[selectedCard];
        return true;
    }

    public void renderPlayCards(){
        playerSelected.cardObject.transform.position = playerPlayPosition.transform.position;
        playerSelected.cardObject.transform.localScale = playerPlayPosition.transform.localScale;

        enemySelected.cardObject.transform.position = enemyPlayPosition.transform.position;
        enemySelected.cardObject.transform.localScale = enemyPlayPosition.transform.localScale;
        enemySelected.cardObject.transform.SetParent(canvasUIParent.transform);
    }

    void compareCardsState(){ 
        condition result = compareCards(playerSelected, enemySelected);
        Debug.Log(result);
        switch (result)
        {
            case condition.lose:
                enemyScore += 1;
                break;

            case condition.win:
                playerScore += 1;
                break;

            case condition.draw:
                break;
        }

        currentState = states.idle;
    }

    IEnumerator destroyCards() {
        yield return new WaitForSeconds(1);

        Destroy(playerCards[playerSelected.cardPos].cardObject);
        Destroy(enemyCards[enemySelected.cardPos].cardObject);

        Destroy(playerSelected.cardObject);
        Destroy(enemySelected.cardObject);
    }
}
