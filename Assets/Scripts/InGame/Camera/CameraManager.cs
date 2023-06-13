using UnityEngine;
using Cinemachine;
using BFM;
using UnityEngine.Tilemaps;

public class CameraManager : SingletonBehaviour<CameraManager>
{
    private const string SPAWN_POINT_NAME = "Floor";

    private CinemachineVirtualCamera virtualCam;

    private float extra = 2.0f;

    public Camera cam { get; private set; }

    protected override void Awake()
    {
        virtualCam = transform.Find("Cinemachine").GetComponent<CinemachineVirtualCamera>();
        cam = transform.Find("PlayerCamera").GetComponent<Camera>();
    }

    private void Start()
    {
        ConfinerSetting("Floor");
    }

    #region
    public Vector2 RandomPosInGrid(SpawnMobLocation location)
    {
        Vector2 camPoint = cam.ScreenToWorldPoint(new Vector2(cam.orthographicSize * Screen.width / Screen.height, cam.orthographicSize));
        Vector2 weight = new Vector2((cam.orthographicSize * Screen.width / Screen.height * 2) / 3.0f, (cam.orthographicSize * 2) / 3.0f);

        Vector3 pos = Vector3.zero;
        switch (location)
        {
            case SpawnMobLocation.TOPLEFT:
                pos = new Vector2(Random.Range(camPoint.x - extra, camPoint.x), Random.Range(-camPoint.y, -camPoint.y + extra));
                if (!Physics2D.OverlapPoint(pos, 1 << (int)LayerConstant.MAP).name.Equals(SPAWN_POINT_NAME))
                {
                    DebugManager.Instance.PrintDebug("SpawnTest: TOPLEFT");
                    pos = new Vector2(Random.Range(camPoint.x, camPoint.x + weight.x), Random.Range(camPoint.y + weight.y * 2, -camPoint.y));
                }
                break;
            case SpawnMobLocation.TOP:
                pos = new Vector2(Random.Range(camPoint.x + weight.x, camPoint.x + weight.x * 2), Random.Range(-camPoint.y, -camPoint.y + extra));
                if (!Physics2D.OverlapPoint(pos, 1 << (int)LayerConstant.MAP).name.Equals(SPAWN_POINT_NAME))
                {
                    DebugManager.Instance.PrintDebug("SpawnTest: TOP");
                    pos = new Vector2(Random.Range(camPoint.x + weight.x, camPoint.x + weight.x * 2), Random.Range(camPoint.y + weight.y * 2, -camPoint.y));
                }
                break;
            case SpawnMobLocation.TOPRIGHT:
                pos = new Vector2(Random.Range(-camPoint.x, -camPoint.x + extra), Random.Range(-camPoint.y, -camPoint.y + extra));
                if (!Physics2D.OverlapPoint(pos, 1 << (int)LayerConstant.MAP).name.Equals(SPAWN_POINT_NAME))
                {
                    DebugManager.Instance.PrintDebug("SpawnTest: TOPRIGHT");
                    pos = new Vector2(Random.Range(camPoint.x + weight.x * 2, -camPoint.x), Random.Range(camPoint.y + weight.y * 2, -camPoint.y));
                }
                break;
            case SpawnMobLocation.LEFT:
                pos = new Vector2(Random.Range(camPoint.x - extra, camPoint.x), Random.Range(camPoint.y + weight.y, camPoint.y + weight.y * 2));
                if (!Physics2D.OverlapPoint(pos, 1 << (int)LayerConstant.MAP).name.Equals(SPAWN_POINT_NAME))
                {
                    DebugManager.Instance.PrintDebug("SpawnTest: LEFT");
                    pos = new Vector2(Random.Range(camPoint.x, camPoint.x + weight.x), Random.Range(camPoint.y + weight.y, camPoint.y + weight.y * 2));
                }
                break;
            case SpawnMobLocation.RIGHT:
                pos = new Vector2(Random.Range(-camPoint.x, -camPoint.x + extra), Random.Range(camPoint.y + weight.y, camPoint.y + weight.y * 2));
                if (!Physics2D.OverlapPoint(pos, 1 << (int)LayerConstant.MAP).name.Equals(SPAWN_POINT_NAME))
                {
                    DebugManager.Instance.PrintDebug("SpawnTest: RIGHT");
                    pos = new Vector2(Random.Range(camPoint.x + weight.x * 2, -camPoint.x), Random.Range(camPoint.y + weight.y, camPoint.y + weight.y * 2));
                }
                break;
            case SpawnMobLocation.BOTTOMLEFT:
                pos = new Vector2(Random.Range(camPoint.x - extra, camPoint.x), Random.Range(camPoint.y - extra, camPoint.y));
                if (!Physics2D.OverlapPoint(pos, 1 << (int)LayerConstant.MAP).name.Equals(SPAWN_POINT_NAME))
                {
                    DebugManager.Instance.PrintDebug("SpawnTest: BOTTOMLEFT");
                    pos = new Vector2(Random.Range(camPoint.x, camPoint.x + weight.x), Random.Range(camPoint.y, camPoint.y + weight.y));
                }
                break;
            case SpawnMobLocation.BOTTOM:
                pos = new Vector2(Random.Range(camPoint.x + weight.x, camPoint.x + weight.x * 2), Random.Range(camPoint.y - extra, camPoint.y));
                if (!Physics2D.OverlapPoint(pos, 1 << (int)LayerConstant.MAP).name.Equals(SPAWN_POINT_NAME))
                {
                    DebugManager.Instance.PrintDebug("SpawnTest: BOTTOM");
                    pos = new Vector2(Random.Range(camPoint.x + weight.x, camPoint.x + weight.x * 2), Random.Range(camPoint.y, camPoint.y + weight.y));
                }
                break;
            case SpawnMobLocation.BOTTOMRIGHT:
                pos = new Vector2(Random.Range(-camPoint.x, -camPoint.x + extra), Random.Range(camPoint.y - extra, camPoint.y));
                if (!Physics2D.OverlapPoint(pos, 1 << (int)LayerConstant.MAP).name.Equals(SPAWN_POINT_NAME))
                {
                    DebugManager.Instance.PrintDebug("SpawnTest: BOTTOMRIGHT");
                    pos = new Vector2(Random.Range(camPoint.x + weight.x * 2, -camPoint.x), Random.Range(camPoint.y, camPoint.y + weight.y));
                }
                break;
            default:
                pos = Vector3.zero;
                break;
        }

        pos.z = (int)LayerConstant.MONSTER;
        return pos;
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
