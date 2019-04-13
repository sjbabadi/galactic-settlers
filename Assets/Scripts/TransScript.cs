using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransScript : MonoBehaviour
{
    [SerializeField]
    private GameObject placeObject;

    [SerializeField]
    private LayerMask buildingLayer;

    private Vector2 mousePos;

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y));

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseRay = Camera.main.ScreenToWorldPoint(transform.position);
            RaycastHit2D rayHit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, buildingLayer);
            Debug.Log(rayHit.collider.name);
            //if we don't hit another building, build here
            if (rayHit.collider.gameObject.GetComponent<Tile>().walkable && rayHit.collider.gameObject.GetComponent<Tile>().empty)
            {
                Instantiate(placeObject, transform.position, Quaternion.identity);
                PlacementScript.ClearSelection();
                foreach (Tile t in FindObjectOfType<GameState>().playerBuildingLocations)
                {
                    t.movementSelect = false;
                }
            }
        }
    }
}