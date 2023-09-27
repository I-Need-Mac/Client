using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructionstone : FieldStructure
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!front.enabled)
        {
            return;
        }

        if (collision.gameObject.layer == (int)LayerConstant.SKILL)
        {
            StartCoroutine(Activation());
            Gimmick.GimmickActivate(transform, this.fieldStructureData.gimmick, this.fieldStructureData.gimmickParam);
        }
    }

    protected override IEnumerator Activation()
    {
        SpriteRenderer sprite = front.GetComponent<SpriteRenderer>();
        front.enabled = false;
        sprite.enabled = false;
        yield return new WaitForSeconds(this.fieldStructureData.coolTime);
        sprite.enabled = true;
        front.enabled = true;
    }
}
