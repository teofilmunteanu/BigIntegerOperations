using BigIntegerOperations;

BigInt x = new BigInt("-121");
BigInt y = new BigInt("200");
BigInt z = x + y;
z.Value.ToList().ForEach(x => Console.WriteLine(x));
Console.WriteLine(z.Sign);