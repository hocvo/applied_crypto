using System;
using System.Security.Cryptography;
using System.IO;

namespace P1_2
{
    class Cryptanalysis
    {
        static void Main(string[] args)
        {
            DateTime dt = new DateTime(2020, 07, 03, 11, 00, 00);// DateTime.Now;
            TimeSpan ts = dt.Subtract(new DateTime(1970, 1, 1));
            string plainText = Environment.GetCommandLineArgs()[1];
            string cipherText = Environment.GetCommandLineArgs()[2];
            int minStart = (int) ts.TotalMinutes; 
            int minEnd = minStart + 24 * 60; // 1 day after
            //Console.WriteLine("Start at {0}, end at {1}", minStart, minEnd);
            for(int i = minStart; i <= minEnd; i++) {
                Random rng = new Random(i);
                byte[] key = BitConverter.GetBytes(rng.NextDouble());
                string encText = Encrypt(key, plainText);
                if (String.Equals(encText, cipherText)) {
                    Console.WriteLine(i);
                    break;
                }
            }
        }
        private static string Encrypt(byte[] key, string plainText)
        {
            DESCryptoServiceProvider csp = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms,
                    csp.CreateEncryptor(key, key), CryptoStreamMode.Write);
            StreamWriter sw = new StreamWriter(cs);
            sw.Write(plainText);
            sw.Flush();
            cs.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }

    }
}
