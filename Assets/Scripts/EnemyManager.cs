using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : TurnManager
{
    GameState gs;
    Vector2 basePos;

    [SerializeField] public GameObject enemyBarracks;
    [SerializeField] public GameObject enemyFarm;
    [SerializeField] public GameObject enemyMine;
    [SerializeField] public GameObject enemyUnit;

    protected override void Awake()
    {
        base.Awake();
        gs =Object.FindObjectOfType<GameState>().GetComponent<GameState>();

    }

    void Start()
    {
        basePos = new Vector2(gs.enemyBase.transform.position.x, gs.enemyBase.transform.position.y);
    }

    public void PlayTurn()
    {
        StartCoroutine(PlayTurnRoutine());
    }

    IEnumerator PlayTurnRoutine()
    {
        //enemy's action sequence
        yield return new WaitForSeconds(5f);
        GenerateBuilding(new Vector2(0,2), enemyFarm);
        FinishTurn();
    }

    void GenerateBuilding(Vector2 offset, GameObject buildingType)
    {
        Vector2 spawnPos = basePos + offset;

        //instantiate indicated building type at position
        GameObject g = (GameObject)Instantiate(buildingType, spawnPos, Quaternion.identity);

        //add building to buildingCounts

        //add building to gs.enemyBuildings

    }

    void GenerateUnit()
    {
        //get position
        //instantiate at position
        //add unit to gs.enemyUnits
        //add necessary scripts to unit (attack, movement, etc)
    }
}