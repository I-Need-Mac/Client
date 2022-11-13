using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTopImage : MonoBehaviour
{
    private string structureName;

    private Dictionary<string, Dictionary<string, object>> fieldStructureData = new Dictionary<string, Dictionary<string, object>>();

    void Start()
    {
        structureName = this.gameObject.name;

        fieldStructureData = CSVReader.Read("FieldStructureTable");

        string topPath;
        topPath = fieldStructureData[structureName]["TopPath"].ToString();

        DebugManager.Instance.PrintDebug("[LoadImage]" + structureName + topPath);

        SpriteRenderer spriteR = gameObject.GetComponent<SpriteRenderer>();
        Sprite sprites = ImageLoader.Instance.LoadLocalImageToSprite(topPath);
        spriteR.sprite = sprites;
    }
}
