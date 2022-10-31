using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ImageLoader : SingleTon<ImageLoader>
{
    private Dictionary<string, Texture2D> cachedImageTexture2D = new Dictionary<string, Texture2D>();
    private Dictionary<string, Sprite> cachedImageSprite = new Dictionary<string, Sprite>();

    public ImageLoader()
    {
        cachedImageTexture2D.Add("Dummy", Resources.Load("Art/Dummy", typeof(Texture2D)) as Texture2D);
        cachedImageSprite.Add("Dummy", Resources.Load("Art/Dummy", typeof(Sprite)) as Sprite);
    }



    public Texture2D LoadLocalImageToTexture2D(string path)
    {
        Texture2D return2D;
        if (cachedImageTexture2D.ContainsKey(path))
        {
            DebugManager.Instance.PrintDebug("[ImageLoader] 캐싱된 이미지 : " + path);
            return2D = cachedImageTexture2D[path];
        }
        else
        {
            DebugManager.Instance.PrintDebug("[ImageLoader] 신규 이미지 : " + path);

            return2D = Resources.Load(path, typeof(Texture2D)) as Texture2D;
            if (return2D == null)
            {
                DebugManager.Instance.PrintDebug("[ImageLoader] Dummy 이미지 : 경로 이상");
                return2D = cachedImageTexture2D["Dummy"];
            }
            else { cachedImageTexture2D.Add(path, return2D); }

        }
        return return2D;
    }

    public Sprite LoadLocalImageToSprite(string path)
    {
        Sprite returnSprite;
        if (cachedImageSprite.ContainsKey(path))
        {
            DebugManager.Instance.PrintDebug("[ImageLoader] 캐싱된 이미지 : " + path);
            returnSprite = cachedImageSprite[path];
        }
        else
        {
            DebugManager.Instance.PrintDebug("[ImageLoader] 신규 이미지 : " + path);

            returnSprite = Resources.Load(path, typeof(Sprite)) as Sprite;
            if (returnSprite == null)
            {
                DebugManager.Instance.PrintDebug("[ImageLoader] Dummy 이미지 : 경로 이상");
                returnSprite = cachedImageSprite["Dummy"];
            }
            else
            {
                cachedImageSprite.Add(path, returnSprite);
            }
        }
        return returnSprite;
    }
}
