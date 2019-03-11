using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGen: MonoBehaviour
{

    public GameObject Soldier;

    [SerializeField] private float waitTime = 5.0f;
    private float timer = 0.0f;

    // Update is called once per frame
    public void Update()
    {
        int numOfBarracks = GameState.buildingCounts[2];
        int numUnits = GameState.Units;
        int numUnitsAllowed = GameState.UnitMax;
        int unitsDiff = numUnitsAllowed - numUnits;

        //Debugging-------------------------------
        Debug.Log("Num of barr" + numOfBarracks);
        Debug.Log("Num of units" + numUnits);
        Debug.Log("Num of units allowed" + numUnitsAllowed);
        Debug.Log("Num diff" + unitsDiff);
        //End Debugging-------------------------------


        timer += Time.deltaTime;
        if (timer > waitTime)
        {
            //check to make sure we can instantiate units
            if (unitsDiff > 0)
            {
                //if num of units allowed is greater than or equal to num of barracks built
                if (unitsDiff >= numOfBarracks)
                {
                    for (int i = 0; i < numOfBarracks; i++)
                        {
                            Instantiate(Soldier, new Vector2(i + 2.0f, 0), Quaternion.identity);
                            GameState.Units++;
                        }
                } else
                {
                    //if num of units allowed is less than num of barracks built
                    for (int i = 0; i < numOfBarracks; i++)
                    {
                        Instantiate(Soldier, new Vector2(i + 2.0f, 0), Quaternion.identity);
                        GameState.Units++;
                    }
                }

            }

            //timer = timer - waitTime;
        }
        
    }
}