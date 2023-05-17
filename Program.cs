﻿
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

BigInt x = new BigInt(xs);
BigInt y = new BigInt(ys);

Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();

BigInt z = x % y;
z.Show();

stopwatch.Stop();
Console.WriteLine("\nElapsed Time: " + stopwatch.ElapsedMilliseconds);
