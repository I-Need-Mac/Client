using UnityEngine;
using Cinemachine;
using BFM;
using UnityEngine.Tilemaps;

public class CameraManager : SingletonBehaviour<CameraManager>
{
    private CinemachineVirtualCamera virtualCam;
    private Camera cam;

    private Tilemap tileMap;
    private float extra = 2.0f;
    
    protected override void Awake()
    {
        virtualCam = transform.Find("Cinemachine").GetComponent<CinemachineVirtualCamera>();
        cam = transform.Find("PlayerCamera").GetComponent<Camera>();
    }

    private void Start()
    {
        ConfinerSetting("Floor");
        tileMap = GameManager.Instance.tileMap;
    }

    #region
    //카메라 범위에서 그리드 벡터값 가져오게 해줌 아마도

    /*
     * 그리드 좌표는 쓰기 편하게 알파벳으로~!
     * A B C
     * D   E
     * F G H
     */

    public Vector2 RandomPosInGrid(SponeMobLocation location)
    {
        Vector2 camPoint = cam.ScreenToWorldPoint(new Vector2(cam.orthographicSize * Screen.width / Screen.height, cam.orthographicSize));
        Vector2 weight = new Vector2((cam.orthographicSize * Screen.width / Screen.height * 2) / 3.0f, (cam.orthographicSize * 2) / 3.0f);

        Vector2 pos;
        switch (location)
        {
            case SponeMobLocation.TOPLEFT:
                pos = new Vector2(Random.Range(camPoint.x - extra, camPoint.x), Random.Range(-camPoint.y, -camPoint.y + extra));
                if (tileMap.GetTile(tileMap.WorldToCell(pos)) == null)
                {
                    DebugManager.Instance.PrintDebug("escape!");
                    return new Vector2(Random.Range(camPoint.x, camPoint.x + weight.x), Random.Range(camPoint.y + weight.y * 2, -camPoint.y));
                }
                return pos;
            case SponeMobLocation.TOP:
                pos = new Vector2(Random.Range(camPoint.x + weight.x, camPoint.x + weight.x * 2), Random.Range(-camPoint.y, -camPoint.y + extra));
                if (tileMap.GetTile(tileMap.WorldToCell(pos)) == null)
                {
                    DebugManager.Instance.PrintDebug("escape!");
                    return new Vector2(Random.Range(camPoint.x + weight.x, camPoint.x + weight.x * 2), Random.Range(camPoint.y + weight.y * 2, -camPoint.y));
                }
                return pos;
            case SponeMobLocation.TOPRIGHT:
                pos = new Vector2(Random.Range(-camPoint.x, -camPoint.x + extra), Random.Range(-camPoint.y, -camPoint.y + extra));
                if (tileMap.GetTile(tileMap.WorldToCell(pos)) == null)
                {
                    DebugManager.Instance.PrintDebug("escape!");
                    return new Vector2(Random.Range(camPoint.x + weight.x * 2, -camPoint.x), Random.Range(camPoint.y + weight.y * 2, -camPoint.y));
                }
                return pos;
            case SponeMobLocation.LEFT:
                pos = new Vector2(Random.Range(camPoint.x - extra, camPoint.x), Random.Range(camPoint.y + weight.y, camPoint.y + weight.y * 2));
                if (tileMap.GetTile(tileMap.WorldToCell(pos)) == null)
                {
                    DebugManager.Instance.PrintDebug("escape!");
                    return new Vector2(Random.Range(camPoint.x, camPoint.x + weight.x), Random.Range(camPoint.y + weight.y, camPoint.y + weight.y * 2));
                }
                return pos;
            case SponeMobLocation.RIGHT:
                pos = new Vector2(Random.Range(-camPoint.x, -camPoint.x + extra), Random.Range(camPoint.y + weight.y, camPoint.y + weight.y * 2));
                if (tileMap.GetTile(tileMap.WorldToCell(pos)) == null)
                {
                    DebugManager.Instance.PrintDebug("escape!");
                    return new Vector2(Random.Range(camPoint.x + weight.x * 2, -camPoint.x), Random.Range(camPoint.y + weight.y, camPoint.y + weight.y * 2));
                }
                return pos;
            case SponeMobLocation.BOTTOMLEFT:
                pos = new Vector2(Random.Range(camPoint.x - extra, camPoint.x), Random.Range(camPoint.y - extra, camPoint.y));
                if (tileMap.GetTile(tileMap.WorldToCell(pos)) == null)
                {
                    DebugManager.Instance.PrintDebug("escape!");
                    return new Vector2(Random.Range(camPoint.x, camPoint.x + weight.x), Random.Range(camPoint.y, camPoint.y + weight.y));
                }
                return pos;
            case SponeMobLocation.BOTTOM:
                pos = new Vector2(Random.Range(camPoint.x + weight.x, camPoint.x + weight.x * 2), Random.Range(camPoint.y - extra, camPoint.y));
                if (tileMap.GetTile(tileMap.WorldToCell(pos)) == null)
                {
                    DebugManager.Instance.PrintDebug("escape!");
                    return new Vector2(Random.Range(camPoint.x + weight.x, camPoint.x + weight.x * 2), Random.Range(camPoint.y, camPoint.y + weight.y));
                }
                return pos;
            case SponeMobLocation.BOTTOMRIGHT:
                pos = new Vector2(Random.Range(-camPoint.x, -camPoint.x + extra), Random.Range(camPoint.y - extra, camPoint.y));
                if (tileMap.GetTile(tileMap.WorldToCell(pos)) == null)
                {
                    DebugManager.Instance.PrintDebug("escape!");
                    return new Vector2(Random.Range(camPoint.x + weight.x * 2, -camPoint.x), Random.Range(camPoint.y, camPoint.y + weight.y));
                }
                return pos;
            case SponeMobLocation.ROUND:
                return Vector2.zero;
            case SponeMobLocation.FACE:
                return Vector2.zero;
            case SponeMobLocation.BACK:
                return Vector2.zero;
            default:
                return Vector2.zero;
        }
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
