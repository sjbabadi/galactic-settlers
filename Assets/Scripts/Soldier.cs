using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Unit
{


    // Update is called once per frame
    void Update()
    {
/*
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 15;

        Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);

        RaycastHit2D hit = Physics2D.Raycast(screenPos, Vector2.up, 1, LayerMask.NameToLayer("Player"));
        //Debug.Log(screenPos);

        if ((hit.collider != null) && (hit.collider.tag == "Player"))
        {
            Debug.Log("got here");
            map.selectedUnit = gameObject;

        }
*/
    
        if (Input.GetMouseButtonDown(0))
        {
            map.selectedUnit = gameObject;
        }
        if (map.selectedUnit)
        { 
            if (!turnUsed)
            {
                FindSelectableTiles();
            }

            turnUsed = true;
        }





        if (!moving)
        {
            //   FindSelectableTiles();
            CheckMouse();
        }
     
    
    }


    void CheckMouse()
    {

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;

            Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);

            RaycastHit2D hit = Physics2D.Raycast(screenPos, Vector2.zero);

            //Debug.Log(screenPos);

            if (hit)
            {
                if (hit.collider.tag == "Tile")
                {
                    Tile t = hit.collider.GetComponent<Tile>();

                   // Debug.Log("selected tile to move to: " + t);

                    if (t.selectable)
                    {
                        if(map.selectedUnit)
                        {
                            MoveToTile(t);
                        }
                        
                    }
                }
            }
        }
    }

}
