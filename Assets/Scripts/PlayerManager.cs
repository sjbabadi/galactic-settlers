using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : TurnManager
{
    public bool inputEnabled;

    protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        //if it's not the player's turn, immediately return / don't run anything
        if(gm.CurrentTurn != Turn.Player)
        {
            inputEnabled = false;
            return;
        }
    }
}
