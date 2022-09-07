#define debug
using System;
using System.Numerics;

namespace P4
{
    class RSA
    {
        static BigInteger e = 65537;
        static BigInteger NEG_ONE = new BigInteger(-1);
        static void Main(string[] args)
        {
            string p_e = args[0];
            string p_c = args[1];
            string q_e = args[2];
            string q_c = args[3];
            string cipher = args[4];
            string plain = args[5];

            BigInteger p = BigInteger.Pow(2, Int32.Parse(p_e));
            p = p - BigInteger.Parse(p_c);
            BigInteger q = BigInteger.Pow(2, Int32.Parse(q_e));
            q = q - BigInteger.Parse(q_c);
            BigInteger modulus = p * q;
            BigInteger phi = (p-1) * (q-1);
            BigInteger d = InverseMod(phi, e, NEG_ONE, NEG_ONE, NEG_ONE, NEG_ONE, phi);
            //BigInteger d = InverseMod(26, 15, NEG_ONE, NEG_ONE, NEG_ONE, NEG_ONE, 26);
#if debug
            Console.WriteLine(d);
            Console.WriteLine("Is ed % phi = 1: {0}", (e*d%phi)==1);
#endif
            // Decrypting
            BigInteger C = BigInteger.Parse(cipher);
#if debug
            Console.WriteLine("Decrypting the cipher {0}", C);
#endif
            BigInteger decrypted = BigInteger.ModPow(C, d, modulus);

            // Encrypting
            BigInteger M = BigInteger.Parse(plain);      
#if debug
            Console.WriteLine("Encrypting the message {0}", M);
#endif
            BigInteger encrypted = BigInteger.ModPow(M, e, modulus);
            Console.WriteLine("{0},{1}", decrypted, encrypted);
        }
        /**
         * Find the inverse mod of phi for e value.
         * From: http://www-math.ucdenver.edu/~wcherowi/courses/m5410/exeucalg.html
         * For the first two steps, the value of this number is given: p0 = 0 and p1 = 1.
         * For the remainder of the steps, we recursively calculate pi = pi-2 - pi-1 * qi-2 (mod n).
         * Continue this calculation for one step beyond the last step of the Euclidean algorithm.
         */
        // p1 = p(i-1), p2 = p(i-2) where i is the ith iteration
        // need to keep 2 last quotients to calculate P(i). 
        static BigInteger InverseMod(BigInteger dividend, BigInteger divisor, BigInteger q1, BigInteger q2,
         BigInteger p1, BigInteger p2, BigInteger modulus)
        {
#if trace
            Console.WriteLine("q1,q2 = {0},{1}", q1, q2);
#endif
            if (divisor == 0) {
                return calculateP(p1, p2, q2, modulus);
            }
            BigInteger q = dividend / divisor;
            BigInteger remainder = dividend % divisor;
            if (remainder < 0) {
                remainder += divisor;
            }
#if debug
            if (remainder == 1) {
                Console.WriteLine("Found remainder of 1. Has inverse!");
            }
#endif
#if trace
            Console.WriteLine("{0} = {1}({2}) + {3}", dividend, q, divisor, remainder);
#endif
            if (p2 == NEG_ONE) {// first iteration
                p2 = 0;
            }
            else if (p1 == NEG_ONE) {// second iteration
                p1 = 1;
            }
            else {
                BigInteger p = calculateP(p1, p2, q2, modulus);
                p2 = p1;
                p1 = p;
            }
            q2 = q1;
            q1 = q;
            return InverseMod(divisor, remainder, q1, q2, p1, p2, modulus);
        }
        /**
        * pi = pi-2 - pi-1 * qi-2 (mod n).
        */
        static BigInteger calculateP(BigInteger p1, BigInteger p2,
         BigInteger q2, BigInteger modulus)
        {
            BigInteger p = p2 - (p1 * q2) % modulus;
            if (p < 0) {
                p += modulus;
            }
#if trace
            Console.WriteLine("p = {0}-({1}*{2}) % {3} = {4}", p2, p1, q2, modulus, p);
#endif
            return p;
        }
    }
}
