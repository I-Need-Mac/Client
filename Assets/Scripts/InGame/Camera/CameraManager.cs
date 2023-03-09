using UnityEngine;
using Cinemachine;
using BFM;

public class CameraManager : SingletonBehaviour<CameraManager>
{
    private CinemachineVirtualCamera virtualCam;
    private Camera cam;

    private float x;
    private float y;

    private float weightX;
    private float weightY;

    protected override void Awake()
    {
        virtualCam = transform.Find("Cinemachine").GetComponent<CinemachineVirtualCamera>();
        cam = transform.Find("PlayerCamera").GetComponent<Camera>();
    }

    private void Start()
    {
        //Vector2 vector = cam.ScreenToWorldPoint();
        //x = vector.x;
        //y = vector.y;
        //weightX = Mathf.Abs(x) * 2 / 3.0f;
        //weightY = Mathf.Abs(y) * 2 / 3.0f;
        ConfinerSetting("Floor");
    }

    #region
    //카메라 범위에서 그리드 벡터값 가져오게 해줌 아마도

    /*
     * 그리드 좌표는 쓰기 편하게 알파벳으로~!
     * A B C
     * D   E
     * F G H
     */

    public Vector2 RandomPosInGrid(string grid)
    {
        Vector2 camPoint = cam.ScreenToWorldPoint(new Vector2(cam.orthographicSize * Screen.width / Screen.height, cam.orthographicSize));
        Vector2 weight = new Vector2((cam.orthographicSize * Screen.width / Screen.height * 2) / 3.0f, (cam.orthographicSize * 2) / 3.0f);

        switch (grid)
        {
            case "A":
                return new Vector2(Random.Range(camPoint.x, camPoint.x + weight.x), Random.Range(camPoint.y + weight.y * 2, -camPoint.y));
            case "B":
                return new Vector2(Random.Range(camPoint.x + weight.x, camPoint.x + weight.x * 2), Random.Range(camPoint.y + weight.y * 2, -camPoint.y));
            case "C":
                return new Vector2(Random.Range(camPoint.x + weight.x * 2, -camPoint.x), Random.Range(camPoint.y + weight.y * 2, -camPoint.y));
            case "D":
                return new Vector2(Random.Range(camPoint.x, camPoint.x + weight.x), Random.Range(camPoint.y + weight.y, camPoint.y + weight.y * 2));
            case "E":
                return new Vector2(Random.Range(camPoint.x + weight.x * 2, -camPoint.x), Random.Range(camPoint.y + weight.y, camPoint.y + weight.y * 2));
            case "F":
                return new Vector2(Random.Range(camPoint.x, camPoint.x + weight.x), Random.Range(camPoint.y, camPoint.y + weight.y));
            case "G":
                return new Vector2(Random.Range(camPoint.x + weight.x, camPoint.x + weight.x * 2), Random.Range(camPoint.y, camPoint.y + weight.y));
            case "H":
                return new Vector2(Random.Range(camPoint.x + weight.x * 2, -camPoint.x), Random.Range(camPoint.y, camPoint.y + weight.y));
            default:
                return Vector2.zero;
                //case "A":
                //    return new Vector2(camPoint.x + weight.x, camPoint.y + weight.y * 2);
                //case "B":
                //    return new Vector2(camPoint.x + weight.x * 2, camPoint.y + weight.y * 2);
                //case "C":
                //    return new Vector2(-camPoint.x, camPoint.y + weight.y * 2);
                //case "D":
                //    return new Vector2(camPoint.x + weight.x, camPoint.y + weight.y);
                //case "E":
                //    return new Vector2(-camPoint.x, camPoint.y + weight.y);
                //case "F":
                //    return new Vector2(camPoint.x + weight.x, camPoint.y);
                //case "G":
                //    return new Vector2(camPoint.x + weight.x * 2, camPoint.y);
                //case "H":
                //    return new Vector2(-camPoint.x, camPoint.y);
                //default:
                //    return Vector2.zero;
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
        DebugManager.Instance.PrintDebug("test: "+GameObject.Find(mapName));
        virtualCam.GetComponent<CinemachineConfiner>().m_BoundingShape2D =
            GameObject.Find(mapName).GetComponent<CompositeCollider2D>();
    }
}
