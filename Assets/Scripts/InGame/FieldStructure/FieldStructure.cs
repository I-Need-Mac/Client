using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldStructure : MonoBehaviour
{
    [SerializeField] private int structureId;

    private FieldStructureData fieldStructureData;
    private Transform top;
    private Transform front;

    private void Awake()
    {
        SetFieldStructureData(structureId);
        FieldStructureInit();
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
            List<GimmickEnum> list = new List<GimmickEnum>();
            foreach (string str in (table["Gimmick"] as List<string>))
            {
                list.Add((GimmickEnum)Enum.Parse(typeof(GimmickEnum), str, true));
            }
            fieldStructureData.SetGimmick(list);
        }
        catch
        {
            try
            {
                List<GimmickEnum> list = new List<GimmickEnum>()
                {
                    (GimmickEnum)Enum.Parse(typeof(GimmickEnum), table["Gimmick"].ToString(), true),
                };
                fieldStructureData.SetGimmick(list);
            }
            catch
            {
                fieldStructureData.SetGimmick(new List<GimmickEnum>());
            }
        }

        try
        {
            List<float> list = new List<float>();
            foreach (string str in (table["GimmickParam"] as List<string>))
            {
                list.Add(float.Parse(str));
            }
            fieldStructureData.SetGimmickParam(list);
        }
        catch
        {
            try
            {
                List<float> list = new List<float>()
                {
                    float.Parse(table["GimmickParam"].ToString()),
                };
                fieldStructureData.SetGimmickParam(list);
            }
            catch
            {
                fieldStructureData.SetGimmickParam(new List<float>());
            }
        }

        fieldStructureData.SetTopIsPassable(Convert.ToBoolean(table["TopIsPassible"].ToString().ToLower()));
        fieldStructureData.SetFrontIsPassable(Convert.ToBoolean(table["FrontIsPassible"].ToString().ToLower()));
        fieldStructureData.SetCastTime(Convert.ToInt32(table["CastTime"]) / 1000.0f);
        fieldStructureData.SetLayerOrder(Convert.ToInt32(table["LayerOrder"]));
        fieldStructureData.SetCoolTime(Convert.ToInt32(table["CoolTime"]) / 1000.0f);
        fieldStructureData.SetIsAct(Convert.ToBoolean(table["IsAct"].ToString().ToLower()));
    }

    private void FieldStructureInit()
    {
        SetLayer(transform);
        top = transform.Find("Top");
        front = transform.Find("Front");

        top.GetComponent<SpriteRenderer>().sprite = ResourcesManager.Load<Sprite>(fieldStructureData.topPath);
        top.GetComponent<Collider2D>().enabled = fieldStructureData.topIsPassable;
        front.GetComponent<SpriteRenderer>().sprite = ResourcesManager.Load<Sprite>(fieldStructureData.frontPath);
        front.GetComponent<Collider2D>().enabled = fieldStructureData.frontIsPassable;
    }

    private void SetLayer(Transform trans)
    {
        trans.gameObject.layer = fieldStructureData.layerOrder;
        foreach (Transform child in trans)
        {
            SetLayer(child);
        }
    }
}
