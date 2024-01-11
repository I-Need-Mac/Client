using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadFrontalImage : MonoBehaviour
{
    private string structureName;

    private Dictionary<string, Dictionary<string, object>> fieldStructureData = new Dictionary<string, Dictionary<string, object>>();

    void Start()
    {
        structureName = this.gameObject.name;

        fieldStructureData = CSVReader.Read("FieldStructureTable");

        string frontalPath;
        frontalPath = fieldStructureData[structureName]["FrontalPath"].ToString();

        DebugManager.Instance.PrintDebug("[LoadImage]" + structureName + frontalPath);

        SpriteRenderer spriteR = gameObject.GetComponent<SpriteRenderer>();
        // Sprite[] sprites = Resources.LoadAll<Sprite>("Art/" + frontalPath);
        // spriteR.sprite = sprites[0];
        Sprite sprites = ImageLoader.Instance.LoadLocalImageToSprite(frontalPath);
        spriteR.sprite = sprites;
    }
}
