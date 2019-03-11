using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{ 
    private const float SPEED = 10.0f;
    private const float MIN_SIZE = 5f;
    private const float MAX_SIZE = 17f;

    //change size to zoom in/out
    //clamp min/max values
    //mouse wheel scrolling

    /// <summary>
    /// 
    /// </summary>
    private void LateUpdate()
    {

        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize
            -= Input.GetAxis("Mouse ScrollWheel") * Camera.main.orthographicSize, MIN_SIZE, MAX_SIZE);

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(SPEED * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(-SPEED * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, -SPEED * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, SPEED * Time.deltaTime, 0));
        }

    }
  
}