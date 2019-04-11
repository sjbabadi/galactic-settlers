
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float SPEED = 10.0f;
    private const float MIN_SIZE = 5f;
    private const float MAX_SIZE = 17f;

    private const float MIN_DISTANCE = 10f;
    private const float MAX_DISTANCE = 20f;

    /// <summary>
    /// Handles zooming in and out and moving camera around. Enforces limits on size and position.
    /// </summary>
    private void LateUpdate()
    {

        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize
            -= Input.GetAxis("Mouse ScrollWheel") * Camera.main.orthographicSize, MIN_SIZE, MAX_SIZE);

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = transform.position + new Vector3(SPEED * Time.deltaTime, 0, 0);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, MIN_DISTANCE, MAX_DISTANCE),
                                             Mathf.Clamp(transform.position.y, MIN_DISTANCE, MAX_DISTANCE),
                                             transform.position.z);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = transform.position + new Vector3(-SPEED * Time.deltaTime, 0, 0);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, MIN_DISTANCE, MAX_DISTANCE),
                                             Mathf.Clamp(transform.position.y, MIN_DISTANCE, MAX_DISTANCE),
                                             transform.position.z);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position = transform.position + new Vector3(0, -SPEED * Time.deltaTime, 0);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, MIN_DISTANCE, MAX_DISTANCE),
                                             Mathf.Clamp(transform.position.y, MIN_DISTANCE, MAX_DISTANCE),
                                             transform.position.z);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position = transform.position + new Vector3(0, SPEED * Time.deltaTime, 0);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, MIN_DISTANCE, MAX_DISTANCE),
                                             Mathf.Clamp(transform.position.y, MIN_DISTANCE, MAX_DISTANCE),
                                             transform.position.z);
        }

    }

}