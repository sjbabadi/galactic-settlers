using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Turn
{
    Player,
    Enemy
}

public class GameManager : MonoBehaviour
{
    PlayerManager player;
    EnemyManager enemy;

    Turn currentTurn = Turn.Player;
    public Turn CurrentTurn { get { return currentTurn; } }

    //flags
    // has the user pressed start?
    bool hasGameStarted = false;
    public bool HasGameStarted { get { return hasGameStarted; } set { hasGameStarted = value; } }

    // have we begun gamePlay?
    bool isGamePlaying = false;
    public bool IsGamePlaying { get { return isGamePlaying; } set { isGamePlaying = value; } }

    // have we met the game over condition?
    bool isGameOver = false;
    public bool IsGameOver { get { return isGameOver; } set { isGameOver = value; } }

    void Awake()
    {
        player = Object.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
        enemy = Object.FindObjectOfType<EnemyManager>().GetComponent<EnemyManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (player != null && enemy != null)
        {
            StartCoroutine("MainGameLoop");
        }
        else
        {
            Debug.LogWarning("GameManager: no player or no enemy found");
        }
    }

    IEnumerator MainGameLoop()
    {
        yield return StartCoroutine("StartGameRoutine");
        yield return StartCoroutine("PlayGameRoutine");
        yield return StartCoroutine("EndGameRoutine");
    }

    IEnumerator StartGameRoutine()
    {
        //player.InputEnabled = false;

        while (!hasGameStarted)
        {
            //show start screen
            //user presses play
            //hasGameStarted = true
            yield return null;
        }
    }

    IEnumerator PlayGameRoutine()
    {
        isGamePlaying = true;
        //player.InputEnabled = true;

        while (!isGameOver)
        {
            yield return null;

            //check for win condition (base health to zero)
            isGameOver = IsWinner();
        }
    }

    void PlayPlayerTurn()
    {
        currentTurn = Turn.Player;
        
    }

    void PlayEnemyTurn()
    {
        currentTurn = Turn.Enemy;
    }

    void IsWinner()
    {

    }

    public void UpdateTurn()
    {
        if(currentTurn == Turn.Player && player != null)
        {
            if (player.isTurnComplete)
            {
                PlayEnemyTurn();
            }
        }
    }
}
