using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour {

    public bool FollowVertically = true;
    public bool FollowHorizontally = true;

    public float MinY;
    public float MaxY;

    public float MinX;
    public float MaxX;

    private float DistanceFromCamera = 14f;
    private float smoothTime = 0.2f;
    private float xVelocity;
    private float yVelocity;
	
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x, Input.mousePosition.y, DistanceFromCamera));

        if (FollowHorizontally)
        {
            float newX = Mathf.SmoothDamp(transform.position.x, mousePos.x, ref xVelocity, smoothTime);
            transform.position = new Vector3(Mathf.Clamp(newX, MinX, MaxX), transform.position.y, transform.position.z);
        }

        if (FollowVertically)
        {
            float newY = Mathf.SmoothDamp(transform.position.y, mousePos.y, ref yVelocity, smoothTime);
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(newY, MinY, MaxY), transform.position.z);
        }
    }
}
