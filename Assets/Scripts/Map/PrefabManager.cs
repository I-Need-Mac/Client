using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        string _gimmickParameter;
        string[] gimmickParameterSplit;
        int[] gimmickParameter = new int[10];
        bool isPassable = false;
        string _isPassable;
        int castTime;
        int layerOrder;


        // StructureName, FrontalPath, TopPath, GimmickType 받아오기
        structureName = fieldStructureData[structureID]["StructureName"].ToString();
        frontalPath = fieldStructureData[structureID]["FrontalPath"].ToString();
        topPath = fieldStructureData[structureID]["TopPath"].ToString();
        currentType = (GimmickType)System.Enum.Parse(typeof(GimmickType),fieldStructureData[structureID]["Gimmick"].ToString());


        // GimmickParameter 배열로 받아오기
        _gimmickParameter = fieldStructureData[structureID]["GimmickParam"].ToString();
        gimmickParameterSplit = _gimmickParameter.Split(',');
        for (int i = 0; i < gimmickParameterSplit.Length; i++)
        {
            gimmickParameterSplit[i] = gimmickParameterSplit[i].Replace("\"", "");
            gimmickParameter[i] = int.Parse(gimmickParameterSplit[i]);
        }


        // isPassable 받아오기
        _isPassable = fieldStructureData[structureID]["IsPassable"].ToString();
        if (_isPassable == "TRUE")
        {
            isPassable = true;
        }
        else if (_isPassable == "FALSE") 
        {
            isPassable = false;
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
        top.gameObject.layer = layerOrder;

        /*
        if (frontalPath != "Null" && topPath != "Null")
        {
            float topHeight = 0f;
            Sprite[] topSprites = Resources.LoadAll<Sprite>("Arts/" + topPath);
            topHeight = topSprites[0].bounds.size.y;
            float frontHeight = 0f;
            Sprite[] frontSprites = Resources.LoadAll<Sprite>("Arts/" + frontalPath);
            frontHeight = frontSprites[0].bounds.size.y;

            top.transform.position = new Vector3(0, (topHeight+frontHeight) / 2, 0);
        }
        */
        
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
        if(frontalPath != "Null" && topPath != "Null")
        {
            top.transform.localPosition = new Vector3(0, (topHeight + frontHeight) / 2, 0);
        }
        
        // 통행 불가 시 콜라이더 설정
        if(!isPassable)
        {
            front.AddComponent<BoxCollider2D>();
        }
        
        
        // GimmickType 별 기능 수행
        switch(currentType)
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
