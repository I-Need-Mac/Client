using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GimmickType     // 게임 속 버튼의 집합을 만들어준다.
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
        int gimmickParameter;
        bool isPassable = false;
        string _isPassable;
        int castTime;
        int layerOrder;

        structureName = fieldStructureData[structureID]["StructureName"].ToString();
        frontalPath = fieldStructureData[structureID]["FrontalPath"].ToString();
        topPath = fieldStructureData[structureID]["TopPath"].ToString();
        currentType = (GimmickType)System.Enum.Parse(typeof(GimmickType),fieldStructureData[structureID]["Gimmick"].ToString());
        // currentType = (GimmickType)fieldStructureData[structureID]["Gimmick"];
        gimmickParameter = int.Parse(fieldStructureData[structureID]["GimmickParam"].ToString());
        _isPassable = fieldStructureData[structureID]["IsPassable"].ToString();
        if(_isPassable == "TRUE") isPassable = true;
        else if(_isPassable == "FALSE") isPassable = false;
        castTime = int.Parse(fieldStructureData[structureID]["CastTime"].ToString());
        layerOrder = int.Parse(fieldStructureData[structureID]["LayerOrder"].ToString());


        GameObject front = transform.GetChild(0).gameObject;
        GameObject top = transform.GetChild(1).gameObject;



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
            // Sprite sprites = ImageLoader.Instance.LoadLocalImageToSprite(frontalPath);
            // spriteR.sprite = sprites;

        }

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
            // Sprite sprites = ImageLoader.Instance.LoadLocalImageToSprite(topPath);
            // spriteR.sprite = sprites;

        }
        
        if(frontalPath != "Null" && topPath != "Null")
        {
            top.transform.position = new Vector3(0, (topHeight + frontHeight) / 2, 0);
        }
        
        if(!isPassable)
        {
            front.AddComponent<BoxCollider2D>();
        }
        


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

        this.gameObject.layer = layerOrder;



        /*
        string frontalPath;
        frontalPath = fieldStructureData[structureName]["FrontalPath"].ToString();

        DebugManager.Instance.PrintDebug("[LoadImage]" + structureName + frontalPath);

        SpriteRenderer spriteR = gameObject.GetComponent<SpriteRenderer>();
        // Sprite[] sprites = Resources.LoadAll<Sprite>("Art/" + frontalPath);
        // spriteR.sprite = sprites[0];
        Sprite sprites = ImageLoader.Instance.LoadLocalImageToSprite(frontalPath);
        spriteR.sprite = sprites;
        */
    }
}
