using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 페이지 기능
public class UI_Page : UI_Popup
{
    public enum PageTableInfo
    {
        PageType,
        TextBox1,
        TextBox2,
        TextBox3,
        VoicePath1,
        VoicePath2,
        VoicePath3,
        ImagePath,
    }

    public enum PageType
    {
        Text,
        Picture,
        PictureTextTop,
        PictureTextBottom,
    }

    enum Buttons
    {
    }

    enum Images
    {
    }

    enum Texts
    {
    }

    [System.Serializable]
    struct TextType
    {
        [SerializeField]
        public Text topText;
        [SerializeField]
        public Text middleText;
        [SerializeField]
        public Text bottomText;
    }

    [System.Serializable]
    struct PictureTextType
    {
        [SerializeField]
        public Text text;
        [SerializeField]
        public Image picture;
    }

    [SerializeField]
    TextType text;

    [SerializeField]
    Image picture;

    [SerializeField]
    PictureTextType pictureTextTop;
    [SerializeField]
    PictureTextType pictureTextBottom;

    PageType type = PageType.Text;
    public PageType TYPE { get { return type; } }

    Queue<string> textTypeScript = new Queue<string>();

    public void SetData(int id, List<object> data)
    {
        // 페이지 타입별로 페이지 구성을 해줍니다.
        string typeName = data[(int)PageTableInfo.PageType].ToString();
        string[] pageTypes = Enum.GetNames(typeof(PageType));

        // 모든 타입을 꺼줍니다.
        for (int i = 0; i < pageTypes.Length; i++)
        {
            GameObject go = Util.FindChild(this.gameObject, pageTypes[i]);
            if (go == null)
                continue;

            go.SetActive(false);
        }

        // 해당하는 타입만 활성 합니다.
        GameObject activeObject = Util.FindChild(this.gameObject, typeName);
        if (activeObject)
            activeObject.SetActive(true);

        // 타입에 맞는 데이터를 셋팅합니다.
        if (typeName == pageTypes[(int)PageType.Text])
        {   // 텍스트 타입
            type = PageType.Text;

            textTypeScript.Enqueue(data[(int)PageTableInfo.TextBox1].ToString());
            textTypeScript.Enqueue(data[(int)PageTableInfo.TextBox2].ToString());
            textTypeScript.Enqueue(data[(int)PageTableInfo.TextBox3].ToString());

            text.topText.text = null;
            text.middleText.text = null;
            text.bottomText.text = null;
        }
        else if (typeName == pageTypes[(int)PageType.Picture])
        {   // 이미지 타입
            type = PageType.Picture;

            string path = $"{Define.UiImagePath}" + data[(int)PageTableInfo.ImagePath].ToString();
            picture.sprite = Resources.Load<Sprite>(path);
        }
        else if (typeName == pageTypes[(int)PageType.PictureTextTop])
        {   // 텍스트-이미지 타입
            type = PageType.PictureTextTop;

            textTypeScript.Enqueue(data[(int)PageTableInfo.TextBox1].ToString());
            //pictureTextTop.text.text = data[(int)PageTableInfo.TextBox1].ToString();
            
            string path = $"{Define.UiImagePath}" + data[(int)PageTableInfo.ImagePath].ToString();
            pictureTextTop.picture.sprite = Resources.Load<Sprite>(path);
        }
        else if (typeName == pageTypes[(int)PageType.PictureTextBottom])
        {   // 이미지-텍스트 타입
            type = PageType.PictureTextBottom;

            textTypeScript.Enqueue(data[(int)PageTableInfo.TextBox1].ToString());
            //pictureTextBottom.text.text = data[(int)PageTableInfo.TextBox1].ToString();

            string path = $"{Define.UiImagePath}" + data[(int)PageTableInfo.ImagePath].ToString();
            pictureTextBottom.picture.sprite = Resources.Load<Sprite>(path);
        }
        else
        {

        }
    }

    public void ClearPage()
    {
        text.topText.text = null;
        text.middleText.text = null;
        text.bottomText.text = null;

        pictureTextTop.text.text = null;
        pictureTextBottom.text.text = null;
    }

    public void ActivePage()
    {
        ClearPage();

        switch (type)
        {
            case PageType.Text:
                text.topText.TypeText(textTypeScript.Peek(), onComplete:
                    () => text.middleText.TypeText(textTypeScript.Peek(), onComplete:
                    () => text.bottomText.TypeText(textTypeScript.Peek(), onComplete: () => Debug.Log("Text Complete"))));
                break;
            case PageType.Picture:
                break;
            case PageType.PictureTextTop:
                pictureTextTop.text.TypeText(textTypeScript.Peek(), onComplete: () => Debug.Log("PictureTextTop Text Complete"));
                break;
            case PageType.PictureTextBottom:
                pictureTextBottom.text.TypeText(textTypeScript.Peek(), onComplete: () => Debug.Log("PictureTextBottom Text Complete"));
                break;
            default:
                break;
        }
    }

    public void SkipPage(out bool isLast, bool isPageSkip = false)
    {
        isLast = false;

        switch (type)
        {
            case PageType.Text:
                if (isPageSkip)
                {   // 한 쪽을 스킵합니다.
                    text.topText.SkipTypeText();
                    text.middleText.SkipTypeText();
                    text.bottomText.SkipTypeText();
                }
                else
                {   // 한 문단을 스킵합니다.
                    if (text.topText.IsSkippable())
                    {
                        text.topText.SkipTypeText();
                    }
                    else if (text.middleText.IsSkippable())
                    {
                        text.middleText.SkipTypeText();
                    }
                    else if (text.bottomText.IsSkippable())
                    {
                        text.bottomText.SkipTypeText();
                        isLast = true;
                    }
                }
                break;
            case PageType.Picture:
                isLast = true;
                break;
            case PageType.PictureTextTop:
                pictureTextTop.text.SkipTypeText();
                isLast = true;
                break;
            case PageType.PictureTextBottom:
                pictureTextBottom.text.SkipTypeText();
                isLast = true;
                break;
            default:
                break;
        }
    }

    public bool IsPageSkippable()
    {
        switch (type)
        {
            case PageType.Text:
                // 스킵할게 없습니다.
                if (!text.topText.IsSkippable() && !text.middleText.IsSkippable() 
                    && !text.bottomText.IsSkippable())
                    return false;

                break;
            case PageType.Picture:
                break;
            case PageType.PictureTextTop:
                if (!pictureTextTop.text.IsSkippable())
                    return false;
                
                break;
            case PageType.PictureTextBottom:
                if (!pictureTextBottom.text.IsSkippable())
                    return false;

                break;
            default:
                break;
        }

        return true;
    }
}
