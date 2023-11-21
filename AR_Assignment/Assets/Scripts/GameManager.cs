using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class GameManager : MonoBehaviour {
    //card data struct
    enum condition { win = 1, lose = -1, draw = 0 }
    enum cardType { rock = 0, paper = 1, scissors = 2 };
    enum states { start, idle, play, compare, end, destroy }

    //Card class 
    public class card{
        public GameObject Object;
        public int type;
        public int cardPos;

        //Constructor
        public card(GameObject object_, int type_, int cardPos_){
            type = type_;
            cardPos = cardPos_;
            Object = object_;
        }

        //Function to destroy object
        public void OnDestroy(){
            if (Object != null) { Destroy(Object); }
        }
    }

    // Start is called before the first frame update
    [Header("Card objects")]
    [SerializeField] GameObject[] cards; //card prefabs

    [Header("Player Cards")]
    [SerializeField] Transform[] playerCardPositions; //Where the player's cards are gonna be 
    [SerializeField] Transform playerPlayPosition; //Where the player's selected cards are gonna be

    [Header("Enemy Cards")]
    [SerializeField] Transform[] enemyCardPositions; //Where the enemy's cards are gonna be 
    [SerializeField] Transform enemyPlayPosition; //Where the enemy's selected cards are gonna be 
    [SerializeField] GameObject[] enemyCardsInScene;

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

    //Timer before the cards are destroy
    float timer = 0;
    float timerDuration = 1;

    //State machine
    states currentState = states.start;

    //Debuging stuff
    int count = 0;

    //For the sake of rendering the cards irl
    int enemyCardCount = 0;

    // Update is called once per frame
    void Update() {
        switch(currentState) { 
            case states.start:
                ShuffleCards();
                RenderEnemyCards();
                currentState = states.idle;
                break;

            case states.idle:
                if (CheckEndGame()){
                    currentState = states.end;
                }
                break;

            case states.play:
                if(playerSelected.Object != null){
                    if (enemyPlayCard()) {
                        renderPlayCards();
                        RenderEnemyCards();
                        currentState = states.compare;
                    }
                }
                break;

            case states.compare:
                CompareCardsState();
                break;

            case states.destroy:
                if(timer < timerDuration){
                    timer += Time.deltaTime;
                }
                else{
                    DestroyCards();
                    timer = 0;
                }
                
                break;
                
            case states.end:
                EndGame();
                break;
        }

        //Debug.Log(currentState);
    }



    //Start state
    public void ShuffleCards()
    {
        //Shuffle player cards
        for (int i = 0; i < playerCardPositions.Length; i++)
        {
            if (playerCards[i] != null)
            {
                Destroy(playerCards[i].Object);
            }

            //Creating the card
            int cardIndex = Random.Range(0, 3);
            playerCards[i] = new card(Instantiate(cards[cardIndex], playerCardPositions[i].position, Quaternion.identity, canvasUIParent.transform), cardIndex, i);
            playerCards[i].Object.GetComponent<clickCard>().cardPos = i;

        }


        //Shuffle the enemy's cards
        enemyCardCount = 0;
        for (int i = 0; i < enemyCardPositions.Length; i++)
        {
            if (enemyCards[i] != null)
            {
                Destroy(enemyCards[i].Object);
            }

            int cardIndex = Random.Range(0, 3);
            enemyCards[i] = new card(Instantiate(cards[cardIndex], enemyCardPositions[i].position, Quaternion.identity), cardIndex, i);
            enemyCards[i].Object.transform.localScale = enemyCardPositions[i].transform.localScale;
            enemyCards[i].Object.GetComponent<clickCard>().cardPos = i;

            //note, the enemy's card is not set to the canvas ui
            //this is so that it doesn't render after shuffling, cause that will be cheating

            //Adds 1 to the card count
            enemyCardCount += 1;
        }

    }

    //Play state
    public void PlayCard(int cardPos){
            //Selecting cards are only allowed on the play state
            if (currentState != states.idle){
                return;
            }
       
            playerSelected = playerCards[cardPos];
            currentState = states.play;
    }
    bool enemyPlayCard()
    {
        int selectedCard = Random.Range(0, enemyCards.Length); //Randomly pick which card for the enemy to play

        //Picks another random card if that specific card has been played
        if (enemyCards[selectedCard] == null)
        {
            return false;
        }

        enemyCardCount -= 1;
        enemySelected = enemyCards[selectedCard];
        return true;
    }
    void renderPlayCards()
    {
        playerSelected.Object.transform.position = playerPlayPosition.transform.position;
        playerSelected.Object.transform.localScale = playerPlayPosition.transform.localScale;

        enemySelected.Object.transform.position = enemyPlayPosition.transform.position;
        enemySelected.Object.transform.localScale = enemyPlayPosition.transform.localScale;
        enemySelected.Object.transform.SetParent(canvasUIParent.transform);
    }

    //Compare state
    void CompareCardsState()
    {
        condition result = CompareCards(playerSelected, enemySelected);
        if(result == condition.lose){
            enemyScore += 1;
        }

        if(result == condition.win)
        {
            playerScore += 1;
        }
        
        Debug.Log(
            "Playerscore: " + playerScore + " " + "Enemyscore: " + enemyScore
            );

        //Update ui 
        middleText.text = "VS";
        leftText.text = "Player " + playerScore.ToString();
        rightText.text = enemyScore.ToString() + " Enemy";

        currentState = states.destroy;
    }
    condition CompareCards(card card1, card card2){
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

    //Destroy state
    void DestroyCards() {
        playerCards[playerSelected.cardPos].OnDestroy();
        enemyCards[enemySelected.cardPos].OnDestroy();  

        enemyCards[enemySelected.cardPos] = null;
        playerCards[playerSelected.cardPos] = null;

        playerSelected.OnDestroy();
        enemySelected.OnDestroy();

        playerSelected = null;
        enemySelected = null;

        middleText.text = "";
        currentState = states.idle;
    }

    //Idle state
    bool CheckEndGame(){
        int count = 0;
        for(int i = 0; i < playerCards.Length; i++){
            if (playerCards[i] != null) { count++; }
        }
        if(count == 0) { return true; }

        return false;
    }
    condition CheckEndGameCondition() {
        if(playerScore > enemyScore) { return condition.win; }
        if(enemyScore > playerScore) { return condition.lose; }
        return condition.draw;
    }

    //End state
    void EndGame(){
        middleText.text = CheckEndGameCondition().ToString();
    }

    //Reset the game back to normal
    public void Reset(){
        playerScore = 0;
        enemyScore = 0;

        middleText.text = "";
        leftText.text = "Player " + playerScore.ToString();
        rightText.text = enemyScore.ToString() + " Enemy";

        currentState = states.start;
    }

    //Render the cards in the scene next to the opponent
    void RenderEnemyCards(){
        for(int i = 0; i < enemyCardsInScene.Length; i++){
            enemyCardsInScene[i].SetActive(false);
        }

        for(int i = 0; i < enemyCardCount; i++){
            enemyCardsInScene[i].SetActive(true);
        }
    }
}
