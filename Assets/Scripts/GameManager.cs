using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        //yield return StartCoroutine("EndGameRoutine");
        //TODO: EnDGameRoutine will run the game over screen -- post vertical slice
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
            //isGameOver = IsWinner();

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

    //bool IsWinner()
    //{

    //}

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

    private void Update()
    {
        // This exists merely to test EndGame() functionality
        if (Input.GetKeyDown("e"))
        {
            isGameOver = true;

            if (Random.value > 0.5f)
            {
                FindObjectOfType<GameState>().enemyBase.health = 0;
            }
            else
            {
                FindObjectOfType<GameState>().playerBase.health = 0;
            }

            StartCoroutine(EndGame());
        }
    }

    IEnumerator EndGame()
    {
        if (!isGameOver)
        {
            Debug.Log("Who called me!?");
        }
        else
        {
            GameState gs = FindObjectOfType<GameState>();
            if (gs.enemyBase.health <= 0 || gs.playerBase.health <= 0 || gs.enemyBase == null || gs.playerBase == null)
            {
                GameObject es = FindObjectOfType<HUDController>().transform.Find("EndScreen").gameObject;
                es.SetActive(true);

                if (gs.enemyBase.health <= 0 || gs.enemyBase == null)
                {
                    es.transform.Find("Win").gameObject.SetActive(true);
                }
                else
                {
                    es.transform.Find("Lose").gameObject.SetActive(true);
                }

                yield return new WaitForSeconds(2);
                FindObjectOfType<Animator>().SetBool("BattleSceneFade", true);
                yield return new WaitForSeconds(2);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
            else
            {
                Debug.Log("Game over but there is still health in the tank");
            }
        }
    }
}
