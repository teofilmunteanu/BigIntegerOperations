
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

Console.WriteLine(xs + "\n" + ys + "\n");

BigInt x = new BigInt("-123");
BigInt y = new BigInt("5");
BigInt n = new BigInt("15");
Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();

//BigInt z = BigInt.PowModN(x, y, n);
BigInt z = x % y;
z.Show(); Console.WriteLine();

//string xSign = x.Sign ? "-" : "";
//string ySign = y.Sign ? "-" : "";
//Console.WriteLine(xSign);
//x.Show();
//Console.Write("<");
//y.Show();
//Console.Write(" => " + (x < y));

stopwatch.Stop();
Console.WriteLine("\nElapsed Time: " + stopwatch.ElapsedMilliseconds);
