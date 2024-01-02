using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotateImage : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public float rotationSpeed = 90.0f; // 초당 회전 각도
    [SerializeField]
    public RectTransform image;
    void Update()
    {
        // 오브젝트를 Y축을 기준으로 회전시킵니다.
        // 회전 속도를 rotationSpeed로 설정합니다.
        image.Rotate(Vector3.left * rotationSpeed);
    }
}
