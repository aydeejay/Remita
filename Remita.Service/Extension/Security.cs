using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Remita.Service.Extension
{
    public static class Security
    {
        public static string Hash512(String text)
        {
            HashAlgorithm Hasher = new SHA512CryptoServiceProvider();
            byte[] strBytes = Encoding.UTF8.GetBytes(text);
            byte[] strHash = Hasher.ComputeHash(strBytes);
            return BitConverter.ToString(strHash).Replace("-", "").ToLowerInvariant().Trim();
        }

        public static string HmacSha512(string plainText, string key)
        {
            byte[] text_bytes = System.Text.Encoding.ASCII.GetBytes(plainText);
            byte[] key_bytes = System.Text.Encoding.ASCII.GetBytes(key);
            HMACSHA512 sha = new System.Security.Cryptography.HMACSHA512(key_bytes);
            byte[] sha_bytes = sha.ComputeHash(text_bytes);
            string sha_text = System.BitConverter.ToString(sha_bytes);
            sha_text = sha_text.Replace("-", "");
            sha_text = sha_text.ToLower();
            return sha_text;
        }
    }
}