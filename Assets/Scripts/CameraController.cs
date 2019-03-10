using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{ 
    public float speed = 5.0f;

    //change size to zoom in/out
    //clamp min/max values
    //mouse wheel scrolling
    private void LateUpdate()
    {

        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize
            -= Input.GetAxis("Mouse ScrollWheel") * Camera.main.orthographicSize, 2.5f, 50f);



        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        }

    }
  
}