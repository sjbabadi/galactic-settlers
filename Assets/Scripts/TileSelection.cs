using UnityEngine;

public class TileSelection : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector3 WorldCoord = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 newPos = new Vector3(Mathf.FloorToInt(WorldCoord.x + 0.5f), Mathf.FloorToInt(WorldCoord.y + 0.5f), 0);
        //Debug.Log(WorldCoord.x + " " + WorldCoord.y);
        gameObject.transform.position = newPos;
    }
}


