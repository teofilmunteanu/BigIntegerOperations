using BigIntegerOperations;

BigInt x = new BigInt("245");
BigInt y = new BigInt("-9");
BigInt z = x + y;
z.Value.ToList().ForEach(x => Console.WriteLine(x));
Console.WriteLine(z.Sign);