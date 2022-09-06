using System;
using System.IO;
using System.Security.Cryptography;
using System.Numerics;

namespace DiffieHellman
{
    class Program
    {
        static void Main(string[] args)
        {
            string IVStr = args[0];
            // g = 2^g_e - c
            string g_e = args[1];
            string g_c = args[2];
            // N = 2^N_e - c
            string N_e = args[3];
            string N_c = args[4];
            string x = args[5];
            string gyModN = args[6];
            string C = args[7];// 48bytes
            string P = args[8];
            BigInteger g = BigInteger.Pow(2, Int32.Parse(g_e));
            g = g - BigInteger.Parse(g_c);
            BigInteger N = BigInteger.Pow(2, Int32.Parse(N_e));
            N = N - BigInteger.Parse(N_c);
#if debug 
            Console.WriteLine("x = {0}", BigInteger.Parse(x));
            Console.WriteLine("g = {0}", g);
            Console.WriteLine("N = {0}", N);
#endif
            BigInteger gy = BigInteger.Parse(gyModN);
            BigInteger xBig = BigInteger.Parse(x);
            BigInteger key = BigInteger.ModPow(gy, xBig, N);
#if debug 
            Console.WriteLine("Key = {0}", key);
            Console.WriteLine("Key = {0}", key.ToString("X"));
#endif
            byte[] IV = StringToByteArray(IVStr);
            byte[] cipher = StringToByteArray(C);
            //byte[] k = paddKey(key.ToByteArray());
            byte[] k = key.ToByteArray(true,false);
#if debug 
            BigInteger verifyKey = new BigInteger(k);
            Console.WriteLine("Verify key convert byte-integer: {0}", verifyKey == key);
            Console.WriteLine("Encrypting {0}", P);
#endif
            // expect encrypted to be 48 bytes
            byte[] encryptedBytes = encrypt(P, k, IV);
            string encrypted = ByteArrayToString(encryptedBytes);
#if debug
            Console.WriteLine("Encrypted Text Len: {0}", encryptedBytes.Length);
            Console.WriteLine("Encrypted Text: {0}", encrypted);
            Console.WriteLine("Decrypting {0}", C);
#endif
            string decrypted = decrypt(cipher, k, IV);
            //string decrypted = decrypt(encryptedBytes, k, IV);
            Console.WriteLine("{0},{1}", decrypted, encrypted);
        }
        static byte [] encrypt(string plainText, byte[] key, byte[] IV)
        {
            byte[] encrypted;
            using (Aes aesAlg = Aes.Create()) {
#if AES
                Console.WriteLine("Default AES Key: {0}", ByteArrayToString(aesAlg.Key));
                Console.WriteLine("Default AES Mode: {0}", aesAlg.Mode);
                Console.WriteLine("Default AES Padding: {0}", aesAlg.Padding);
#endif
                aesAlg.Key = key;
                aesAlg.IV = IV;
#if debug
                Console.WriteLine("AES Key: {0}", ByteArrayToString(aesAlg.Key));
#endif
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
            return encrypted;
        }
        static string decrypt(byte[] cipher, byte[] key, byte[] IV)
        {
            string plainText = null;
#if debug
            Console.WriteLine("Length of cipherText: {0}", cipher.Length);
#endif
            using (Aes aesAlg = Aes.Create())
            {
#if AES
                Console.WriteLine("Default AES Key: {0}", ByteArrayToString(aesAlg.Key));
                Console.WriteLine("Default AES Mode: {0}", aesAlg.Mode);
                Console.WriteLine("Default AES Padding: {0}", aesAlg.Padding);
#endif
                aesAlg.Key = key;
                aesAlg.IV = IV;
#if debug
                Console.WriteLine("AES Key: {0}", ByteArrayToString(aesAlg.Key));
#endif

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipher))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plainText;
        }
        public static string ByteArrayToString(byte[] ba)
        {
#if debug
            Console.WriteLine("ByteArrayToString - Length: {0}", ba.Length);
#endif
            return BitConverter.ToString(ba).Replace("-"," ");
        }

        public static byte[] StringToByteArray(string hex)
        {
            hex = hex.Replace(" ", string.Empty);
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        static byte[] paddKey(byte[] key)
        {
            byte[] k = new byte[32];
            int paddLen = 32 - key.Length;
#if debug
            Console.WriteLine("Padding {0} byte to key", paddLen);
#endif
            for (int i = 0; i < paddLen; i++) {
                k[i] = 0x00;
            }
            for (int i = paddLen; i < 32; i++) {
                k[i] = key[i-paddLen];
            }
            return k;
        }
    }
}
