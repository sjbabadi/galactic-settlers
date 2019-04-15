
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

    private Vector3 mouseOriginPoint;
    private Vector3 offset;
    private bool dragging;

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
        if (Input.GetMouseButton(0))
        {
            offset = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            if (!dragging)
            {
                dragging = true;
                mouseOriginPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            if (dragging)
            {
                transform.position = mouseOriginPoint - offset;
            }
        }
        else
        {
            dragging = false;
        }

    }

    //public int resWidth = 1600;
    //public int resHeight = 900;

    //private bool takeHiResShot = true;

    //public static string ScreenShotName(int width, int height)
    //{
    //    return string.Format("{0}/screen_{1}x{2}_{3}.png",
    //                         Application.dataPath,
    //                         width, height,
    //                         System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    //}

    //void Update()
    //{
    //    takeHiResShot |= Input.GetKeyDown("k");
    //    if (takeHiResShot)
    //    {
    //        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
    //        GetComponent<Camera>().targetTexture = rt;
    //        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
    //        GetComponent<Camera>().Render();
    //        RenderTexture.active = rt;
    //        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
    //        GetComponent<Camera>().targetTexture = null;
    //        RenderTexture.active = null; // JC: added to avoid errors
    //        Destroy(rt);
    //        byte[] bytes = screenShot.EncodeToPNG();
    //        string filename = ScreenShotName(resWidth, resHeight);
    //        System.IO.File.WriteAllBytes(filename, bytes);
    //        Debug.Log(string.Format("Took screenshot to: {0}", filename));
    //        takeHiResShot = false;
    //    }
    //}
	
	// Source: https://forum.unity.com/threads/export-full-scene-as-image.496232/#post-4299523
    public GameObject target;

    private RenderTexture renderTexture;
    private Camera renderCamera;
    private Vector4 bounds;

    private int resolution = 1000;
    private float cameraDistance = -2.0f;

    private void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            FindObjectOfType<TileSelection>().gameObject.SetActive(false);
            screenshot();
            FindObjectOfType<TileSelection>().gameObject.SetActive(true);
        }
    }

    void screenshot()
    {
        Debug.Log("Initializing camera and stuff...");

        gameObject.AddComponent(typeof(Camera));

        renderCamera = GetComponent<Camera>();
        renderCamera.enabled = true;
        renderCamera.cameraType = CameraType.Game;
        renderCamera.forceIntoRenderTexture = true;
        renderCamera.orthographic = true;
        renderCamera.orthographicSize = 5;
        renderCamera.aspect = 1.0f;
        renderCamera.targetDisplay = 2;

        renderTexture = new RenderTexture(resolution, resolution, 24);

        renderCamera.targetTexture = renderTexture;

        bounds = new Vector4();

        Debug.Log("Initialized successfully!");
        Debug.Log("Computing level boundaries...");

        if (target != null)
        {
            Bounds boundaries;

            if (target.GetComponentInChildren<Renderer>() != null)
            {
                boundaries = target.GetComponentInChildren<Renderer>().bounds;
            }
            else if (target.GetComponentInChildren<Collider2D>() != null)
            {
                boundaries = target.GetComponentInChildren<Collider2D>().bounds;
            }
            else
            {
                Debug.Log("Unfortunately no boundaries could be found :/");

                return;
            }

            bounds.w = boundaries.min.x;
            bounds.x = boundaries.max.x;
            bounds.y = boundaries.min.y;
            bounds.z = boundaries.max.y;
        }
        else
        {
            object[] gameObjects = FindObjectsOfType(typeof(GameObject));

            foreach (object gameObj in gameObjects)
            {
                GameObject levelElement = (GameObject)gameObj;
                Bounds boundaries = new Bounds();

                if (levelElement.GetComponentInChildren<Renderer>() != null)
                {
                    boundaries = levelElement.GetComponentInChildren<Renderer>().bounds;
                }
                else if (levelElement.GetComponentInChildren<Collider2D>() != null)
                {
                    boundaries = levelElement.GetComponentInChildren<Collider2D>().bounds;
                }
                else
                {
                    continue;
                }

                if (boundaries != null)
                {
                    bounds.w = Mathf.Min(bounds.w, boundaries.min.x);
                    bounds.x = Mathf.Max(bounds.x, boundaries.max.x);
                    bounds.y = Mathf.Min(bounds.y, boundaries.min.y);
                    bounds.z = Mathf.Max(bounds.z, boundaries.max.y);
                }
            }
        }

        Debug.Log("Boundaries computed successfuly! The computed boundaries are " + bounds);
        Debug.Log("Computing target image resolution and final setup...");

        int xRes = Mathf.RoundToInt(resolution * ((bounds.x - bounds.w) / (renderCamera.aspect * renderCamera.orthographicSize * 2 * renderCamera.aspect)));
        int yRes = Mathf.RoundToInt(resolution * ((bounds.z - bounds.y) / (renderCamera.aspect * renderCamera.orthographicSize * 2 / renderCamera.aspect)));

        Texture2D virtualPhoto = new Texture2D(xRes, yRes, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;

        Debug.Log("Success! Everything seems ready to render!");

        for (float i = bounds.w, xPos = 0; i < bounds.x; i += renderCamera.aspect * renderCamera.orthographicSize * 2, xPos++)
        {
            for (float j = bounds.y, yPos = 0; j < bounds.z; j += renderCamera.aspect * renderCamera.orthographicSize * 2, yPos++)
            {
                gameObject.transform.position = new Vector3(i + renderCamera.aspect * renderCamera.orthographicSize, j + renderCamera.aspect * renderCamera.orthographicSize, cameraDistance);

                renderCamera.Render();

                virtualPhoto.ReadPixels(new Rect(0, 0, resolution, resolution), (int)xPos * resolution, (int)yPos * resolution);

                Debug.Log("Rendered and copied chunk " + (xPos + 1) + ":" + (yPos + 1));
            }
        }

        Debug.Log("All chunks rendered! Some final adjustments and picture should be saved!");

        RenderTexture.active = null;
        renderCamera.targetTexture = null;

        byte[] bytes = virtualPhoto.EncodeToPNG();

        System.IO.File.WriteAllBytes(string.Format("{0}/Screenshots/screen_{1}x{2}_{3}.png",
                                 Application.dataPath,
                                 xRes, yRes,
                                 System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")), bytes);

        Debug.Log("All done! Always happy to help you :)");
    }

}