using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public GameManager gm;

    public bool isTurnComplete = false;

    protected virtual void Awake()
    {
        gm = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    public void FinishTurn()
    {
        isTurnComplete = true;

        if(gm != null)
        {
            gm.UpdateTurn();
        }
    }
}
