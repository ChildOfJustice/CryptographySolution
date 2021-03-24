using System;

namespace Task_2
{
    public class Program
    {
        const byte Mask4Bit = (1 << 4) - 1;
        static byte[] Permutation =
        {
            16, 1, 2, 3, 4, 5, 6, 7 // [0001]_0000_0000_0001_[0000]_0010 ...
        };
        static public ulong Substitute(ulong value, byte[] permutationRule)
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
            //going through ulong (64 bits) but with step = 4 bits
            for (byte i = 0; i < sizeof(ulong)*8/4; i++)
            {
                //Console.WriteLine(i);
                byte curr4Bits = (byte) ((value & currentMask) >> i*4);
                //Console.WriteLine(curr4Bits);
                if (permutationRule[curr4Bits / 2] > 255) throw new ArgumentException("Wrong array on index " + i);
                byte curr8BitsRule = permutationRule[curr4Bits/2];
                //Console.WriteLine(curr8BitsRule);
                byte new4Bits = 0;
                if (curr4Bits % 2 == 0)
                {
                    new4Bits = (byte) ((curr8BitsRule & (Mask4Bit << 4)) >> 4);//get the left 4 bits and then shift them to right so we will only have 4 bits digit
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
            
            // try
            // {
            //     Console.WriteLine(Convert.ToString((long)Permute(a, InitialPermutation), 2));
            //     
            // } catch (ArgumentNullException e)
            // {
            //     Console.WriteLine(e.Message);
            // }
            // catch (ArgumentException e)
            // {
            //     Console.WriteLine(e.Message);
            // }
        }
    }
}