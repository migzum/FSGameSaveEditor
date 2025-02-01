using System.Security.Cryptography;
using System.Text;

namespace GameSaveEditor
{
    public class EncryptionManager
    {
        private readonly byte[] _key = {
            0xa7, 0xca, 0x9f, 0x33, 0x66, 0xd8, 0x92, 0xc2,
            0xf0, 0xbe, 0xf4, 0x17, 0x34, 0x1c, 0xa9, 0x71,
            0xb6, 0x9a, 0xe9, 0xf7, 0xba, 0xcc, 0xcf, 0xfc,
            0xf4, 0x3c, 0x62, 0xd1, 0xd7, 0xd0, 0x21, 0xf9
        };

        private readonly byte[] _iv = {
            0x74, 0x75, 0x38, 0x39, 0x67, 0x65, 0x6A, 0x69, 
            0x33, 0x34, 0x30, 0x74, 0x38, 0x39, 0x75, 0x32
        };

        public string Encrypt(string jsonStr)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(jsonStr);
            using Aes aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using MemoryStream ms = new MemoryStream();
            using CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            cs.Write(plainBytes, 0, plainBytes.Length);
            cs.FlushFinalBlock();

            return Convert.ToBase64String(ms.ToArray());
        }

        public string Decrypt(string base64Str)
        {
            byte[] cipherBytes = Convert.FromBase64String(base64Str);
            using Aes aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using MemoryStream ms = new MemoryStream(cipherBytes);
            using CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using StreamReader reader = new StreamReader(cs);
            string plainText = reader.ReadToEnd();

            return plainText;
        }
    }
}