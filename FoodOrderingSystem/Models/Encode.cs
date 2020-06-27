using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace FoodOrderingSystem.Models
{
    public class AesEncryptor
    {
        private static readonly string DefaultKey = "dAFv18D2mWizMLk2LeJjRu90n59HY5OX";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="key">Key must be 32 character long, add 'AESEncryptionDefaultKey' to config for default key, else pre-default key will be use</param>
        /// <returns></returns>
        public static string Encrypt(string txt, string key = "")
        {
            try
            {
                key = string.IsNullOrEmpty(key) ? DefaultKey : key;
                if (key.Length != 32)
                    return string.Empty;
                return string.IsNullOrEmpty(txt) ? string.Empty : IosAesEncryptor.EncryptString(txt, key);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key">Key must be 32 character long, add 'AESEncryptionDefaultKey' to config for default key, else pre-default key will be use</param>
        /// <returns></returns>
        public static string Decrypt(string str, string key = "")
        {
            try
            {
                key = string.IsNullOrEmpty(key) ? DefaultKey : key;
                if (key.Length != 32)
                    return string.Empty;
                return string.IsNullOrEmpty(str) ? null : IosAesEncryptor.DecryptString(str, key);
            }
            catch
            {
                return null;
            }
        }

    }

    public class IosAesEncryptor
    {
        public static string EncryptString(string plainSourceStringToEncrypt, string passPhrase)
        {
            //Set up the encryption objects
            using (var acsp = GetProvider(Encoding.Default.GetBytes(passPhrase)))
            {
                var sourceBytes = Encoding.ASCII.GetBytes(plainSourceStringToEncrypt);
                var ictE = acsp.CreateEncryptor();

                //Set up stream to contain the encryption
                var msS = new MemoryStream();

                //Perform the encrpytion, storing output into the stream
                var csS = new CryptoStream(msS, ictE, CryptoStreamMode.Write);
                csS.Write(sourceBytes, 0, sourceBytes.Length);
                csS.FlushFinalBlock();

                //sourceBytes are now encrypted as an array of secure bytes
                var encryptedBytes = msS.ToArray(); //.ToArray() is important, don't mess with the buffer

                //return the encrypted bytes as a BASE64 encoded string
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        public static string DecryptString(string base64StringToDecrypt, string passphrase)
        {
            //Set up the encryption objects
            using (var acsp = GetProvider(Encoding.Default.GetBytes(passphrase)))
            {
                var rawBytes = Convert.FromBase64String(base64StringToDecrypt);
                var ictD = acsp.CreateDecryptor();

                //RawBytes now contains original byte array, still in Encrypted state

                //Decrypt into stream
                var msD = new MemoryStream(rawBytes, 0, rawBytes.Length);
                var csD = new CryptoStream(msD, ictD, CryptoStreamMode.Read);
                //csD now contains original byte array, fully decrypted

                //return the content of msD as a regular string
                return (new StreamReader(csD)).ReadToEnd();
            }
        }

        private static AesCryptoServiceProvider GetProvider(byte[] key)
        {
            var result = new AesCryptoServiceProvider
            {
                BlockSize = 128,
                KeySize = 128,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };

            result.GenerateIV();
            result.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            var realKey = GetKey(key, result);
            result.Key = realKey;
            return result;
        }

        private static byte[] GetKey(byte[] suggestedKey, SymmetricAlgorithm p)
        {
            var kRaw = suggestedKey;
            var kList = new List<byte>();

            for (var i = 0; i < p.LegalKeySizes[0].MinSize; i += 8)
            {
                kList.Add(kRaw[(i / 8) % kRaw.Length]);
            }
            var k = kList.ToArray();
            return k;
        }
    }
}

