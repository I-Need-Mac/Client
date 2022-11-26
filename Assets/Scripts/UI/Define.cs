public class Define
{
    // UI화면 크기
    public const int uiScreenWidth = 1920;
    public const int uiScreenHeight = 1080;

    // UI프리팹 저장 경로
    public const string UiPrefabsPath = "Prefabs/UI";
    public const string UiImagePath = "Arts/UI";

    public const string UiPopupRoot = "@UI_Popup_Root";


    public enum UIEvent
    {   // UI이벤트 핸들러 관련W
        None = 0,
        Click,
        Enter,
        Exit,
        Down,
        Up
    }
}
