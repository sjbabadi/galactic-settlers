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
    GameState gs;

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

    public float delay = 1f;

    void Awake()
    {
        player = Object.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
        enemy = Object.FindObjectOfType<EnemyManager>().GetComponent<EnemyManager>();
        gs = Object.FindObjectOfType<GameState>().GetComponent<GameState>();
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
        //yield return StartCoroutine("StartGameRoutine");
        yield return StartCoroutine("PlayGameRoutine");
        yield return StartCoroutine("EndGameRoutine");
    }

    IEnumerator StartGameRoutine()
    {
        Debug.Log("Start Level");
        player.inputEnabled = false;

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
        Debug.Log("Play Level");
        isGamePlaying = true;
        yield return new WaitForSeconds(delay);
        player.inputEnabled = true;
        while (!isGameOver)
        {
            //check for win condition (base health to zero)
            isGameOver = IsWinner();
            yield return null;
        }
    }

    void PlayPlayerTurn()
    {
        currentTurn = Turn.Player;
        Debug.Log(currentTurn);
        player.isTurnComplete = false;
        player.inputEnabled = true;
    }

    void PlayEnemyTurn()
    {
        currentTurn = Turn.Enemy;
        Debug.Log(currentTurn);
        enemy.isTurnComplete = false;
        enemy.PlayTurn();
    }

    bool IsWinner()
    {
        return (gs.playerBase.health == 0 || gs.enemyBase.health == 0);
    }

    public void UpdateTurn()
    {
        if (currentTurn == Turn.Player && player != null)
        {
            if (player.isTurnComplete)
            {
                PlayEnemyTurn();
            }
        }
        else if (currentTurn == Turn.Enemy)
        {
            //if enemy turn is complete, play player turn
            if (enemy.isTurnComplete)
            {
                PlayPlayerTurn();
            }
        }
    }

    private void Update()
    {
        // This exists merely to test EndGame() functionality
        if (Input.GetKeyDown("e"))
        {
            if (Random.value > 0.5f)
            {
                gs.enemyBase.health = 0;
            }
            else
            {
                gs.playerBase.health = 0;
            }
        }
    }

    IEnumerator EndGameRoutine()
    {
        if (!isGameOver)
        {
            Debug.Log("Who called me!?");
        }
        else
        {
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

                yield return new WaitForSeconds(.5f);

                while (!Input.anyKeyDown)
                {
                    yield return null;
                }

                foreach (Animator anim in FindObjectsOfType<Animator>())
                {
                    if (anim.name == "Music")
                    {
                        anim.SetBool("MusicFadeOut", true);
                    }
                    else
                    {
                        anim.SetBool("BattleSceneFade", true);
                    }
                }
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
