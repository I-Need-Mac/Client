using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Beam : MonoBehaviour
{
    [SerializeField] protected float duration = 1.0f;

    public abstract void BeamActivation(float n, float m);

    private void Awake()
    {
        gameObject.layer = (int)LayerConstant.SKILL;
    }

    public IEnumerator Run()
    {
        yield return new WaitForSeconds(this.duration);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Monster monster))
        {
            monster.Die(true);
        }
    }
}
