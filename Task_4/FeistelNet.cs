using System;

namespace Task_4
{
    public abstract class FeistelNet
    {
        private ulong[] RoundKeys;
        //public abstract ulong[] generateRoundKeys();//RoundKeys = new ulong[16];
        
        const uint Mask28Bit = ((uint)1 << 28) - 1;
        const ulong Mask32Bit = ((ulong)1 << 32) - 1;
        const ulong Mask56Bit = ((ulong)1 << 56) - 1;
        
        protected ulong Data;

        public byte[] CipherTemplateMethod(byte[] DataBytes, bool encrypt)
        {
	        ProcessDataBytes(DataBytes);
	        Hook1();//InitialPermutation
	        DoTheJob(encrypt);
	        Hook2();//FinalPermutation
	        return BitConverter.GetBytes(Data);
        }

        protected void ProcessDataBytes(byte[] DataBytes)
        {
	        if (DataBytes.Length != 8)
	        {
		        throw new ArgumentException("DataBytes");
	        }
	        Data = BitConverter.ToUInt64(DataBytes, 0);
        }

        protected void DoTheJob(bool encrypt)
        {
	        uint L = (uint)((Data >> 32) & Mask32Bit), PreviousL = L;
	        uint R = (uint)(Data & Mask32Bit), PreviousR = R;
	        for (byte Round = 0; Round < 16; Round++)
	        {
		        if (encrypt)
		        {
			        L = PreviousR;
			        R = PreviousL ^ AbstractFeistelFunction(PreviousR, RoundKeys[Round]);
			        PreviousL = L;
			        PreviousR = R;
		        }
		        else
		        {
			        R = PreviousL;
			        L = PreviousR ^ AbstractFeistelFunction(PreviousL, RoundKeys[15 - Round]);
			        PreviousR = R;
			        PreviousL = L;
		        }
	        }
	        //return BitConverter.GetBytes(Permute(((ulong)L << 32) | R, FinalPermutation));
	        Data = ((ulong) L << 32) | R;
        }

        protected abstract uint AbstractFeistelFunction(uint R, ulong RoundKey);
        
        protected virtual void Hook1() { }

        protected virtual void Hook2() { }
        
        
        
        
        
        static byte[] KeyPermutation =
        {
	        50, 43, 36, 29, 22,	15,  8,  1, 51, 44, 37, 30, 23, 16,
	        9,  2, 52, 45, 38,	31, 24, 17, 10,  3, 53, 46, 39, 32,
	        56, 49, 42, 35, 28,	21, 14,  7, 55, 48, 41, 34, 27, 20,
	        13,  6, 54, 47, 40,	33, 26, 19, 12,  5, 25, 18, 11,  4
        };

        static byte[] RoundKeyPermutation =
        {
	        14, 17, 11, 24,  1,  5,  3, 28, 15,  6, 21, 10,
	        23, 19, 12,  4, 26,  8, 16,  7, 27, 20, 13,  2,
	        41, 52, 31, 37, 47, 55, 30, 40, 51, 45, 33, 48,
	        44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32
        };
        static byte[] ShiftsSequence =
        {
	        1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1
        };
        
        public ulong Key
        {
	        set
	        {
		        if (value > Mask56Bit)
		        {
			        throw new ArgumentException("Key");
		        }
		        RoundKeys = new ulong[16];
		        ulong PermutedKey = Task_1.Program.Permute(value, KeyPermutation);
		        ulong C = (ulong)((PermutedKey >> 28) & Mask28Bit), D = (ulong)(PermutedKey & Mask28Bit);
		        for (byte Round = 0, Shift; Round < 16; Round++)
		        {
			        Shift = ShiftsSequence[Round];
			        C = ((C << Shift) | (C >> (28 - Shift))) & Mask28Bit;
			        D = ((D << Shift) | (D >> (28 - Shift))) & Mask28Bit;
			        RoundKeys[Round] = Task_1.Program.Permute((C << 28) | D, RoundKeyPermutation);
		        }
	        }
        }
    }
}