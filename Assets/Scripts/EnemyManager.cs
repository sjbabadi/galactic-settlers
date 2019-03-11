using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : TurnManager
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

    }

    public void PlayTurn()
    {
        StartCoroutine(PlayTurnRoutine());
    }

    IEnumerator PlayTurnRoutine()
    {
        //enemy's action sequence
        yield return null;
    }
}
