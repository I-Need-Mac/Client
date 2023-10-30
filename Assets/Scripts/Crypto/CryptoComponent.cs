using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptoComponent : MonoBehaviour
{
    [SerializeField]
    public TextAsset rsaBase64PublicKey;
    public TextAsset rsaBase64PrivateKey;

    private void Start()
    {
        Crypto.Instance.SetPrivateKeyPEM(rsaBase64PrivateKey);
        Crypto.Instance.SetPublicKeyPEM(rsaBase64PublicKey);
    }
}