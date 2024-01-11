using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using UnityEngine;
public class Crypto
{
    //static Dictionary<string, CryptoRSA> _rsaManages = new Dictionary<string, CryptoRSA>();
    private string publicKeyPEM;
    private string privateKeyPEM;

    
    public Crypto() {

      
    }

    public void SetPublicKeyPEM(TextAsset pem) {
        publicKeyPEM = pem.text;

    }
    public void SetPrivateKeyPEM(TextAsset pem)
    {
        privateKeyPEM = pem.text;

    }

    public string Encrypt(string plainText)
    {
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        OaepEncoding eng = new OaepEncoding(new RsaEngine());
        eng.Init(true, DeserializePublicKey(publicKeyPEM));

        int length = plainTextBytes.Length;
        int blockSize = eng.GetInputBlockSize();
        List<byte> cipherTextBytes = new List<byte>();
        for (int chunkPosition = 0;
            chunkPosition < length;
            chunkPosition += blockSize)
        {
            int chunkSize = Math.Min(blockSize, length - chunkPosition);
            cipherTextBytes.AddRange(eng.ProcessBlock(
                plainTextBytes, chunkPosition, chunkSize
            ));
        }
        return Convert.ToBase64String(cipherTextBytes.ToArray());
    }

    public string Decrypt(string encryptedText )
    {
        byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);

        OaepEncoding eng = new OaepEncoding(new RsaEngine());
        eng.Init(false, DeserializePrivateKey(privateKeyPEM));

        int length = cipherTextBytes.Length;
        int blockSize = eng.GetInputBlockSize();
        List<byte> plainTextBytes = new List<byte>();
        for (int chunkPosition = 0;
            chunkPosition < length;
            chunkPosition += blockSize)
        {
            int chunkSize = Math.Min(blockSize, length - chunkPosition);
            plainTextBytes.AddRange(eng.ProcessBlock(
                cipherTextBytes, chunkPosition, chunkSize
            ));
        }
        return Encoding.UTF8.GetString(plainTextBytes.ToArray());
    }

    private AsymmetricKeyParameter DeserializePublicKey(string pem)
    {
        using (StringReader reader = new StringReader(pem))
        {
            PemReader pemReader = new PemReader(reader);
            return (AsymmetricKeyParameter)pemReader.ReadObject();
        }
    }

    private AsymmetricKeyParameter DeserializePrivateKey(string pem)
    {
        using (StringReader reader = new StringReader(pem))
        {
            PemReader pemReader = new PemReader(reader);
            return ((AsymmetricCipherKeyPair)pemReader.ReadObject()).Private;
        }
    }


}