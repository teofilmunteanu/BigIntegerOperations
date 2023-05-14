using BigIntegerOperations;

BigInt x = new BigInt("00000000000");
x.Value.ToList().ForEach(x => Console.WriteLine(x));
Console.WriteLine(x.Sign);