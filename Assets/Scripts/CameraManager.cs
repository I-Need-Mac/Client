using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : BFM.SingletonBehaviour<CameraManager>
{
    [SerializeField] Camera cam;

    private float sizeY;
    private float sizeX;

    public float Bottom
    {
        get 
        { 
            return sizeY * -1 + cam.gameObject.transform.position.y; 
        }
    }

    public float Top
    {
        get
        {
        return sizeY + cam.gameObject.transform.position.y;
        }
    }

    public float Left
    {
        get
        {
            return sizeX * -1 + cam.gameObject.transform.position.x;
        }
    }

    public float Right
    {
        get
        {
            return sizeX + cam.gameObject.transform.position.x;
        }
    }

    public float Height
    {
        get
        {
            return sizeY * 2;
        }
    }

    public float Width
    {
        get
        {
            return sizeX * 2;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        sizeY = cam.orthographicSize;
        sizeX = cam.orthographicSize * Screen.width / Screen.height;
    }
}
