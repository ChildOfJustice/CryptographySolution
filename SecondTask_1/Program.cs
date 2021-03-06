using System;

namespace SecondTask_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
	        //Console.WriteLine(GaloisMultiplication(0xd4*8 + 0xbf*4 + 0x5d*2 + 0x30,0x03*8+0x01*40+0x01*2+0x02, 0x01 * 16 + 0x01).ToString("X"));
	        // Console.WriteLine((GaloisMultiplication(0xd4, 2)^GaloisMultiplication(0xbf, 3)^GaloisMultiplication(0x5d, 1)^GaloisMultiplication(0x30, 1)).ToString("X"));
	        // Console.WriteLine((GaloisMultiplication(0xd4, 1)^GaloisMultiplication(0xbf, 2)^GaloisMultiplication(0x5d, 3)^GaloisMultiplication(0x30, 1)).ToString("X"));
	        // return;
	        
	        //Проверка умножения элемента 115 на обратный к нему, полученный быстрым возведением в степень 254
	        Console.WriteLine("Should be 1:");
	        byte element = 115;
	        Console.WriteLine("b * b^-1 = " + GaloisMultiplication(element, FastPowMode(element, 254, 256)));
            //должна получиться единица

            Console.WriteLine("\n\n");
            Console.WriteLine("Check the ");
            Console.WriteLine(CheckIrreduciblePolynomial(283));
            Console.WriteLine(CheckIrreduciblePolynomial(282));
            Console.WriteLine(CheckIrreduciblePolynomial(500));
            
            

            // for (int i = 2; i < 255; i++)
            // {
	           //  // Console.WriteLine(FastPowMode((uint)i, 8, 256)^FastPowMode((uint)i, 4, 256)^FastPowMode((uint)i, 3, 256)^FastPowMode((uint)i, 1, 256)^1);
	           //  if ((FastPowMode((uint) i, 1, 256) ^ 1) == 0)
	           //  {
		          //   Console.WriteLine("IT IS NOT!!!");
		          //   
		          //   break;
	           //  }
	           //  Console.WriteLine(FastPowMode((uint) i, 1, 256) ^ 1);
	           //  
            // }
            //Console.WriteLine("IT IS");
        }
        
        
        public static byte GaloisMultiplication(byte m1, byte m2, uint Poly = 283) {
	        
	        byte p = 0;
	        byte counter;
	        byte hi_bit_set;
	        for (counter = 0; counter < 8; counter++)
	        {
		        if ((m2 & 1) != 0)
		        {
			        p ^= m1;
		        }
		        hi_bit_set = (byte)(m1 & 0x80);
		        m1 <<= 1;
		        if (hi_bit_set != 0)
		        {
			        m1 ^= 0x1b;
		        }
		        m2 >>= 1;
	        }
	        return p;
	        
	        
	        
			// if (0 == m1 || 0 == m2) { return 0; }
			// uint m1_tmp = m1;
			// uint m2_tmp;
			// uint m1_bit_num = 0;
			// uint PolyMultRez = 0;
			//
			// while (m1_tmp != 0) {
			// 	uint bit_m1 = (m1_tmp & 1u) == 0u ? 0u : 1u;
			// 	m1_tmp = m1_tmp >> 1;
			// 	m2_tmp = m2;
			// 	uint m2_bit_num;
			// 	m2_bit_num = 0;
			// 	while (m2_tmp != 0) {
			// 		uint bit_m2 = (m2_tmp & 1u) == 0u ? 0u : 1u;
			// 		m2_tmp = m2_tmp >> 1;
			// 		if ((bit_m1 != 0) && (bit_m2 != 0)) {
			// 			int BitNum = (int)(m2_bit_num + m1_bit_num);
			// 			PolyMultRez ^= 1u << BitNum;
			// 		}
			// 		m2_bit_num = m2_bit_num + 1;
			// 	}
			// 	m1_bit_num = m1_bit_num + 1;
			// }
			//
			// //Console.WriteLine(PolyMultRez);
			//
			// if(PolyMultRez > Poly)
			// 	return ModeDivide(PolyMultRez, Poly);
			// return PolyMultRez;
        }

        public static byte GaloisReverse(byte b)
        {
	        return FastPowMode(b, 254, 256);//256?
        }
        static uint ModeDivide(int _division, int _divisor)
        {
	        uint division = (uint) _division;
	        uint divisor = (uint) _divisor;
	        
	        if (division < divisor)
	        {
		        throw new ArgumentException("Cannot find the first non null bit when dividing by a greater value");
	        }
	        if (divisor == 0)
	        {
		        throw new DivideByZeroException();
	        }
	        
	        var maxNonNullBitForDivisor = GetMaxNonNullBitNumber(divisor);

	        var tmpDivision = division;
	        var currMaxNonNullBitNumber = GetMaxNonNullBitNumber(division);
	        
	        while (currMaxNonNullBitNumber >= maxNonNullBitForDivisor)
	        {
		        var tmpDivisor = divisor << (currMaxNonNullBitNumber-maxNonNullBitForDivisor);
		        tmpDivision ^= tmpDivisor;
		        currMaxNonNullBitNumber = GetMaxNonNullBitNumber(tmpDivision);
	        }

	        return tmpDivision;
        }
        static int GetMaxNonNullBitNumber(uint number)
        {
	        var tmpDivision = number;
	        int maxBitNumber = -1;
	        while (tmpDivision > 0)
	        {
		        tmpDivision >>= 1;
		        maxBitNumber++;
	        }

	        return maxBitNumber;
        }
        public static byte FastPowMode(byte number, uint pow, uint mode)
        {
	        byte result = 1;      
	        while (pow != 0) {
		        if (pow % 2 == 1)  result = GaloisMultiplication(result, number, 283);
		        pow >>= 1;
		        result = GaloisMultiplication(result, result, 283);
	        }
	        return result;
        }

        public static bool CheckIrreduciblePolynomial(int poly)
        {
	        for (int i = 2; i < poly; i++)
	        {
		        
		        if (i != poly && ModeDivide(poly,  i) == 0)
		        {
			        return false;
		        }
	        }

	        return true;
        }
    }
}