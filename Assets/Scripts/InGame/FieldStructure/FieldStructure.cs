using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldStructure : MonoBehaviour
{
    [SerializeField] protected int structureId;

    protected float hp;

    protected FieldStructureData fieldStructureData;
    protected Collider2D top;
    protected Collider2D front;
    protected SoundRequester soundRequester;

    protected virtual void Awake()
    {
        SetFieldStructureData(structureId);
        FieldStructureInit();
        soundRequester = GetComponent<SoundRequester>();
        
        hp = 1;
    }

    private void OnEnable()
    {
        if (soundRequester != null)
        {
            soundRequester.ChangeSituation(SoundSituation.SOUNDSITUATION.SPAWN);
        }
    }

    private void SetFieldStructureData(int structureId)
    {
        Dictionary<string, object> table = CSVReader.Read("FieldStructureTable")[structureId.ToString()];

        fieldStructureData = new FieldStructureData();
        fieldStructureData.SetStructureId(structureId);
        fieldStructureData.SetStructureName(table["StructureName"].ToString());
        fieldStructureData.SetFrontPath(table["FrontPath"].ToString());
        fieldStructureData.SetTopPath(table["TopPath"].ToString());

        try
        {
            List<string> list = new List<string>();
            foreach (string str in (table["GimmickParam"] as List<string>))
            {
                list.Add(str);
            }
            fieldStructureData.SetGimmickParam(list);
        }
        catch
        {
            try
            {
                List<string> list = new List<string>()
                {
                    table["GimmickParam"].ToString(),
                };
                fieldStructureData.SetGimmickParam(list);
            }
            catch
            {
                fieldStructureData.SetGimmickParam(new List<string>());
            }
        }

        fieldStructureData.SetTopIsPassable(Convert.ToBoolean(table["TopIsPassable"].ToString().ToLower()));
        fieldStructureData.SetFrontIsPassable(Convert.ToBoolean(table["FrontIsPassable"].ToString().ToLower()));
        fieldStructureData.SetCastTime(Convert.ToInt32(table["CastTime"]) / 1000.0f);
        fieldStructureData.SetLayerOrder(Convert.ToInt32(table["LayerOrder"]));
        fieldStructureData.SetCoolTime(Convert.ToInt32(table["CoolTime"]) / 1000.0f);
        fieldStructureData.SetIsAct(Convert.ToBoolean(table["IsAct"].ToString().ToLower()));
    }

    private void FieldStructureInit()
    {
        SetLayer(transform);
        top = transform.Find("Top").GetComponent<Collider2D>();
        if (transform.Find("Top").TryGetComponent(out Collider2D col))
        {
            top = col;
            top.isTrigger = fieldStructureData.topIsPassable;
        }
        if (transform.Find("Front").TryGetComponent(out Collider2D col2))
        {
            front = col2;
            front.isTrigger = fieldStructureData.frontIsPassable;
        }
    }

    private void SetLayer(Transform trans)
    {
        trans.gameObject.layer = (int)LayerConstant.DECORATION;

        if (trans.TryGetComponent(out Renderer render))
        {
            render.sortingLayerName = ((LayerConstant)fieldStructureData.layerOrder).ToString();
        }
        else if (trans.TryGetComponent(out MeshRenderer meshRender))
        {
            meshRender.sortingLayerName = ((LayerConstant)fieldStructureData.layerOrder).ToString();
        }

        foreach (Transform child in trans)
        {
            SetLayer(child);
        }
    }

    public virtual void Remove()
    {
        StopAllCoroutines();
        Destroy(this.gameObject);
    }

    //{
    //    if (!front.enabled)
    //    {
    //        return;
    //    }

    //    if (collision.gameObject.layer == (int)LayerConstant.SKILL)
    //    {
    //        StartCoroutine(Activation());
    //        Gimmick.GimmickActivate(transform, this.fieldStructureData.gimmick, this.fieldStructureData.gimmickParam);
    //    }
    //}

    //protected abstract IEnumerator Activation();
    //{
    //    front.enabled = false;
    //    yield return new WaitForSeconds(this.fieldStructureData.coolTime);
    //    front.enabled = true;
    //}
}
