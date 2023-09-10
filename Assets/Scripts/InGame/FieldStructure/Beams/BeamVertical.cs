using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamVertical : Beam
{
    public override void BeamActivation(float n, float m)
    {
        transform.SetParent(GameManager.Instance.player.transform);
        transform.localScale = new Vector2(m, n);
        transform.localPosition = new Vector3(0.0f, -0.2f, transform.localPosition.z);
        StartCoroutine(Run());
    }
}
