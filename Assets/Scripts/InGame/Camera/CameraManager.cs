using UnityEngine;
using Cinemachine;
using BFM;

public class CameraManager : SingletonBehaviour<CameraManager>
{
    private CinemachineVirtualCamera virtualCamera;

    protected override void Awake()
    {
    }

    private void Start()
    {
        virtualCamera = transform.Find("Cinemachine").GetComponent<CinemachineVirtualCamera>();
        ConfinerSetting("Floor");
    }

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
        virtualCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D =
            GameObject.Find(mapName).GetComponent<CompositeCollider2D>();
    }
}
