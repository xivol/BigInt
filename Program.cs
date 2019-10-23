using System;

namespace BigInt
{
    class Program
    {
        static void Main(string[] args)
        {
            BigInt i = new BigInt(10000000);
            BigInt j = new BigInt(999999999);
            BigInt k = new BigInt("1234567890123456798");

            Console.WriteLine(i);
            Console.WriteLine(j);
            Console.WriteLine(k);
            
            Console.WriteLine(k + j);
            Console.WriteLine(j * j);
            Console.WriteLine(j / 999);
            Console.WriteLine(1000 % i);

            Console.WriteLine(k / j);
            Console.WriteLine(k % j);
            Console.WriteLine((k / j) * j + (k % j) == k);
        }
    }
}
