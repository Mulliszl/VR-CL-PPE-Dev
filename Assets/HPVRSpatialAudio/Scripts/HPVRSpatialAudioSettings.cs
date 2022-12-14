// (c) Copyright 2020 HP Development Company, L.P.

using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using UnityEngine;

namespace HP.VR.SpatialAudio
{
    public class HPVRSpatialAudioSettings : ScriptableObject
    {
        public enum LicenseTypes : int
        {
            Trial = 2,
            Enterprise = 3,
            Rev_Share = 4 
        }

        public LicenseTypes LicenseType = LicenseTypes.Trial;

        [HideInInspector]
        public int RequestedLicense => (int) LicenseType;

        [HideInInspector]
        [SerializeField]
        private byte[] ClientIDBytes;

        [HideInInspector]
        [SerializeField]
        private byte[] AccessKeyBytes;

        private byte[] ENCRYPT_KEY = Encoding.ASCII.GetBytes("1CaIy2QAi27pyQvZcTGaGLbDxDGzBxZP");
        private byte[] ENCRYPT_IV = Encoding.ASCII.GetBytes("vq8J6AQOe1UjX22u");

        public string ClientID
        {
            get
            {
                string id = "";
                if (ClientIDBytes?.Length > 0)
                {
                    id = DecryptStringFromBytes_Aes(ClientIDBytes, ENCRYPT_KEY, ENCRYPT_IV);
                }
                return id;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    ClientIDBytes = EncryptStringToBytes_Aes(value, ENCRYPT_KEY, ENCRYPT_IV);
                }
                else
                {
                    ClientIDBytes = new byte[] { };
                }
            }
        }

        public string AccessKey
        {
            get
            {
                string key = "";
                if (AccessKeyBytes?.Length > 0)
                {
                    key = DecryptStringFromBytes_Aes(AccessKeyBytes, ENCRYPT_KEY, ENCRYPT_IV);
                }
                return key;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    AccessKeyBytes = EncryptStringToBytes_Aes(value, ENCRYPT_KEY, ENCRYPT_IV);
                }
                else
                {
                    AccessKeyBytes = new byte[] { };
                }
            }
        }

        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}
