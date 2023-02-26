using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemResourceLoader
{
    private static Dictionary<string, Sprite> itemResources = new Dictionary<string, Sprite>();

    public static Sprite Load(string path)
    {
        Sprite resource = null;

        if (itemResources.ContainsKey(path))
        {
            resource = itemResources[path];
        }
        else
        {
            resource = Resources.Load<Sprite>(path);
            itemResources.Add(path, resource);
            
        }
        
        return resource;
    }
}
