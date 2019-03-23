using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    // Reference to GameState & GameManager
    private GameState gs;
    private GameManager gm;

    // Building stats
    private float health = 100;

    private void Start()
    {
        gs = FindObjectOfType<GameState>();
        gm = FindObjectOfType<GameManager>();

        gs.Units[(int)gm.CurrentTurn]++;
        if (gm.CurrentTurn == Turn.Player)
        {
            gs.playerBuildings.Add(this);
        }
        else
        {
            gs.enemyBuildings.Add(this);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        
        if(health <=0)
        {
            Destroy(gameObject);
        }
    }
}
