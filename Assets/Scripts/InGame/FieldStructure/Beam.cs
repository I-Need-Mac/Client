using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    [SerializeField] private float beamDuration = 1.0f;

    public void BeamInit(float h, float v)
    {
        this.transform.localScale = new Vector2(h, v);
        StartCoroutine(Activation());
    }

    private IEnumerator Activation()
    {
        yield return new WaitForSeconds(beamDuration);
        Destroy(this.gameObject);
    }
}
