using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    protected UnitController unit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("up"))
        {
            transform.Translate(new Vector2(0, 1));
        }
        else if (Input.GetKeyDown("down"))
        {
            transform.Translate(new Vector2(0, -1));
        }
        else if (Input.GetKeyDown("left"))
        {
            transform.Translate(new Vector2(-1, 0));
        }
        else if (Input.GetKeyDown("right"))
        {
            transform.Translate(new Vector2(1, 0));
        }
    }
}
