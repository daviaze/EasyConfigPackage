using System.Security.Cryptography;
using System.Text;

namespace EasyConfig
{
    internal class Encryption
    {
        internal static string Encrypt(string plainText, string password)
        {
            // Derivar uma chave e IV (Initialization Vector) a partir da senha
            using var aesAlg = Aes.Create();
            var key = GenerateKeyFromPassword(password, aesAlg.KeySize / 8);
            var iv = GenerateKeyFromPassword(password, aesAlg.BlockSize / 8);

            aesAlg.Key = key;
            aesAlg.IV = iv;

            using var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                using var swEncrypt = new StreamWriter(csEncrypt);
                swEncrypt.Write(plainText);
            }
            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        internal static string Decrypt(string plainText, string password)
        {
            // Derivar uma chave e IV a partir da senha
            using var aesAlg = Aes.Create();
            var key = GenerateKeyFromPassword(password, aesAlg.KeySize / 8);
            var iv = GenerateKeyFromPassword(password, aesAlg.BlockSize / 8);

            aesAlg.Key = key;
            aesAlg.IV = iv;

            using var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            using var msDecrypt = new MemoryStream(Convert.FromBase64String(plainText));
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            return srDecrypt.ReadToEnd();
        }

        internal static byte[] GenerateKeyFromPassword(string password, int length)
        {
            using var sha256 = SHA256.Create();
            // Gerar o hash completo da senha
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(passwordBytes);

            // Retornar apenas os primeiros 'length' bytes do hash
            byte[] key = new byte[length];
            Array.Copy(hash, key, length);
            return key;
        }
    }
}
