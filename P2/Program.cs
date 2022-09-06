using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace P2
{
    class BirthDayAtk
    {
        private static string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private static Random rand = new Random();
        private static Dictionary<string, string> collision = new Dictionary<string, string>();
        static void Main(string[] args)
        {
            string salt = Environment.GetCommandLineArgs()[1];
            byte saltByte = Convert.ToByte(salt.Trim(),16);
            //string s = "Hello World!";
            //byte[] b = Encoding.UTF8.GetBytes(s);
            //byte[] b2 = new byte[b.Length+1];
            //Array.Copy(b,b2, b.Length);
            //b2[b.Length] = saltByte;
            //Console.WriteLine(Get5BytesSubString(b2));

            string s1 = "";
            //string s2 = "";
            string hash1 = "hash1";
            //string hash2 = "hash2";
            MD5 md5 = MD5.Create();
            //while(!String.Equals(hash1, hash2)) {
            while(true) {
                s1 = RandomString(10);
                //s2 = RandomString(10);
                byte [] b1 = AppendSalt(s1, saltByte);
                //byte [] b2 = AppendSalt(s2, saltByte);

                //Console.WriteLine(ByteToString(b1));
                //Console.WriteLine(ByteToString(b2));
                byte[] o1 = md5.ComputeHash(b1);
                //byte[] o2 = md5.ComputeHash(b2);

                hash1 = Get5BytesSubString(o1);
                if (collision.ContainsKey(hash1)) {
                    Console.WriteLine("{0},{1}", s1, collision[hash1]);
                    break;
                }
                else {
                    collision.Add(hash1, s1);
                }
                //hash2 = Get5BytesSubString(o2);
            }
            
            //Console.WriteLine(hash1);
            //Console.WriteLine(hash2);
            //Console.WriteLine("{0},{1}", s1, s2);
        }
        public static string RandomString(int len)
        {
            char [] c = new char[len];
            for (int i = 0; i < len; i++) {
                c[i] = chars[rand.Next(chars.Length)];
            }
            return new String(c);
        }
        public static byte[] AppendSalt(string s, byte salt)
        {
            byte[] sBytes = Encoding.UTF8.GetBytes(s);
            byte[] bytes = new byte[sBytes.Length+1];
            Array.Copy(sBytes, bytes, sBytes.Length);
            bytes[sBytes.Length] = salt;
            return bytes;
        }
        public static string ByteToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-"," ");
        }
        public static string Get5BytesSubString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-"," ").Substring(0,14);
        }
    }
}
