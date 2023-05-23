
using BigIntegerOperations;
using System.Diagnostics;

//Generate 78 digits nr (2^264 has 78 digits)
string xs = "";
int xDigits = new Random().Next(70, 79);
for (int i = 0; i < xDigits; i++)
{
    xs += new Random().Next(1, 10);
}

string ys = "";
int yDigits = new Random().Next(65, xDigits);
for (int i = 0; i < yDigits; i++)
{
    ys += new Random().Next(1, 10);
}

BigInt x0 = new BigInt(xs);
BigInt y0 = new BigInt(ys);
BigInt m0 = y0;
BigInt x1 = new BigInt("-453545");
BigInt y1 = new BigInt("12214");
BigInt m1 = new BigInt("30");
BigInt x3 = new BigInt("2345234564356456");
BigInt m3 = new BigInt("234523456435645");

BigInt x4 = new BigInt(xs);
BigInt y4 = new BigInt("23452");
BigInt m4 = new BigInt(ys);

Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();

ShowOperationExample("addition", x0, y0, null);
ShowOperationExample("subtraction", x0, y0, null);
ShowOperationExample("modulo", x0, y0, null);
ShowOperationExample("sqrtMod", x0, null, y0);
ShowOperationExample("powMod", x4, y4, m4);
ShowOperationExample("cmmdc", x0, y0, null);
ShowOperationExample("inverse", x3, null, m3);
ShowOperationExample("opposite", x0, null, m0);

stopwatch.Stop();
Console.WriteLine("\nElapsed Time: " + stopwatch.ElapsedMilliseconds);


void ShowOperationExample(string operation, BigInt x, BigInt? y, BigInt? m)
{
    Console.Write("x " + operation + (y is not null ? " y " : "") + (m is not null ? " mod m " : ""));
    Console.Write("\nx = ");
    x.Show();
    if (y is not null)
    {
        Console.Write("\ny = "); y.Show();
    }
    if (m is not null)
    {
        Console.Write("\nm = "); m.Show();
    }

    Console.WriteLine();
    BigInt z = BigInt.Zero;
    switch (operation)
    {
        case "addition":
            z = x + y;
            break;
        case "subtraction":
            z = x - y;
            break;
        case "modulo":
            z = x % y;
            break;
        case "sqrtMod":
            z = BigInt.SqrtModN(x, m);
            break;
        case "powMod":
            z = BigInt.PowModN(x, y, m);
            break;
        case "cmmdc":
            z = BigInt.cmmdc(x, y);
            break;
        case "inverse":
            z = BigInt.Inverse(x, m);
            break;
        case "opposite":
            z = BigInt.Opposite(x, m);
            break;
        default:
            Console.WriteLine("invalid operation\n");
            break;
    }
    Console.Write("res: ");
    z.Show();
    Console.WriteLine("\n");
}