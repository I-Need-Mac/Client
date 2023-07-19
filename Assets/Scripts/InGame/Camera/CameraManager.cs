using UnityEngine;
using Cinemachine;
using BFM;
using UnityEngine.Tilemaps;

public class CameraManager : SingletonBehaviour<CameraManager>
{
    private const string SPAWN_POINT_NAME = "Floor";
    private const float RADIUS = 10.0f;
    private const int ROUND_AMOUNT = 50;

    private CinemachineVirtualCamera virtualCam;

    private float extra = 4.0f;

    public Camera cam { get; private set; }

    protected override void Awake()
    {
        virtualCam = GetComponentInChildren<CinemachineVirtualCamera>();
        cam = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        ConfinerSetting("Floor");
    }

    public Vector2 GetMousePoint()
    {
        return cam.ScreenToWorldPoint(Input.mousePosition);
    }

    #region
    public Vector2[] Round(int amount)
    {
        Vector2[] poses = new Vector2[amount];
        float angle = 360 / amount;

        float radius = RADIUS;

        if (amount > ROUND_AMOUNT)
        {
            radius = RADIUS * 1.25f;
        }

        for (int i = 0; i < amount; i++)
        {
            Vector2 pos = new Vector2(Mathf.Cos(i * angle * Mathf.Deg2Rad), Mathf.Sin(i * angle * Mathf.Deg2Rad)) * radius + (Vector2)GameManager.Instance.player.transform.position;
            poses[i] = pos;
            try
            {
                if (Physics2D.OverlapPoint(pos, 1 << (int)LayerConstant.MAP).name.Equals(SPAWN_POINT_NAME))
                {
                    poses[i] = pos;
                }
                else
                {
                    --i;
                    angle += 15;
                }
            }
            catch
            {
                --i;
                angle += 15;
            }
        }

        return poses;
    }

    //DebugManager.Instance.PrintDebug("[CAM] topleft:" + cam.ScreenToWorldPoint(new Vector2(0, Screen.height)));
    //DebugManager.Instance.PrintDebug("[CAM] topright:" + cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)));
    //DebugManager.Instance.PrintDebug("[CAM] bottomleft:" + cam.ScreenToWorldPoint(new Vector2(0, 0)));
    //DebugManager.Instance.PrintDebug("[CAM] bottomright:" + cam.ScreenToWorldPoint(new Vector2(Screen.width, 0)));
    private bool IsInCamera(Vector2 target)
    {
        Vector2 pos = this.cam.WorldToViewportPoint(target);
        if (pos.x > 0 && pos.x < 1 && pos.y > 0 && pos.y < 1)
        {
            return true;
        }

        return false;
    }

    private bool IsFloor(Vector2 target)
    {
        try
        {
            return Physics2D.OverlapPoint(target, 1 << (int)LayerConstant.MAP).name.Equals(SPAWN_POINT_NAME);
        }
        catch
        {
            return false;
        }
    }

    public Vector2 RandomPosInGrid(SpawnMobLocation location)
    {
        //Vector2 camPoint = cam.ScreenToWorldPoint(new Vector2(cam.orthographicSize * Screen.width / Screen.height, cam.orthographicSize));
        //Vector2 weight = new Vector2((cam.orthographicSize * Screen.width / Screen.height * 2) / 3.0f, (cam.orthographicSize * 2) / 3.0f);

        Vector2 topLeft = cam.ScreenToWorldPoint(new Vector2(0, Screen.height));
        Vector2 topRight = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 bottomLeft = cam.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 bottomRight = cam.ScreenToWorldPoint(new Vector2(Screen.width, 0));

        Vector3 pos = Vector3.zero;
        switch (location)
        {
            case SpawnMobLocation.TOPLEFT:
                pos = new Vector2(Random.Range(topLeft.x - extra, topLeft.x), Random.Range(topLeft.y, topLeft.y + extra));
                break;
            case SpawnMobLocation.TOP:
                pos = new Vector2(Random.Range(topLeft.x, topRight.x), Random.Range(topLeft.y, topLeft.y + extra));
                break;
            case SpawnMobLocation.TOPRIGHT:
                pos = new Vector2(Random.Range(topRight.x, topRight.x + extra), Random.Range(topLeft.y, topLeft.y + extra));
                break;
            case SpawnMobLocation.LEFT:
                pos = new Vector2(Random.Range(topLeft.x - extra, topLeft.x), Random.Range(bottomLeft.y, topLeft.y));
                break;
            case SpawnMobLocation.RIGHT:
                pos = new Vector2(Random.Range(topRight.x, topRight.x + extra), Random.Range(bottomLeft.y, topLeft.y));
                break;
            case SpawnMobLocation.BOTTOMLEFT:
                pos = new Vector2(Random.Range(topLeft.x - extra, topLeft.x), Random.Range(bottomLeft.y - extra, bottomLeft.y));
                break;
            case SpawnMobLocation.BOTTOM:
                pos = new Vector2(Random.Range(topLeft.x, topRight.x), Random.Range(bottomLeft.y - extra, bottomLeft.y));
                break;
            case SpawnMobLocation.BOTTOMRIGHT:
                pos = new Vector2(Random.Range(topRight.x, topRight.x + extra), Random.Range(bottomLeft.y - extra, bottomLeft.y));
                break;
            default:
                pos = Vector3.zero;
                break;
        }

        if (!IsFloor(pos) || IsInCamera(pos))
        {
            return RandomPosInGrid((SpawnMobLocation)(((int)location + 1) % 8));
        }

        pos.z = (int)LayerConstant.MONSTER;
        return pos;
        //try
        //{
        //    switch (location)
        //    {
        //        case SpawnMobLocation.TOPLEFT:
        //            pos = new Vector2(Random.Range(camPoint.x - extra, camPoint.x), Random.Range(-camPoint.y, -camPoint.y + extra));
        //            if (!Physics2D.OverlapPoint(pos, 1 << (int)LayerConstant.MAP).name.Equals(SPAWN_POINT_NAME))
        //            {
        //                DebugManager.Instance.PrintDebug("SpawnTest: TOPLEFT");
        //                pos = new Vector2(Random.Range(camPoint.x, camPoint.x + weight.x), Random.Range(camPoint.y + weight.y * 2, -camPoint.y));
        //            }
        //            break;
        //        case SpawnMobLocation.TOP:
        //            pos = new Vector2(Random.Range(camPoint.x + weight.x, camPoint.x + weight.x * 2), Random.Range(-camPoint.y, -camPoint.y + extra));
        //            if (!Physics2D.OverlapPoint(pos, 1 << (int)LayerConstant.MAP).name.Equals(SPAWN_POINT_NAME))
        //            {
        //                DebugManager.Instance.PrintDebug("SpawnTest: TOP");
        //                pos = new Vector2(Random.Range(camPoint.x + weight.x, camPoint.x + weight.x * 2), Random.Range(camPoint.y + weight.y * 2, -camPoint.y));
        //            }
        //            break;
        //        case SpawnMobLocation.TOPRIGHT:
        //            pos = new Vector2(Random.Range(-camPoint.x, -camPoint.x + extra), Random.Range(-camPoint.y, -camPoint.y + extra));
        //            if (!Physics2D.OverlapPoint(pos, 1 << (int)LayerConstant.MAP).name.Equals(SPAWN_POINT_NAME))
        //            {
        //                DebugManager.Instance.PrintDebug("SpawnTest: TOPRIGHT");
        //                pos = new Vector2(Random.Range(camPoint.x + weight.x * 2, -camPoint.x), Random.Range(camPoint.y + weight.y * 2, -camPoint.y));
        //            }
        //            break;
        //        case SpawnMobLocation.LEFT:
        //            pos = new Vector2(Random.Range(camPoint.x - extra, camPoint.x), Random.Range(camPoint.y + weight.y, camPoint.y + weight.y * 2));
        //            if (!Physics2D.OverlapPoint(pos, 1 << (int)LayerConstant.MAP).name.Equals(SPAWN_POINT_NAME))
        //            {
        //                DebugManager.Instance.PrintDebug("SpawnTest: LEFT");
        //                pos = new Vector2(Random.Range(camPoint.x, camPoint.x + weight.x), Random.Range(camPoint.y + weight.y, camPoint.y + weight.y * 2));
        //            }
        //            break;
        //        case SpawnMobLocation.RIGHT:
        //            pos = new Vector2(Random.Range(-camPoint.x, -camPoint.x + extra), Random.Range(camPoint.y + weight.y, camPoint.y + weight.y * 2));
        //            if (!Physics2D.OverlapPoint(pos, 1 << (int)LayerConstant.MAP).name.Equals(SPAWN_POINT_NAME))
        //            {
        //                DebugManager.Instance.PrintDebug("SpawnTest: RIGHT");
        //                pos = new Vector2(Random.Range(camPoint.x + weight.x * 2, -camPoint.x), Random.Range(camPoint.y + weight.y, camPoint.y + weight.y * 2));
        //            }
        //            break;
        //        case SpawnMobLocation.BOTTOMLEFT:
        //            pos = new Vector2(Random.Range(camPoint.x - extra, camPoint.x), Random.Range(camPoint.y - extra, camPoint.y));
        //            if (!Physics2D.OverlapPoint(pos, 1 << (int)LayerConstant.MAP).name.Equals(SPAWN_POINT_NAME))
        //            {
        //                DebugManager.Instance.PrintDebug("SpawnTest: BOTTOMLEFT");
        //                pos = new Vector2(Random.Range(camPoint.x, camPoint.x + weight.x), Random.Range(camPoint.y, camPoint.y + weight.y));
        //            }
        //            break;
        //        case SpawnMobLocation.BOTTOM:
        //            pos = new Vector2(Random.Range(camPoint.x + weight.x, camPoint.x + weight.x * 2), Random.Range(camPoint.y - extra, camPoint.y));
        //            if (!Physics2D.OverlapPoint(pos, 1 << (int)LayerConstant.MAP).name.Equals(SPAWN_POINT_NAME))
        //            {
        //                DebugManager.Instance.PrintDebug("SpawnTest: BOTTOM");
        //                pos = new Vector2(Random.Range(camPoint.x + weight.x, camPoint.x + weight.x * 2), Random.Range(camPoint.y, camPoint.y + weight.y));
        //            }
        //            break;
        //        case SpawnMobLocation.BOTTOMRIGHT:
        //            pos = new Vector2(Random.Range(-camPoint.x, -camPoint.x + extra), Random.Range(camPoint.y - extra, camPoint.y));
        //            if (!Physics2D.OverlapPoint(pos, 1 << (int)LayerConstant.MAP).name.Equals(SPAWN_POINT_NAME))
        //            {
        //                DebugManager.Instance.PrintDebug("SpawnTest: BOTTOMRIGHT");
        //                pos = new Vector2(Random.Range(camPoint.x + weight.x * 2, -camPoint.x), Random.Range(camPoint.y, camPoint.y + weight.y));
        //            }
        //            break;
        //        default:
        //            pos = Vector3.zero;
        //            break;
        //    }
        //}
        //catch
        //{
        //    return Vector2.zero;
        //}
    }

    #endregion

    private void RecursiveChild(Transform trans, LayerConstant layer)
    {
        trans.gameObject.layer = (int)layer;
        trans.localPosition = new Vector3(trans.position.x, trans.position.y, (int)layer);

        foreach (Transform child in trans)
        {
            RecursiveChild(child, layer);
        }
    }

    private void ConfinerSetting(string mapName)
    {
        virtualCam.GetComponent<CinemachineConfiner>().m_BoundingShape2D =
            GameObject.Find(mapName).GetComponent<CompositeCollider2D>();
    }
}
