using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<Dictionary<string, object>> test = CSVReader.Read("CharacterStat");
        Dictionary<int, List<object>> csvRead = CSVReader.FileRead("CharacterStat");

        if (test != null)
        {
            for (int i = 0; i < test.Count; i++)
            {
                Debug.LogFormat("=============================================");
                Debug.LogFormat("CharacterId : {0}", test[i]["CharacterId"]);
                Debug.LogFormat("Character_Name : {0}", test[i]["Character_Name"]);
                Debug.LogFormat("CriRatio : {0}", test[i]["CriRatio"]);
                Debug.LogFormat("=============================================");
            }
        }
    }
}
