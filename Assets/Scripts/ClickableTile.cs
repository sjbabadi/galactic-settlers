using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTile : MonoBehaviour
{

    public int tileX;
    public int tileY;
    public Tile_map map;

    void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click!");

            map.GeneratePathTo(tileX, tileY);

            while (map.selectedUnit.GetComponent<Unit>().currentPath != null)
            {
                map.selectedUnit.GetComponent<Unit>().MoveNextTile();
            }

            map.selectedUnit = null;
        }
    }
}
