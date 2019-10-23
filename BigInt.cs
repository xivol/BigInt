using System;
using System.Collections.Generic;
using System.Text;

namespace BigInt
{
    public partial class BigInt
    {
        private List<uint> data;
        //private int sign;
        public const uint Base = 1000000;

        public BigInt()
        {
            data = new List<uint> { 0 };
            //sign = 0;
        }

        public BigInt(long number)
        {
            data = new List<uint>();
            //sign = Math.Sign(number);
            //number = Math.Abs(number);
            while (number > 0)
            {
                data.Add((uint)(number % Base));
                number /= Base;
            }
        }

        public BigInt(string number)
        {
            number = number.Trim();
            //if (number[0] == '-')
            //{
            //    sign = -1;
            //    number.Remove(0, 1);
            //}
            //else if (number[0] != '0')
            //    sign = 1;
            //else
            //    sign = 0;

            int baseLen = (int)Math.Round(Math.Log10(Base));
            data = new List<uint>();
            if (number.Length <= baseLen)
            {
                data.Add(uint.Parse(number));
            }
            else
            {
                int i = number.Length - baseLen;
                while (i >= 0)
                {
                    uint value = uint.Parse(number.Substring(i, baseLen));
                    if (value > Base)
                        throw new NotImplementedException("WHAT?!");
                    data.Add(value);
                    i -= baseLen;
                }
                if (i < 0)
                {
                    uint value = uint.Parse(number.Substring(0, i + baseLen));
                    data.Add(value);
                }
            }
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            int baseLen = (int)Math.Round(Math.Log10(Base));
            for (int i = 0; i < data.Count - 1; ++i)
            {
                result.Insert(0, data[i].ToString("D" + baseLen));
            }
            result.Insert(0, data[data.Count - 1]);
            return result.ToString();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(data, Length, IsZero);
        }

        public uint Length
        {
            get
            {
                if (IsZero) return 0;
                if (data[data.Count - 1] == 1)
                    return (uint)(data.Count - 1) * (uint)Math.Round(Math.Log10(Base)) + 1;

                return (uint)(data.Count - 1) * (uint)Math.Round(Math.Log10(Base)) +
                    (uint)Math.Ceiling(Math.Log10(data[data.Count - 1]));
            }
        }

        public byte this[int index]
        {
            get {
                if (index >= Length)
                    throw new IndexOutOfRangeException(index.ToString());

                int baseLen = (int)Math.Round(Math.Log10(Base));
                int sector = index / baseLen;
                int digit = index % baseLen;

                uint value = data[sector];
                byte result = (byte)(value % 10);
                while (value > 0 && digit > 0)
                {
                    value /= 10;
                    result = (byte)(value % 10);
                    digit -= 1;
                }

                return result;
            }
        }

        public bool IsZero
        {
            get { return data[0] == 0 && data.Count == 1; }
        }
    }
}
