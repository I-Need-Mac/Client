using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    //�� ũ�� ����
    public void SetMapSize(int width, int height)
    {
        this.transform.position = Vector2.zero;

        transform.localScale = new Vector2(width, height);
    }
}
