using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Unit
{


    // Update is called once per frame
    void Update()
    {
        if (!moving)
        {
         //   FindSelectableTiles();
            CheckMouse();
        }
    }


    void CheckMouse()
    {

        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;

            Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);

            RaycastHit2D hit = Physics2D.Raycast(screenPos, Vector2.zero);

            if (hit)
            {
                if (hit.collider.tag == "Tile")
                {
                    Tile t = hit.collider.GetComponent<Tile>();

                    if (t.selectable)
                    {
                        MoveToTile(t);
                    }
                }
            }
        }
    }

}
