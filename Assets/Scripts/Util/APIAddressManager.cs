using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class APIAddressManager 
{

    public const string REQUEST_GETTEST = "api";
    public const string REQUEST_REGIST ="api/auth/regist";
    public const string REQUEST_LOGIN = "api/auth/login";
    public const string REQUEST_CHECKNAME ="api/auth/duplicated";
    public const string REQUEST_GAME_START = "api/game/start";
    public const string REQUEST_OPEN_SORCERER = "api/characters/open";
    public const string REQUEST_USER_OWN_BOX = "api/reward-box/user-own/990321";
    public const string REQUEST_BOX_OPEN_START = "api/reward-box/open-start";
    public const string REQUEST_BOX_OPEN = "api/reward-box/open-end";
}
