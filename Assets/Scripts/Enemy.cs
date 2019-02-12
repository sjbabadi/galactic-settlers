using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameState gs;
    [SerializeField] private Tilemap map;

    public float health;
    public float power;
    public float eMoney;
    public float eFood;
    public float eUnitsMax;
    public float ePopulationCurrent;

    float timeDelay = 10f;

    //load prefabs
    public GameObject eFarm;
    public GameObject eHouse;
    public GameObject eMine;
    public GameObject eBarr;

    private Vector3 enemyBaseLoc = new Vector3(18f, 1f, 11f);   //enemy base location

    //set building rate
    public float _buildRate = 5.0f;
    public float _canBuild = 0.0f;

    private void Start()
    {
        health = 100;
        power = 30;
        gs.enemies.Add(gameObject.GetComponent<Enemy>());

        eMoney = 10000;
        eFood = 0;
        eUnitsMax = 0;
        ePopulationCurrent = 0;

    }

    public void Update()
    {
        if (eMoney > 500)
        {
            if (eFood == ePopulationCurrent)
            {
                build(eFarm);
            } else if (eUnitsMax == 0 || eUnitsMax == ePopulationCurrent)
            {
                build(eHouse);
            } else if (eUnitsMax > ePopulationCurrent)
            {
                build(eBarr);
            }
            
        }



    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            map.deletePiece(transform.position);
            Destroy(gameObject);
        }
    }

    public void Attack()
    {
        foreach (Soldier enemy in gs.allies)
        {
            if (enemy != null)
            {
                if (Vector3.Distance(enemy.transform.position, transform.position) < 2f)
                {
                    enemy.TakeDamage(power);
                }
            }
        }
    }

    public void TakeAction()
    {
        Attack();
    }

    public void build(GameObject building)
    {


        if (Time.time > _canBuild)
        {
            //build
            var position = new Vector3(Random.Range(-5, 1), 0, Random.Range(-5, 1));
            Instantiate(building, enemyBaseLoc + position, Quaternion.identity);
            _canBuild = Time.time + _buildRate;

            //cost
            eMoney -= 500;

            if (building == eFarm)
            {
                eFood += 10;
            } else if (building == eHouse)
            {
                eUnitsMax += 10;
            } else if (building == eBarr)
            {
                ePopulationCurrent += 10;
            } else
            {
                eMoney += 2000;
            }

            Debug.Log("built = " + building);
            Debug.Log("efood = " + eFood);
            Debug.Log("money = " + eMoney);
            Debug.Log("units max = " + eUnitsMax);
        }


    }

}
