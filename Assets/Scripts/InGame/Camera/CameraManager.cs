using UnityEngine;
using Cinemachine;
using BFM;

public class CameraManager : SingletonBehaviour<CameraManager>
{
    private CinemachineVirtualCamera virtualCamera;

    protected override void Awake()
    {
        virtualCamera = transform.Find("Cinemachine").GetComponent<CinemachineVirtualCamera>();
        ConfinerSetting("Floors");
    }

    private void ConfinerSetting(string mapName)
    {
        virtualCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D =
            GameObject.Find(mapName).GetComponent<CompositeCollider2D>();
    }
}
