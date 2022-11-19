using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ������ ���
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
        // ������ Ÿ�Ժ��� ������ ������ ���ݴϴ�.
        string typeName = data[(int)PageTableInfo.PageType].ToString();
        string[] pageTypes = Enum.GetNames(typeof(PageType));

        // ��� Ÿ���� ���ݴϴ�.
        for (int i = 0; i < pageTypes.Length; i++)
        {
            GameObject go = Util.FindChild(this.gameObject, pageTypes[i]);
            if (go == null)
                continue;

            go.SetActive(false);
        }

        // �ش��ϴ� Ÿ�Ը� Ȱ�� �մϴ�.
        GameObject activeObject = Util.FindChild(this.gameObject, typeName);
        if (activeObject)
            activeObject.SetActive(true);

        // Ÿ�Կ� �´� �����͸� �����մϴ�.
        if (typeName == pageTypes[(int)PageType.Text])
        {   // �ؽ�Ʈ Ÿ��
            type = PageType.Text;

            textTypeScript.Enqueue(data[(int)PageTableInfo.TextBox1].ToString());
            textTypeScript.Enqueue(data[(int)PageTableInfo.TextBox2].ToString());
            textTypeScript.Enqueue(data[(int)PageTableInfo.TextBox3].ToString());

            text.topText.text = null;
            text.middleText.text = null;
            text.bottomText.text = null;
        }
        else if (typeName == pageTypes[(int)PageType.Picture])
        {   // �̹��� Ÿ��
            type = PageType.Picture;

            string path = $"{Define.UiImagePath}" + data[(int)PageTableInfo.ImagePath].ToString();
            picture.sprite = Resources.Load<Sprite>(path);
        }
        else if (typeName == pageTypes[(int)PageType.PictureTextTop])
        {   // �ؽ�Ʈ-�̹��� Ÿ��
            type = PageType.PictureTextTop;

            textTypeScript.Enqueue(data[(int)PageTableInfo.TextBox1].ToString());
            //pictureTextTop.text.text = data[(int)PageTableInfo.TextBox1].ToString();
            
            string path = $"{Define.UiImagePath}" + data[(int)PageTableInfo.ImagePath].ToString();
            pictureTextTop.picture.sprite = Resources.Load<Sprite>(path);
        }
        else if (typeName == pageTypes[(int)PageType.PictureTextBottom])
        {   // �̹���-�ؽ�Ʈ Ÿ��
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
                {   // �� ���� ��ŵ�մϴ�.
                    text.topText.SkipTypeText();
                    text.middleText.SkipTypeText();
                    text.bottomText.SkipTypeText();
                }
                else
                {   // �� ������ ��ŵ�մϴ�.
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
                // ��ŵ�Ұ� �����ϴ�.
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
