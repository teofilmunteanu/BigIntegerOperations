using BigIntegerOperations;

BigInt x = new BigInt(" -00fdsfd00000\t\n4fdsf334");
x.Value.ToList().ForEach(x => Console.WriteLine(x));
Console.WriteLine(x.Sign);