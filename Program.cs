using BigIntegerOperations;

BigInt x = new BigInt("");
x.Value.ToList().ForEach(x => Console.WriteLine(x));
Console.WriteLine(x.Sign);