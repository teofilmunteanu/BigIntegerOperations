using BigIntegerOperations;

BigInt x = new BigInt("328732738245");
BigInt y = new BigInt("-747838389");
BigInt z = x + y;
z.Value.ToList().ForEach(x => Console.WriteLine(x));
Console.WriteLine(z.Sign);