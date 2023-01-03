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

    private void ConfinerSetting(string mapName)
    {
        DebugManager.Instance.PrintDebug("test: "+GameObject.Find(mapName));
        virtualCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D =
            GameObject.Find(mapName).GetComponent<CompositeCollider2D>();
    }
}
