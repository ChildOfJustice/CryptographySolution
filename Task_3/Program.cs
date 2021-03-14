using System;

namespace Task_3
{
    internal class Program
    {
        const byte Mask4Bit = (1 << 4) - 1;
        public static Func<byte, byte> substituteRule;
        public static Random rand = new Random();

        static byte Rule(byte curr4Bits)
        {
            
            byte[] curr8BitsRuleArray = {0};
            rand.NextBytes(curr8BitsRuleArray);
            byte new4Bits = 0;
            byte curr8BitsRule = curr8BitsRuleArray[0]; 
            if (curr4Bits % 2 == 0)
            {
                new4Bits = (byte) ((curr8BitsRule & (Mask4Bit << 4)) >> 4);
                //Console.WriteLine(new4Bits);
            }
            else
            {
                new4Bits = (byte) (curr8BitsRule & (Mask4Bit));
            }

            return new4Bits;
        }
        
        
        
        static public ulong Substitute(ulong value, Func<byte, byte> substituteRule)
        {
            if (substituteRule == null)
            {
                throw new ArgumentNullException(nameof(substituteRule));
            }

            ulong result = 0;
            ulong currentMask = Mask4Bit;
            //going through ulong (64 bits) but with step = 4 bits
            for (byte i = 0; i < sizeof(ulong)*8/4; i++)
            {
                //Console.WriteLine(i);
                byte curr4Bits = (byte) ((value & currentMask) >> i*4);
                //Console.WriteLine(curr4Bits);
                byte new4Bits = substituteRule(curr4Bits);

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
            substituteRule = Rule;
            Console.WriteLine(Convert.ToString((long)Substitute(a, substituteRule), 2));
        }
    }
}