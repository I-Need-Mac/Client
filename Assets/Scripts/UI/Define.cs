public class Define
{
    // UIȭ�� ũ��
    public const int uiScreenWidth = 1920;
    public const int uiScreenHeight = 1080;

    // UI������ ���� ���
    public const string UiPrefabsPath = "Prefabs/UI";
    public const string UiPopupRoot = "@UI_Popup_Root";

    public enum UIEvent
    {   // UI�̺�Ʈ �ڵ鷯 ����
        None = 0,
        Click,
        Enter,
        Exit,
        Down,
        Up
    }
}
