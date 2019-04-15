using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class EnemyUnitController : MonoBehaviour
{
    private GameState gs;
    private GameManager gm;
    private UnitController unit;

    public bool attackMode = true;
    // Start is called before the first frame update
    void Start()
    {
        gs = FindObjectOfType<GameState>();
        gm = FindObjectOfType<GameManager>();
        unit = GetComponent<UnitController>();
    }

    [Task]
    void IsEnemyTurn()
    {
        if (gm.CurrentTurn == Turn.Enemy)
        {
            Task.current.Succeed();
        }
    }

    [Task]
    void TurnUsed()
    {
        if (unit.turnUsed)
        {
            Task.current.Succeed();
        }
    }

    [Task]
    void AttackMode()
    {
        if (attackMode)
        {
            Task.current.Succeed();
        }
    }
    
    [Task]
    void TargetInRange()
    {
        Task.current.Succeed();
    }

    [Task]
    void Fire()
    {
        Task.current.Succeed();
    }

    [Task]
    void TargetInSight()
    {
        Task.current.Succeed();
    }

    [Task]
    void SetMoveLocation()
    {
        Task.current.Succeed();
    }

    [Task]
    void Moving()
    {
        Task.current.Succeed();
    }

    [Task]
    void MoveToLocation()
    {
        Task.current.Succeed();
    }

    [Task]
    void BaseInSight()
    {
        Task.current.Succeed();
    }

    [Task]
    void Patrol()
    {
        Task.current.Succeed();
    }
}
