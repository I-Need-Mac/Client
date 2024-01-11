using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        public TextMeshProUGUI topText;
        [SerializeField]
        public TextMeshProUGUI middleText;
        [SerializeField]
        public TextMeshProUGUI bottomText;
    }

    [System.Serializable]
    struct PictureTextType
    {
        [SerializeField]
        public TextMeshProUGUI text;
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
    
    public bool isFinished =false;
    public PageType TYPE { get { return type; } }


    private FadeInImage topFade;
    private FadeInImage bottomFade;
    private TypeMeshProComponent topText;
    private TypeMeshProComponent middleText;
    private TypeMeshProComponent bottomText;
    private TypeMeshProComponent pictureTopText;
    private TypeMeshProComponent pictureBottomText;

    List<string> textTypeScript = new List<string>();
    UI_StoryBook  storyBook;

    public void SetData(int id, List<object> data, UI_StoryBook parent)
    {
        storyBook = parent;
        topFade = pictureTextTop.picture.GetComponent<FadeInImage>();
        bottomFade = pictureTextBottom.picture.GetComponent<FadeInImage>();

        topText = text.topText.GetComponent<TypeMeshProComponent>();
        middleText = text.middleText.GetComponent<TypeMeshProComponent>();
        bottomText = text.bottomText.GetComponent<TypeMeshProComponent>();
        pictureTopText = pictureTextTop.text.GetComponent<TypeMeshProComponent>();
        pictureBottomText = pictureTextBottom.text.GetComponent<TypeMeshProComponent>();

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

            textTypeScript.Add(LocalizeManager.Instance.GetText(data[(int)PageTableInfo.TextBox1].ToString()));
            textTypeScript.Add(LocalizeManager.Instance.GetText(data[(int)PageTableInfo.TextBox2].ToString()));
            textTypeScript.Add(LocalizeManager.Instance.GetText(data[(int)PageTableInfo.TextBox3].ToString()));

            topText.SetTextToType(textTypeScript[0]);
            middleText.SetTextToType(textTypeScript[1]);
            bottomText.SetTextToType(textTypeScript[2]);

            text.topText.text = null;
            text.middleText.text = null;
            text.bottomText.text = null;
        }
        else if (typeName == pageTypes[(int)PageType.Picture])
        {   // 이미지 타입
            type = PageType.Picture;
            string path = $"{Define.UiCharacterPath}/" + data[(int)PageTableInfo.ImagePath].ToString();
            picture.sprite = Resources.Load<Sprite>(path);
        }
        else if (typeName == pageTypes[(int)PageType.PictureTextTop])
        {   // 텍스트-이미지 타입
            type = PageType.PictureTextTop;

            textTypeScript.Add(LocalizeManager.Instance.GetText(data[(int)PageTableInfo.TextBox1].ToString()));
            //pictureTextTop.text.text = data[(int)PageTableInfo.TextBox1].ToString();
            pictureTopText.SetTextToType(textTypeScript[0]);

            string path = data[(int)PageTableInfo.ImagePath].ToString();
            pictureTextTop.picture.sprite = ResourcesManager.Load<Sprite>(path);
        }
        else if (typeName == pageTypes[(int)PageType.PictureTextBottom])
        {   // 이미지-텍스트 타입
            type = PageType.PictureTextBottom;

            textTypeScript.Add(LocalizeManager.Instance.GetText(data[(int)PageTableInfo.TextBox1].ToString()));
            //pictureTextBottom.text.text = data[(int)PageTableInfo.TextBox1].ToString();
            pictureBottomText.SetTextToType(textTypeScript[0]);

            string path = data[(int)PageTableInfo.ImagePath].ToString();
            pictureTextBottom.picture.sprite = ResourcesManager.Load<Sprite>(path);
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

        pictureTextTop.picture.GetComponent<FadeInImage>().SetFadeOut();
        pictureTextBottom.picture.GetComponent<FadeInImage>().SetFadeOut();
    }

    public void ActivePage()
    {
        ClearPage();

        switch (type)
        {
            case PageType.Text:
                topText.TypeText(textTypeScript[0], onComplete:
                    () => middleText.TypeText(textTypeScript[1], onComplete:
                    () => bottomText.TypeText(textTypeScript[2], onComplete: () => storyBook.NextPage())));
                isFinished = true;
                Debug.Log("Text Complete");
                break;
            case PageType.Picture:
                break;
            case PageType.PictureTextTop:

                pictureTopText.TypeText(textTypeScript[0], onComplete: 
                    () => topFade.StartFadeIn(onComplete: 
                    () =>storyBook.NextPage()));
                isFinished = true;
                Debug.Log("PictureTextTop Text Complete");
                break;
            case PageType.PictureTextBottom:
                bottomFade.StartFadeIn(onComplete:
                    ()=>pictureBottomText.TypeText(textTypeScript[0], onComplete:
                    () => storyBook.NextPage()));
                isFinished = true;
                Debug.Log("PictureTextBottom Text Complete");
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
                    topText.SkipTypeText();
                    middleText.SkipTypeText();
                    bottomText.SkipTypeText();

                    topText.ShowText();
                    middleText.ShowText();
                    bottomText.ShowText();

                    
                }
                else
                {   // 한 문단을 스킵합니다.
                    if (topText.IsSkippable())
                    {
                        topText.SkipTypeText();
                        topText.ShowText();
                    }
                    else if (middleText.IsSkippable())
                    {
                        middleText.SkipTypeText();
                        middleText.ShowText();
                    }
                    else if (bottomText.IsSkippable())
                    {
                        bottomText.SkipTypeText();
                        bottomText.ShowText();
                        isLast = true;
                    }
                }
                break;
            case PageType.Picture:
                isLast = true;
                break;
            case PageType.PictureTextTop:
                pictureTopText.SkipTypeText();
                pictureTopText.ShowText();
                topFade.SkipFade();
                topFade.SetFadeIn();
                isLast = true;
                break;
            case PageType.PictureTextBottom:
                pictureBottomText.SkipTypeText();
                pictureBottomText.ShowText();
                bottomFade.SkipFade();
                bottomFade.SetFadeIn();
                isLast = true;
                break;
            default:
                break;
        }
        isFinished = true;
    }

    public bool IsPageSkippable()
    {
        switch (type)
        {
            case PageType.Text:
                // 스킵할게 없습니다.
                if (!topText.IsSkippable() && !middleText.IsSkippable() 
                    && !bottomText.IsSkippable())
                    return false;

                break;
            case PageType.Picture:
                break;
            case PageType.PictureTextTop:
                if (!pictureTopText.IsSkippable()&& topFade.IsSkippable())
                    return false;
                
                break;
            case PageType.PictureTextBottom:
                if (!pictureBottomText.IsSkippable() && bottomFade.IsSkippable())
                    return false;

                break;
            default:
                break;
        }

        return true;
    }
}
