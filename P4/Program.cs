using System;
using System.Numerics;

namespace P4
{
    class RSA
    {
        static BigInteger e = 65537;
        static void Main(string[] args)
        {
            string p_e = args[0];
            string p_c = args[1];
            string q_e = args[2];
            string q_c = args[3];
            string C = args[4];
            string P = args[5];

            BigInteger p = BigInteger.Pow(2, Int32.Parse(p_e));
            p = p - BigInteger.Parse(p_c);
            BigInteger q = BigInteger.Pow(2, Int32.Parse(q_e));
            q = q - BigInteger.Parse(q_c);
            BigInteger test = InverseMod(new BigInteger(65538));
        }
        /**
         * Find the inverse mod of phi for e value.
         */
        static BigInteger InverseMod(BigInteger phi)
        {
            BigInteger p_0 = 0;
            BigInteger p_1 = 0;
            BigInteger q = phi / e;
            BigInteger r = phi % e;
            Console.WriteLine(q);
            Console.WriteLine(r);
            return q;
        }
    }
}
