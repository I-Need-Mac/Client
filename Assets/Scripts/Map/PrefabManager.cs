using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum GimmickType
{
    Tile,
    Obstacle,
    Ornament,
    Damage,
    Durability,
    Teleport,
    Heal,
    BeamWidth,
    BeamVeritcal,
    BeamCircle,
    BeamCross,
    SurpriseBox,
    Warning,
    Punishment,
    DebuffSpeed
}


public class PrefabManager : MonoBehaviour
{
    private string structureID;

    private Dictionary<string, Dictionary<string, object>> fieldStructureData = new Dictionary<string, Dictionary<string, object>>();

    void Start()
    {
        structureID = this.gameObject.name;

        fieldStructureData = CSVReader.Read("FieldStructureTable");


        string structureName;
        string frontalPath;
        string topPath;
        GimmickType currentType;
        // var _gimmickParameter;
        // string[] gimmickParameterSplit;
        int[] gimmickParameter = new int[10];
        bool topIsPassable = false;
        string _topIsPassable;
        bool frontIsPassable = false;
        string _frontIsPassable;
        int castTime;
        int layerOrder;


        // StructureName, FrontalPath, TopPath, GimmickType 받아오기
        structureName = fieldStructureData[structureID]["StructureName"].ToString();
        frontalPath = fieldStructureData[structureID]["FrontalPath"].ToString();
        topPath = fieldStructureData[structureID]["TopPath"].ToString();
        currentType = (GimmickType)System.Enum.Parse(typeof(GimmickType),fieldStructureData[structureID]["Gimmick"].ToString());


        // GimmickParameter 배열로 받아오기
        try
        {
            var _gimmickParameter = (List<string>)fieldStructureData[structureID]["GimmickParam"];
            // gimmickParameterSplit = _gimmickParameter.Split(',');

            int i = 0;
            foreach (string s in _gimmickParameter)
            {
                gimmickParameter[i] = int.Parse(s);
                i++;
            }
        }
        catch (InvalidCastException e)
        {
            gimmickParameter[0] = int.Parse(fieldStructureData[structureID]["GimmickParam"].ToString());
        }

        /*
        for (int i = 0; i < gimmickParameterSplit.Length; i++)
        {
            gimmickParameterSplit[i] = gimmickParameterSplit[i].Replace("\"", "");
            gimmickParameter[i] = int.Parse(gimmickParameterSplit[i]);
        }
        */
        

        // isPassable 받아오기
        _topIsPassable = fieldStructureData[structureID]["TopIsPassible"].ToString();
        if (_topIsPassable == "TRUE")
        {
            topIsPassable = true;
        }
        else if (_topIsPassable == "FALSE") 
        {
            topIsPassable = false;
        }

        _frontIsPassable = fieldStructureData[structureID]["FrontIsPassable"].ToString();
        if (_frontIsPassable == "TRUE")
        {
            frontIsPassable = true;
        }
        else if (_frontIsPassable == "FALSE")
        {
            frontIsPassable = false;
        }

        // CastTime, LayerOrder int형으로 받아오기
        castTime = int.Parse(fieldStructureData[structureID]["CastTime"].ToString());
        layerOrder = int.Parse(fieldStructureData[structureID]["LayerOrder"].ToString());

        // 프리팹의 Front, Top 오브젝트 받아오기
        GameObject front = transform.GetChild(0).gameObject;
        GameObject top = transform.GetChild(1).gameObject;

        // 오브젝트 레이어 설정
        this.gameObject.layer = layerOrder;
        front.gameObject.layer = layerOrder;
        if (topPath != "Null")
        {
            top.gameObject.layer = layerOrder - 2;
        }



        float topHeight = 0f;
        float frontHeight = 0f;

        
        // Front 이미지 삽입 및 높이 받기
        if (frontalPath == "Null")
        {
            Destroy(front);
        }
        else
        {
            SpriteRenderer frontSpriteR = front.GetComponent<SpriteRenderer>();
            DebugManager.Instance.PrintDebug("Arts/" + frontalPath);
            Sprite[] frontSprites = Resources.LoadAll<Sprite>("Arts/" + frontalPath);
            frontSpriteR.sprite = frontSprites[0];
            frontHeight = frontSprites[0].bounds.size.y;
            // front.transform.position = new Vector3(0, 0, 0);
            // Sprite sprites = ImageLoader.Instance.LoadLocalImageToSprite(frontalPath);
            // spriteR.sprite = sprites;

        }

        // Top 이미지 삽입 및 높이 받기
        if (topPath == "Null")
        {
            Destroy(top);
        }
        else
        {
            SpriteRenderer topSpriteR = top.GetComponent<SpriteRenderer>();
            DebugManager.Instance.PrintDebug("Arts/" + topPath);
            Sprite[] topSprites = Resources.LoadAll<Sprite>("Arts/" + topPath);
            topSpriteR.sprite = topSprites[0];
            topHeight = topSprites[0].bounds.size.y;
            // top.transform.position = new Vector3(0, 0, 0);
            // Sprite sprites = ImageLoader.Instance.LoadLocalImageToSprite(topPath);
            // spriteR.sprite = sprites;

        }
        
        // Top 오브젝트를 Front 바로 위에 붙이기
        if(topPath != "Null" && currentType != GimmickType.Teleport)
        {
            top.transform.localPosition = new Vector3(0, (topHeight + frontHeight) / 2, 0);
        }
        


        // 통행 불가 시 콜라이더 설정
        if (!frontIsPassable)
        {
            front.AddComponent<BoxCollider2D>();
        }
        if (!topIsPassable && topPath != "Null")
        {
            top.AddComponent<BoxCollider2D>();
        }


        // GimmickType 별 기능 수행
        switch (currentType)
        {
            case GimmickType.Tile:

                break;
            case GimmickType.Obstacle:

                break;
            case GimmickType.Ornament:

                break;
            case GimmickType.Damage:

                break;
            case GimmickType.Durability:

                break;
            case GimmickType.Teleport:
                // front 설정
                front.AddComponent<BoxCollider2D>();
                BoxCollider2D colliderFront = front.GetComponent<BoxCollider2D>();
                colliderFront.isTrigger = true;
                front.AddComponent<Teleport>();
                Teleport scriptFront = front.GetComponent<Teleport>();
                scriptFront.telleportTo = top;
                // top 설정
                top.AddComponent<BoxCollider2D>();
                BoxCollider2D colliderTop = top.GetComponent<BoxCollider2D>();
                colliderTop.isTrigger = true;
                top.AddComponent<Teleport>();
                Teleport scriptTop = top.GetComponent<Teleport>();
                scriptTop.telleportTo = front;

                break;
            case GimmickType.Heal:

                break;
            case GimmickType.BeamWidth:

                break;
            case GimmickType.BeamVeritcal:

                break;
            case GimmickType.BeamCircle:

                break;
            case GimmickType.BeamCross:

                break;
            case GimmickType.SurpriseBox:

                break;
            case GimmickType.Warning:

                break;
            case GimmickType.Punishment:

                break;
            case GimmickType.DebuffSpeed:

                break;

        }

    }
}
