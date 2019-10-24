using System;
using System.Collections.Generic;
namespace BigInt
{
    public partial class BigInt
    {
        public BigInt Sqrt()
        {
            BigInt root1 = this;
            BigInt root2 = (root1 + this / root1) / 2;

            while (root2 != root1 && root2 - 1 != root1)
            {
                root1 = root2;
                root2 = (root1 + this / root1) / 2;
            }
            return root1 == root2 ? root1 : root2;
        }

        public List<BigInt> Factors()
        {
            List<BigInt> fact = new List<BigInt>();
            BigInt prime = 2;
            
            BigInt remainder;
            BigInt divided = this;
            BigInt quotent = divided.Div(prime, out remainder);
            while (remainder.IsZero)
            {
                divided = quotent;
                fact.Add(prime);
                quotent = divided.Div(prime, out remainder);
            }
            prime += 1;

            while (prime <= divided)
            {
                quotent = divided.Div(prime, out remainder);
                while (remainder.IsZero)
                {
                    divided = quotent;
                    fact.Add(prime);
                    quotent = divided.Div(prime, out remainder);
                }
                prime += 2;
            }
            return fact;
        }
    }
}
