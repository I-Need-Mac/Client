using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptoComponent : MonoBehaviour
{
    [SerializeField]
    public TextAsset rsaBase64PublicKey;
    public TextAsset rsaBase64PrivateKey;
    [SerializeField]
    string steamCustomID = "";
    private void Start()
    {
        APIManager.Instance.SetPublicKeyPEM(rsaBase64PublicKey);
        APIManager.Instance.SetPrivateKeyPEM(rsaBase64PrivateKey);
       APIManager.Instance.SetSteamID(steamCustomID);
        DebugManager.Instance.PrintDebug("[Crypto] Crypto Init");
       
    }


}
