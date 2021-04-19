using System;
using System.Numerics;

namespace Task_4
{
    public abstract class FeistelNet
    {
        protected ulong[] RoundKeys;
        //public abstract ulong[] generateRoundKeys();//RoundKeys = new ulong[16];
        
        
        //const ulong Mask32Bit = ((ulong)1 << 32) - 1;

        public int FeistelRoundQuantity;
        
        protected int DataSize;
        protected ulong Data;

        private int byteSize = 8;

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
	        DataSize = DataBytes.Length;
	        if (DataBytes.Length == 8)
	        {
		        Data = BitConverter.ToUInt64(DataBytes, 0);
	        }
	        else
	        {
		        Data = BitConverter.ToUInt64(DataBytes, 0);
		        Data <<= 64;
		        Data |= BitConverter.ToUInt64(DataBytes, 8);
		        foreach (var VARIABLE in BitConverter.GetBytes(Data))
		        {
			        Console.Write(" " + VARIABLE);
		        }
	        }
        }

        protected void DoTheJob(bool encrypt)
        {
	        ulong mask = ((ulong)1 << (DataSize*byteSize)/2) - 1;
	        ulong L = (ulong)((Data >> (DataSize*byteSize)/2) & mask), PreviousL = L;
	        ulong R = (ulong)(Data & mask), PreviousR = R;
	        for (byte Round = 0; Round < FeistelRoundQuantity; Round++)
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
			        L = PreviousR ^ AbstractFeistelFunction(PreviousL, RoundKeys[FeistelRoundQuantity-1 - Round]);
			        PreviousR = R;
			        PreviousL = L;
		        }
	        }
	        //return BitConverter.GetBytes(Permute(((ulong)L << 32) | R, FinalPermutation));
	        Data = ((ulong) L << (DataSize*byteSize)/2) | R;
        }

        protected abstract ulong AbstractFeistelFunction(ulong R, ulong RoundKey);
        
        protected virtual void Hook1() { }

        protected virtual void Hook2() { }
        
        public abstract  ulong Key
        {
	        set;
        }
    }
}