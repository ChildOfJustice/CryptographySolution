// using System;
// using System.Numerics;
// using System.Security.Cryptography;
// using System.Xml;
//
// namespace ThirdTask_2
// {
//     public class PreviousVersion
//     {
//        
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         
//         // тест Миллера — Рабина на простоту числа
//         // производится k раундов проверки числа n на простоту
//         public static bool MillerRabinTest(BigInteger n, int k)
//         {
//             // если n == 2 или n == 3 - эти числа простые, возвращаем true
//             if (n == 2 || n == 3)
//                 return true;
//
//             // если n < 2 или n четное - возвращаем false
//             if (n < 2 || n % 2 == 0)
//                 return false;
//
//             // представим n − 1 в виде (2^s)·t, где t нечётно, это можно сделать последовательным делением n - 1 на 2
//             BigInteger t = n - 1;
//
//             int s = 0;
//
//             while (t % 2 == 0)
//             {
//                 t /= 2;
//                 s += 1;
//             }
//
//             // повторить k раз
//             for (int i = 0; i < k; i++)
//             {
//                 // выберем случайное целое число a в отрезке [2, n − 2]
//                 RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
//
//                 byte[] _a = new byte[n.ToByteArray().LongLength];
//
//                 BigInteger a;
//
//                 do
//                 {
//                     rng.GetBytes(_a);
//                     a = new BigInteger(_a);
//                 }
//                 while (a < 2 || a >= n - 2);
//
//                 // x ← a^t mod n, вычислим с помощью возведения в степень по модулю
//                 BigInteger x = BigInteger.ModPow(a, t, n);
//
//                 // если x == 1 или x == n − 1, то перейти на следующую итерацию цикла
//                 if (x == 1 || x == n - 1)
//                     continue;
//
//                 // повторить s − 1 раз
//                 for (int r = 1; r < s; r++)
//                 {
//                     // x ← x^2 mod n
//                     x = BigInteger.ModPow(x, 2, n);
//
//                     // если x == 1, то вернуть "составное"
//                     if (x == 1)
//                         return false;
//
//                     // если x == n − 1, то перейти на следующую итерацию внешнего цикла
//                     if (x == n - 1)
//                         break;
//                 }
//
//                 if (x != n - 1)
//                     return false;
//             }
//
//             // вернуть "вероятно простое"
//             return true;
//         }
//         
//         
//         
//         
//         public static BigInteger MHP(BigInteger a, BigInteger k, BigInteger b)
//         {
//             //return ThirdTask_1.Program.FastPowModeBigInt(a, k % ThirdTask_1.Program.EulerFunction(b), b);
//             return BigInteger.ModPow(a, k % ThirdTask_1.Program.EulerFunction(b), b);
//             //return Math.Pow(a, k % ThirdTask_1.Program.EulerFunction(b)) % b;
//         }
//         public static bool FermaTest(BigInteger n, int k)
//         {
//             // если n == 2 или n == 3 - эти числа простые, возвращаем true
//             if (n == 2 || n == 3)
//                 return true;
//
//             // если n < 2 или n четное - возвращаем false
//             if (n < 2 || n % 2 == 0)
//                 return false;
//
//             // if (startPosCheckFrom < 1)
//             //     throw new ArgumentException("Cannot check from this position");
//
//             // for(BigInteger a = startPosCheckFrom; a < n-1; a++)
//             // {
//             //     if (MHP(a, n-1, n) != 1) 
//             //     {
//             //         return false;
//             //     }
//             // }
//             
//             
//             // повторить k раз
//             for (int i = 0; i < k; i++)
//             {
//                 // выберем случайное целое число a в отрезке [2, n − 2]
//                 RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
//
//                 byte[] _a = new byte[n.ToByteArray().LongLength];
//
//                 BigInteger a;
//
//                 do
//                 {
//                     rng.GetBytes(_a);
//                     a = new BigInteger(_a);
//                     Console.WriteLine("choosing..");
//                 }
//                 while (a < 2 || a >= n - 2);
//
//                 if (MHP(a, n-1, n) != 1) 
//                 {
//                     return false;
//                     
//                 }
//             }
//             
//             
//             return true;
//         }
//
//
//
//
//
//         public static bool SoloveyShtrassenTest(BigInteger n, int k)
//         {
//             // если n == 2 или n == 3 - эти числа простые, возвращаем true
//             if (n == 2 || n == 3)
//                 return true;
//
//             // если n < 2 или n четное - возвращаем false
//             if (n < 2 || n % 2 == 0)
//                 return false;
//
//             //TODO
//             return MillerRabinTest(n, k);
//             
//             // повторить k раз
//             for (int i = 0; i < k; i++)
//             {
//                 // выберем случайное целое число a в отрезке [2, n − 2]
//                 RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
//
//                 byte[] _a = new byte[n.ToByteArray().LongLength];
//
//                 BigInteger a;
//
//                 do
//                 {
//                     rng.GetBytes(_a);
//                     a = new BigInteger(_a);
//                 } while (a < 2 || a >= n - 2);
//
//                 if (ThirdTask_1.Program.ExtendedEuclideanAlgorithm(a, n, out BigInteger x, out BigInteger y) > 1)
//                     return false;
//
//                 // if (ThirdTask_1.Program.FastPowModeBigInt(a, (n - 1) / 2, n) != ThirdTask_1.Program.Jacobi(a, n))
//                 //     return false;
//                 Console.WriteLine("a is " + a);
//                 Console.WriteLine("n is " + n);
//                 Console.WriteLine("Trying to: a^((n-1)/2)=" + BigInteger.ModPow(a, (n - 1) / 2, n) + " and J:" + ThirdTask_1.Program.J(a, n));
//                 if (BigInteger.ModPow(a, (n - 1) / 2, n) != ThirdTask_1.Program.J(a, n))
//                     return false;
//
//             }
//
//             return true;
//         }
//     }
// }