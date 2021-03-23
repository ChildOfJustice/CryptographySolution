using System;

namespace Task_4
{
    public abstract class FeistelNet
    {
        protected ulong[] RoundKeys;
        //public abstract ulong[] generateRoundKeys();//RoundKeys = new ulong[16];
        
        
        const ulong Mask32Bit = ((ulong)1 << 32) - 1;
        
        
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
        
        public abstract  ulong Key
        {
	        set;
        }
    }
}