using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    //¸Ê Å©±â ¼³Á¤
    public void SetMapSize(int width, int height)
    {
        transform.localScale = new Vector2(width, height);

        transform.position = Vector2.zero;
    }
}
