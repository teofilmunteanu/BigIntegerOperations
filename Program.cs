
using BigIntegerOperations;
using System.Diagnostics;

//Generate 78 digits nr (2^264 has 78 digits)
string xs = "";
for (int i = 0; i < 78; i++)
{
    xs += new Random().Next(5, 10);
}

string ys = "";
for (int i = 0; i < 78; i++)
{
    ys += new Random().Next(1, 4);
}

Console.WriteLine(xs + "\n" + ys + "\n");


BigInt x = new BigInt(xs);
BigInt y = new BigInt(ys);
//BigInt z = x + y;
//z.Show();

Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();

BigInt z = x / y;
z.Show();

stopwatch.Stop();
Console.WriteLine("\nElapsed Time: " + stopwatch.ElapsedMilliseconds);
