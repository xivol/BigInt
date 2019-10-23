using System;
using System.Collections.Generic;
namespace BigInt
{
    public partial class BigInt : IComparable<BigInt>, IEquatable<BigInt>
    {
        private void TrimZeros()
        {
            while (data.Count > 1 && data[data.Count - 1] == 0)
                data.RemoveAt(data.Count - 1);
        }

        private BigInt Add(BigInt other)
        {
            BigInt result = new BigInt();
            result.data.Clear();
            BigInt min, max;
            if (this.data.Count < other.data.Count)
            {
                min = this;
                max = other;
            }
            else
            {
                min = other;
                max = this;
            }

            uint carry = 0;

            for (int i = 0; i < min.data.Count; i++)
            {
                uint sum = carry + min.data[i] + max.data[i];
                result.data.Add(sum % Base);
                carry = sum / Base;
            }

            for (int i = min.data.Count; i < max.data.Count; i++)
            {
                result.data.Add(carry + max.data[i]);
                carry = 0;
            }

            if (carry > 0)
                result.data.Add(carry);
            return result;
        }


        private BigInt Sub(BigInt other)
        {

            BigInt result = new BigInt();
            result.data.Clear();
            uint carry = 0;

            BigInt min, max;
            if (this.data.Count < other.data.Count)
            {
                throw new NotImplementedException();
                //min = this;
                //max = other;
                //result.sign = -1;
            }
            else
            {
                min = other;
                max = this;
                //result.sign = 1;
            }

            for (int i = 0; i < min.data.Count; i++)
            {
                int sum = (int)this.data[i] - (int)(carry + other.data[i]);
                if (sum < 0)
                {
                    result.data.Add((uint)(sum + (int)Base));
                    carry = 1;
                }
                else
                {
                    result.data.Add((uint)sum);
                    carry = 0;
                }
            }

            for (int i = min.data.Count; i < max.data.Count; i++)
            {
                uint sum = this.data[i] - carry;
                result.data.Add(sum);
                carry = 0;
            }
            if (carry > 0)
                result.data.Add(carry);

            result.TrimZeros();
            return result;
        }

        private BigInt Mul(int other)
        {
            BigInt result = new BigInt();
            result.data.Clear();
            ulong carry = 0;
            uint uOther = (uint)other;
            for (int i = 0; i < data.Count; i++)
            {
                ulong prod = (ulong)this.data[i] * uOther + carry;
                result.data.Add((uint)(prod % Base));
                carry = prod / Base;

            }
            if (carry > 0)
            {
                result.data.Add((uint)carry);
            }

            result.TrimZeros();
            return result;
        }

        private BigInt Mul(BigInt other)
        {
            BigInt result = new BigInt();
            result.data = new List<uint>(new uint[data.Count + other.data.Count]);
            
            for (int i = 0; i < data.Count; i++)
            {
                ulong carry = 0;
                for (int j = 0; j < other.data.Count; j++)
                {
                    ulong prod = result.data[i+j] +
                        (ulong)data[i] * other.data[j] +
                        carry;
                    result.data[i+j] = (uint)(prod % Base);
                    carry = prod / Base;
                }
                if (carry > 0)
                {
                    result.data[i + other.data.Count] += (uint)carry;
                }

            }

            result.TrimZeros();
            return result;
        }

        private BigInt Div(int other, out int modulus)
        {
            if (other == 0)
                throw new DivideByZeroException();
            if (other < 0)
                throw new NotImplementedException();
            BigInt result = new BigInt();
            result.data.Clear();
            ulong carry = 0;
            uint uOther = (uint)other;
            for (int i = data.Count - 1; i >=0; i--)
            {
                ulong cur = data[i] + carry * Base;
                result.data.Insert(0, (uint)(cur / uOther));
                carry = cur % uOther;
            }
            modulus = (int)carry;

            result.TrimZeros();
            return result;
        }

        private BigInt Div(BigInt other, out BigInt modulus)
        {
            if (other.IsZero)
                throw new DivideByZeroException();
            if (other.Length > this.Length)
            {
                modulus = this;
                return new BigInt();
            }

            BigInt result = new BigInt();
            BigInt divided = new BigInt();
            int i = (int)this.Length - 1;
            while (i >= 0)
            {
                while (i >= 0 && divided <= other)
                {
                    divided = (divided * 10) + this[i];
                    i -= 1;
                }

                int q = 1;
                BigInt prod = other;
                while (q < 10 && prod <= divided)
                {
                    q += 1;
                    prod = other + prod;
                }
                q -= 1;
                result = result * 10 + q;
                divided = divided - (other * q);
            }
            modulus = divided;
            return result;
        }

        public int CompareTo(BigInt other)
        {
            if (this.Length < other.Length)
                return -1;
            if (this.Length > other.Length)
                return 1;
            for (int i = data.Count - 1; i >= 0; i--)
            {
                if (data[i] < other.data[i])
                    return -1;
                if (data[i] > other.data[i])
                    return 1;
            }
            return 0;
        }

        public bool Equals(BigInt other)
        {
            return this.CompareTo(other) == 0;
        }

        public static implicit operator BigInt(long value)
        {
            return new BigInt(value);
        }

        public static BigInt operator +(BigInt lhv, BigInt rhv)
        {
            return lhv.Add(rhv);
        }

        public static BigInt operator -(BigInt lhv, BigInt rhv)
        {
            return lhv.Sub(rhv);
        }

        public static BigInt operator *(BigInt lhv, int rhv)
        {
            return lhv.Mul(rhv);
        }

        public static BigInt operator *(BigInt lhv, BigInt rhv)
        {
            return lhv.Mul(rhv);
        }

        public static BigInt operator /(BigInt lhv, int rhv)
        {
            int modulus;
            return lhv.Div(rhv, out modulus);
        }

        public static BigInt operator /(BigInt lhv, BigInt rhv)
        {
            BigInt modulus;
            return lhv.Div(rhv, out modulus);
        }

        public static int operator %(BigInt lhv, int rhv)
        {
            int modulus;
            lhv.Div(rhv, out modulus);
            return modulus;
        }

        public static BigInt operator %(BigInt lhv, BigInt rhv)
        {
            BigInt modulus;
            lhv.Div(rhv, out modulus);
            return modulus;
        }

        public static bool operator ==(BigInt lhv, BigInt rhv)
        {
            return lhv.CompareTo(rhv) == 0;            
        }

        public static bool operator !=(BigInt lhv, BigInt rhv)
        {
            return lhv.CompareTo(rhv) != 0;
        }

        public static bool operator <(BigInt lhv, BigInt rhv)
        {
            return lhv.CompareTo(rhv) == -1;
        }

        public static bool operator <=(BigInt lhv, BigInt rhv)
        {
            return lhv.CompareTo(rhv) <= 0;
        }

        public static bool operator >(BigInt lhv, BigInt rhv)
        {
            return lhv.CompareTo(rhv) == 1;
        }

        public static bool operator >=(BigInt lhv, BigInt rhv)
        {
            return lhv.CompareTo(rhv) >= 0;
        }
    }
}
