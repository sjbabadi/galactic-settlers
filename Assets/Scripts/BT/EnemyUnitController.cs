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
    public GameObject target;
    public Tile moveLocation;

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
        else
        {
            Task.current.Fail();
        }
    }

    [Task]
    void TurnUsed()
    {
        if (unit.turnUsed)
        {
            Task.current.Succeed();
        }
        else
        {
            Task.current.Fail();
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
    void HasTarget()
    {
        if (target)
        {
            Task.current.Succeed();
        }
        else
        {
            Task.current.Fail();
        }
    }
    
    [Task]
    void TargetInRange()
    {
        if (Vector2.Distance(transform.position, target.transform.position) <= unit.strikingDistance)
        {
            Task.current.Succeed();
        }
        else
        {
            Task.current.Fail();
        }
    }

    [Task]
    void Fire()
    {
        target.GetComponent<UnitController>().TakeDamage(unit.attackPower);
        Task.current.Succeed();
    }

    [Task]
    void TargetInSight()
    {
        gs.selectedUnit = gameObject;
        unit.FindSelectableTiles();
        foreach (Tile t in unit.selectableTiles)
        {
            Debug.Log(t.name);
            if (t.occupant)
            {
                Debug.Log("occupant");
                if (t.occupant.GetComponent<Unit>() && t.occupant.GetComponent<Unit>().owner != Turn.Enemy)
                {
                    Debug.Log("a");
                    target = t.occupant;
                }
                else if (t.occupant.GetComponent<Building>() && t.occupant.GetComponent<Building>().owner != Turn.Enemy)
                {
                    Debug.Log("a");
                    target = t.occupant;
                }
                if (target)
                {
                    break;
                }
            }
        }
        if (target)
        {
            Task.current.Succeed();
        }
        else
        {
            Task.current.Fail();
        }
    }

    [Task]
    void SetTravelLocation()
    {
        unit.FindSelectableTiles();
        foreach (Tile t in unit.selectableTiles)
        {
            if (Vector2.Distance(t.transform.position, gs.playerBase.transform.position) + 1 <= Vector2.Distance(transform.position, gs.playerBase.transform.position))
            {

                moveLocation = t;
                Task.current.Succeed();
                break;
            }
        }
    }

    [Task]
    void SetMoveLocation()
    {
        if (target.GetComponent<UnitController>())
        {
            Vector2[] neighborLocations = target.GetComponent<UnitController>().FindNeighborLocations();
            UnitController targetUnit = target.GetComponent<UnitController>();
            unit.FindSelectableTiles();
            foreach (Tile t in unit.selectableTiles)
            {
                //t.selectable = false;
                Vector2 tilePosition = t.transform.position;
                foreach (Vector2 location in neighborLocations)
                {
                    
                    if (location == tilePosition)
                    {
                        moveLocation = t;
                        break;
                    }
                }
            }
        }
        else if (target.GetComponent<BuildingController>())
        {
            Vector2[] neighborLocations = target.GetComponent<Building>().neighbors;
            BuildingController targetBuilding = target.GetComponent<BuildingController>();
            unit.FindSelectableTiles();
            foreach (Tile t in unit.selectableTiles)
            {
                //t.selectable = false;
                Vector2 tilePosition = t.transform.position;
                foreach (Vector2 location in neighborLocations)
                {

                    if (location == tilePosition)
                    {
                        moveLocation = t;
                        break;
                    }
                }
            }
        }
        Task.current.Succeed();
    }

    [Task]
    void Moving()
    {
        if (unit.moving)
        {
            Task.current.Succeed();
        }
        else
        {
            Task.current.Fail();
        }
    }

    [Task]
    void MoveToLocation()
    {
        unit.SetTargetLocation(moveLocation);
        Task.current.Succeed();
    }

    [Task]
    void BaseInSight()
    {
        unit.FindSelectableTiles();
        foreach (Tile t in unit.selectableTiles)
        {
            if (gs.enemyBase.transform.position == t.transform.position)
            {
                Task.current.Succeed();
            }
        }
    }

    [Task]
    void Patrol()
    {
        unit.FindSelectableTiles();
        unit.SetTargetLocation(unit.selectableTiles[Random.Range(0,unit.selectableTiles.Count)]);
        Task.current.Succeed();
    }
}
