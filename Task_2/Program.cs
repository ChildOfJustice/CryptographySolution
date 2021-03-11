using System;

namespace Task_2
{
    internal class Program
    {
        const byte Mask4Bit = (1 << 4) - 1;
        static byte[] Permutation =
        {
            16, 1, 2, 3, 4, 5, 6, 7
        };
        static ulong Substitute(ulong value, byte[] permutationRule)
        {
            if (permutationRule == null)
            {
                throw new ArgumentNullException(nameof(permutationRule));
            }
            if (permutationRule.Length > sizeof(byte)*8)
            {
                throw new ArgumentException("Too big array for permutation rule");
            }
            
            ulong result = 0;
            ulong currentMask = Mask4Bit;
            for (byte i = 0; i < sizeof(ulong)*8; i++)
            {
                byte curr4Bits = (byte) (value & currentMask);
                if (permutationRule[curr4Bits / 2] > 255) throw new ArgumentException("Wrong array");
                byte curr8BitsRule = permutationRule[curr4Bits/2];
                //Console.WriteLine(curr8BitsRule);
                byte new4Bits = 0;
                if (curr4Bits % 2 == 0)
                {
                    new4Bits = (byte) (curr8BitsRule & (Mask4Bit << 4));
                    //Console.WriteLine(new4Bits);
                }
                else
                {
                    new4Bits = (byte) (curr8BitsRule & (Mask4Bit));
                }

                ulong ulong_new4Bits = (ulong)new4Bits;
                ulong_new4Bits <<= i * 4;

                result |= ulong_new4Bits;
                //Console.WriteLine("NOW RES IS " + Convert.ToString((long)result, 2));

                currentMask <<= 4;
                
            }
            return result;
        }
        public static void Main(string[] args)
        {
            ulong a = 0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0100;
            Console.WriteLine(Convert.ToString((long)Substitute(a, Permutation), 2));
        }
    }
}