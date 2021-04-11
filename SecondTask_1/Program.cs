using System;
using System.Collections.Generic;

namespace SecondTask_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // foreach (var primeNumber in FindPrimeNumbers(100))
            // {
            //     Console.Write(primeNumber + ", ");
            // }

            // Console.WriteLine(Galois_b2_ext_mult(115, 183, 283));

            
            //Console.WriteLine(Galois_b2_ext_mult(FastPowMode(115, 254, 254), 115, 283));//283

            uint b = 115;
            uint res = 115;
            for (int i = 1; i < 255; i++)
            {
            
	            res = Galois_b2_ext_mult(res, b, 283);
            }
            
            Console.WriteLine(res);


            Console.WriteLine(Galois_b2_ext_mult(115, FastPowMode(115, 254, 256), 283));
            //Console.WriteLine(Galois_b2_ext_mult(115, 183, 283));//283
            //Console.WriteLine(FastPowMode(115, 282, 282));
            
            // Console.WriteLine(FastPowMode(595, 703, 991));
        }

        int Gcd(int a, int b, out int x, out int y)
        {
	        if (b < a)
	        {
		        var t = a;
		        a = b;
		        b = t;
	        }
    
	        if (a == 0)
	        {
		        x = 0;
		        y = 1;
		        return b;
	        }
 
	        int gcd = Gcd(b % a, a, out x, out y);
    
	        int newY = x;
	        int newX = y - (b / a) * x;
    
	        x = newX;
	        y = newY;
	        return gcd;
        }
        
        static uint GetLeadBitNum(UInt32 Val) {
	        if (0 == Val) {
		        Console.WriteLine("Попытка найти старший бит числа \"0\"! Это ни к чему хорошему не приведёт."); return 0; }
		        
	        int BitNum = 31;
	        uint CmpVal = 1u << BitNum;
	        while (Val < CmpVal) {
		        CmpVal >>= 1;
		        BitNum--;
	        }
	        return (uint)BitNum;
        }
        
        //https://habr.com/ru/post/528142/
        public static uint Galois_b2_ext_mult(uint m1, uint m2, uint Poly = 283) {
			if (0 == m1 || 0 == m2) { return 0; }
			uint m1_tmp = m1;
			uint m2_tmp;
			uint m1_bit_num = 0;

			//Перемножение двух полиномов, при использовании арифметики по модулю 2 достаточно простое занятие.
			//перебираем единички и нолики (для каждого бита первого числа перебираем все биты второго (или наоборот)), складываем номера позиций битов,
			//но не всегда, а только когда оба перебираемых бита - единицы, и инвертируем бит результата под номером, равном сумме позиций для данного шага перебора
			//(инверсия - это прибавление единицы по модулю 2)
			uint PolyMultRez = 0;

			while (m1_tmp != 0) {
				uint bit_m1 = (m1_tmp & 1u) == 0u ? 0u : 1u;
				m1_tmp = m1_tmp >> 1;
				m2_tmp = m2;
				uint m2_bit_num;
				m2_bit_num = 0;
				while (m2_tmp != 0) {
					uint bit_m2 = (m2_tmp & 1u) == 0u ? 0u : 1u;
					m2_tmp = m2_tmp >> 1;
					if ((bit_m1 != 0) && (bit_m2 != 0)) {
						int BitNum = (int)(m2_bit_num + m1_bit_num);
						PolyMultRez ^= 1u << BitNum;
					}
					m2_bit_num = m2_bit_num + 1;
				}
				m1_bit_num = m1_bit_num + 1;
			}
			
			//Тут есть результат умножения полиномов PolyMultRez. Осталось найти остаток от деления на выбранный порождающий полином.
			//Деление полиномов происходит так: Берём старшую степень делимого, и вычитаем старшую степень делителя. 
			//Получаем число - степень частного
			//Теперь перемножаем, а по сути, просто прибавляем к каждой степени делителя степень получившегося частного
			//и повторяем всё по кругу, пока степень делимого не окажется меньше степени делителя
			if(PolyMultRez > Poly)
				return ModeDivide(PolyMultRez, Poly);
			return PolyMultRez;
			
			
			
			
			
			
			
			
			
			
			
			// uint TmpDivisor_lead_bit_n;
			// uint TmpQuotient;
			// uint TmpDivisor = Poly;
			// uint TmpDividend = PolyMultRez;
			// uint TmpDividend_LeadBitNum;
			// uint TmpMult_bitNum;
			// uint TmpMult_rez;

			// TmpDividend_LeadBitNum = GetLeadBitNum(TmpDividend);
			// TmpDivisor_lead_bit_n = GetLeadBitNum(TmpDivisor);

			
			uint tmpDivisor = Poly;
			uint tmpDivision = PolyMultRez;
			while (tmpDivision >= tmpDivisor)
			{
				
			}
			
			
			// while (TmpDividend_LeadBitNum >= TmpDivisor_lead_bit_n) {
			//
			// 	TmpQuotient = (TmpDividend_LeadBitNum - TmpDivisor_lead_bit_n);
			//
			// 	TmpMult_bitNum = 0;
			// 	TmpMult_rez = 0;
			// 	while (TmpDivisor != 0) {
			// 		uint bit_TmpMult = (TmpDivisor & 1u) == 0u ? 0u : 1u;
			// 		TmpDivisor >>= 1;
			// 		TmpMult_rez ^= bit_TmpMult << (int)(TmpQuotient + TmpMult_bitNum);
			// 		TmpMult_bitNum = TmpMult_bitNum + 1;
			// 	}
			// 	TmpDividend = TmpDividend ^ TmpMult_rez;
			// 	TmpDivisor = Poly;
			// 	TmpDividend_LeadBitNum = GetLeadBitNum(TmpDividend);
			// }
			//Результат умножения числел есть остаток от деления произведения многочленов на порождающий полином.
			//return TmpDividend;
			return 0;
        }

        public static uint GaloisReverse(uint b)
        {
	        return FastPowMode(b, 254, 256);
        }

        static uint ModeDivide(uint division, uint divisor)
        {
	        if (division < divisor)
	        {
		        throw new Exception("Cannot find the first non null bit when dividing by a greater value");
	        }
	        
	        var maxNonNullBitForDivisor = GetMaxNonNullBitNumber(divisor);


	        var tmpDivision = division;
	        var currMaxNonNullBitNumber = GetMaxNonNullBitNumber(division);
	        
	        while (currMaxNonNullBitNumber >= maxNonNullBitForDivisor)
	        {
		        var tmpDivisor = divisor << (currMaxNonNullBitNumber-maxNonNullBitForDivisor);
		        //Console.WriteLine("DIVIDING: " + tmpDivision + " - " + tmpDivisor);
		        tmpDivision ^= tmpDivisor;
		        currMaxNonNullBitNumber = GetMaxNonNullBitNumber(tmpDivision);
		        //Console.WriteLine("ITER: " + tmpDivision);
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




        public static uint FastPowMode(uint number, uint pow, uint mode)
        {
	        uint result = 1;      
	        while (pow != 0) {
		        if (pow % 2 == 1)  result = Galois_b2_ext_mult(result, number, 283);
		        pow >>= 1;
		        result = Galois_b2_ext_mult(result, result, 283);
	        }
	        
	        return result;
	        
	        
	        
	        
	        // var numberInBase2 = new List<bool>();
	        //
	        // while (pow > 1)
	        // {
	        //  numberInBase2.Add(pow % 2 == 1);
	        //  pow /= 2;
	        // }
	        // numberInBase2.Add(pow == 1);
	        //
	        // numberInBase2.Reverse();
	        //
	        //
	        // var result = 1u;
	        //
	        // for (int i = 0; i < numberInBase2.Count; i++)
	        // {
	        //  if (numberInBase2[i])
	        //  {
	        //   // result *= number;
	        //   // result %= mode;
	        //   result = Galois_b2_ext_mult(result, number, 283);
	        //   //Console.WriteLine("1! *= number: " + result);
	        //  }
	        //
	        //  if (i != numberInBase2.Count - 1)
	        //  {
	        //   // result *= result;
	        //   // result %= mode;
	        //   result = Galois_b2_ext_mult(result, result, 283);
	        //   //Console.WriteLine("^2: " + result);
	        //  }
	        //  
	        // }
	        // return result;
        }
        
        
        
        
        
        
        
        
        
        
        public static List<uint> FindPrimeNumbers(int n)
        {
            var numbers = new List<uint>();
            //заполнение списка числами от 2 до n-1
            for (var i = 2u; i < n; i++)
            {
                numbers.Add(i);
            }

            for (var i = 0; i < numbers.Count; i++)
            {
                for (var j = 2u; j < n; j++)
                {
                    //удаляем кратные числа из списка
                    numbers.Remove(numbers[i] * j);
                }
            }

            return numbers;
        }
    }
}










//https://ru.wikipedia.org/wiki/%D0%A0%D0%B5%D1%88%D0%B5%D1%82%D0%BE_%D0%AD%D1%80%D0%B0%D1%82%D0%BE%D1%81%D1%84%D0%B5%D0%BD%D0%B0
//https://programm.top/c-sharp/programs/sieve-eratosthenes/