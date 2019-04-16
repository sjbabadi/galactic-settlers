using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class EnemyUnitController : MonoBehaviour
{
    private GameState gs;
    private GameManager gm;
    private UnitController unit;

    public bool attackMode;
    public GameObject target;
    public Tile moveLocation;

    void Start()
    {
        gs = FindObjectOfType<GameState>();
        gm = FindObjectOfType<GameManager>();
        unit = GetComponent<UnitController>();

        attackMode = false;
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
            //Debug.Log("TurnUsed Succeed");
        }
        else
        {
            Task.current.Fail();
            //Debug.Log("TurnUsed Fail");
        }
    }

    [Task]
    void AttackMode()
    {
        if (attackMode)
        {
            Task.current.Succeed();
            //Debug.Log("AttackMode Succeed");
        }
        else
        {
            Task.current.Fail();
            //Debug.Log("AttackMode Fail");
        }
    }

    [Task]
    void HasTarget()
    {
        if (target)
        {
            Task.current.Succeed();
            //Debug.Log("HasTarget Succeed");
        }
        else
        {
            Task.current.Fail();
            //Debug.Log("HasTarget Fail");
        }
    }
    
    [Task]
    void TargetInRange()
    {
        if (Vector2.Distance(transform.position, target.transform.position) <= unit.strikingDistance)
        {
            Task.current.Succeed();
            //Debug.Log("TargetInRange Succeed");
        }
        else
        {
            Task.current.Fail();
            //Debug.Log("TargetInRange Fail");
        }
    }

    [Task]
    void Fire()
    {
        if (target)
        {
            if (target.GetComponent<UnitController>())
            {
                target.GetComponent<UnitController>().TakeDamage(unit.attackPower);
                unit.turnUsed = true;
                Task.current.Succeed();
            }
            else
            {
                target.GetComponent<BuildingController>().TakeDamage(unit.attackPower);
                unit.turnUsed = true;
                Task.current.Succeed();
            }
            //Debug.Log("Fire Succeed");
        }
        else
        {
            Task.current.Fail();
            //Debug.Log("Fire Fail");
        }
    }

    [Task]
    void TargetInSight()
    {
        Collider2D[] nearTiles = Physics2D.OverlapCircleAll(transform.position, unit.move);
        List<Tile> tiles = new List<Tile>();

        target = null;
        foreach (Collider2D hit in nearTiles)
        {
            if (hit.GetComponent<Tile>())
            {
                tiles.Add(hit.GetComponent<Tile>());
            }
        }
        foreach (Tile t in tiles)
        {
            //Debug.Log(t.transform.position);
            if (t.occupant)
            {
                //Debug.Log(t.transform.position);
                if (t.occupant.GetComponent<Unit>() && t.occupant.GetComponent<Unit>().owner != unit.owner)
                {
                    target = t.occupant;
                }
                else if (t.occupant.GetComponent<Building>() && t.occupant.GetComponent<Building>().owner != unit.owner)
                {
                    target = t.occupant;
                }
                if (target)
                {
                    break;
                }
            }
        }
        //gs.selectedUnit = null;
        if (target)
        {
            Task.current.Succeed();
            //Debug.Log("TargetInSight Succeed");
        }
        else
        {
            Task.current.Fail();
            //Debug.Log("TargetInSight Fail");
        }
    }

    [Task]
    void SetTravelLocation()
    {
        Collider2D[] nearTiles = Physics2D.OverlapCircleAll(transform.position, unit.move);
        List<Tile> tiles = new List<Tile>();
        List<Tile> moveTiles = new List<Tile>();

        moveLocation = null;
        foreach (Collider2D hit in nearTiles)
        {
            if (hit.GetComponent<Tile>() && hit.GetComponent<Tile>().empty && hit.GetComponent<Tile>().walkable)
            {
                if (attackMode)
                {
                    if (Vector2.Distance(hit.GetComponent<Tile>().transform.position, gs.playerBase.transform.position) + 1 <= Vector2.Distance(transform.position, gs.playerBase.transform.position))
                    {
                        moveTiles.Add(hit.GetComponent<Tile>());
                    }
                }
                else
                {
                    if (Vector2.Distance(hit.GetComponent<Tile>().transform.position, gs.enemyBase.transform.position) + 1 <= Vector2.Distance(transform.position, gs.enemyBase.transform.position))
                    {
                        moveTiles.Add(hit.GetComponent<Tile>());
                    }
                }
            }
        }

        moveLocation = moveTiles[Random.Range(0,moveTiles.Count)];

        if (moveLocation)
        {
            Task.current.Succeed();
            //Debug.Log("SetTravelLocation Succeed");
        }
        else
        {
            Task.current.Fail();
            //Debug.Log("SetTravelLocation Fail");
        }
    }

    [Task]
    void SetMoveLocation()
    {
        moveLocation = null;
        Vector2[] locations = new Vector2[4];
        locations[0] = new Vector2(target.transform.position.x, target.transform.position.y + 1.0f);
        locations[1] = new Vector2(target.transform.position.x, target.transform.position.y - 1.0f);
        locations[2] = new Vector2(target.transform.position.x - 1.0f, target.transform.position.y);
        locations[3] = new Vector2(target.transform.position.x + 1.0f, target.transform.position.y);

        foreach (Vector2 location in locations)
        {
            if (unit.GetTileAt(location).empty && unit.GetTileAt(location).walkable)
            {
                moveLocation = unit.GetTileAt(location);
                moveLocation.empty = false;
                break;
            }
        }

        if (moveLocation)
        {
            Task.current.Succeed();
            //Debug.Log("SetMoveLocation Succeed");
        }
        else
        {
            Task.current.Fail();
            //Debug.Log("SetMoveLocation Fail");
        }
    }

    [Task]
    void Moving()
    {
        if (unit.moving)
        {
            Task.current.Succeed();
            //Debug.Log("Moving Succeed");
        }
        else
        {
            Task.current.Fail();
            //Debug.Log("Moving Fail");
        }
    }

    [Task]
    void MoveToLocation()
    {
        unit.FindSelectableTiles();
        unit.SetTargetLocation(moveLocation);
        Task.current.Succeed();
        //Debug.Log("MoveToLocation Succeed");
    }

    [Task]
    void BaseInSight()
    {
        if (Vector2.Distance(transform.position, gs.enemyBase.transform.position) < 10f)
        {
            Task.current.Succeed();
            //Debug.Log("BaseInSight Succeed");
        }
        else
        {
            Task.current.Fail();
            //Debug.Log("BaseInSight Fail");
        }
    }

    [Task]
    void Patrol()
    {
        unit.FindSelectableTiles();
        unit.SetTargetLocation(unit.selectableTiles[Random.Range(0,unit.selectableTiles.Count)]);
        Task.current.Succeed();
        //Debug.Log("Patrol Succeed");
    }

    [Task]
    void RemoveTarget()
    {
        if (target)
        {
            target = null;
            Task.current.Succeed();
            //Debug.Log("RemoveTarget Succeed");
        }
        else
        {
            Task.current.Fail();
            //Debug.Log("RemoveTarget Fail");
        }
    }
}
