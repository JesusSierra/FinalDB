using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public float XMargin = 1.0f;
    public float YMargin = 1.0f;

    public float xSmooth = 10.0f;
    public float ySmooth = 10.0f;

    public Vector2 maxXAndY;
    public Vector2 minXAndY;

    public Transform cameraTarget;

    void Awake ()
    {
        cameraTarget =GameObject.FindGameObjectWithTag ("CameraTarget").transform;
    }

    bool CheckXMargin()
    {
        return Mathf.Abs (transform.position.x - cameraTarget.position.x) > XMargin;
    }

    bool CheckYMargin()
    {
        return Mathf.Abs (transform.position.y - cameraTarget.position.y) > YMargin;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        TrackPlayer();
    }

    void TrackPlayer()
    {
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        if (CheckXMargin())
        {
            targetX = Mathf.Lerp(transform.position.x, cameraTarget.position.x, xSmooth * Time.deltaTime);
        }

        if (CheckYMargin())
        {
            targetY = Mathf.Lerp(transform.position.y, cameraTarget.position.y, ySmooth * Time.deltaTime);
        }

        targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
        targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
}
