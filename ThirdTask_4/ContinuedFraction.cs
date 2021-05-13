using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ThirdTask_4
{
    class ContinuedFraction
    {
        private List<BigInteger> quotients = new List<BigInteger>();

        private ContinuedFraction(List<BigInteger> quotients)
        {
            this.quotients = quotients;
        }
        public ContinuedFraction(BigInteger up, BigInteger down)
        {
            BigInteger a = up / down;
            quotients.Add(a);
            while (a * down != up)
            {
                BigInteger tmp = up - a * down;
                up = down;
                down = tmp;
                a = up / down;
                quotients.Add(a);
            }
        }

        public Tuple<BigInteger, BigInteger> GetRational()
        {
            BigInteger up;
            BigInteger down;

            if (quotients.Count == 0)
            {
                up = 0;
                down = 1;
                return new Tuple<BigInteger, BigInteger>(up, down);
            }

            BigInteger last = quotients[quotients.Count - 1];
            BigInteger denom = 1;

            for (int i = quotients.Count - 2; i >= 0; i--)
            {
                BigInteger tmp = quotients[i] * last + denom;
                denom = last;
                last = tmp;
            }

            up = last;
            down = denom;

            return new Tuple<BigInteger, BigInteger>(up, down);
        }

        public List<Tuple<BigInteger, BigInteger>> GetConvergents()
        {
            List<Tuple<BigInteger, BigInteger>> result = new List<Tuple<BigInteger, BigInteger>>();
            for (int i = 1; i < quotients.Count; i++)
            {

                result.Add(new ContinuedFraction(quotients.GetRange(0, i)).GetRational());
            }

            return result;
        }
    }
}
