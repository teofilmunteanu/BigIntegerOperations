using System.Text.RegularExpressions;

namespace BigIntegerOperations
{
    internal class BigInt
    {
        public bool Sign { get; set; }
        public byte[] Value { get; set; }

        public static BigInt One = new("1");
        public static BigInt Two = new("2");
        public static BigInt Zero = new("0");
        public static BigInt Ten = new("10");

        public BigInt(string? input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            input = Regex.Replace(input, @"\s+", string.Empty);

            if (input.Length == 0) throw new ArgumentNullException("Input can't be empty!");


            if (input.StartsWith("-"))
            {
                Sign = true;
                input = input.Substring(1);
            }

            if (Regex.IsMatch(input, "[^0-9]+"))
            {
                throw new ArgumentException("Input can only contain 0-9 digits and the \"-\" sign at the beginning!");
            }


            input = string.Join("", input.SkipWhile(x => x == '0'));
            if (input.Length == 0) input = "0";

            Value = new byte[input.Length - 1];
            Value = input.Select(x => (byte)(x - '0')).ToArray();
        }

        public BigInt(bool Sign, byte[] Value)
        {
            this.Sign = Sign;
            this.Value = Value;
        }

        public static BigInt operator +(BigInt A, BigInt B)
        {
            if (A.Value[0] == 0)
                return B;
            if (B.Value[0] == 0)
                return A;

            int lenght = A.Value.Length < B.Value.Length ? A.Value.Length : B.Value.Length;
            int L = A.Value.Length > B.Value.Length ? A.Value.Length : B.Value.Length;
            byte[] X = new byte[L];
            byte[] Y = new byte[L];
            A.Value.CopyTo(X, L - A.Value.Length);
            B.Value.CopyTo(Y, L - B.Value.Length);

            BigInt C = new BigInt(false, new byte[L + 1]);


            byte carry = 0;
            if (A.Sign == B.Sign)
            {
                C.Sign = A.Sign;
                for (int i = L - 1; i >= 0; i--)
                {
                    byte digit = (byte)(X[i] + Y[i] + carry);
                    if (digit > 9)
                    {
                        carry = (byte)(digit / 10);
                        digit -= 10;
                    }
                    else { carry = 0; }
                    C.Value[i + 1] = digit;
                }
                if (carry != 0)
                {
                    C.Value[0] = carry;

                }
                else
                {
                    byte[] vect = new byte[L];
                    Array.Copy(C.Value, 1, vect, 0, L);
                    if (vect.Length == 0) vect[0] = 0;
                    C = new BigInt(C.Sign, vect);
                }

            }
            else
            {
                C = new BigInt(false, new byte[L]);
                if (A.Value.Length > B.Value.Length)
                {
                    C.Sign = A.Sign;
                }
                else if (A.Value.Length < B.Value.Length)
                {
                    C.Sign = B.Sign;
                }
                else
                {
                    int k = 0;

                    while (k < X.Length && X[k] == Y[k])
                    {
                        k++;
                    }
                    if (k < X.Length)
                    {
                        if (X[k] > Y[k])
                        {
                            C.Sign = A.Sign;
                        }
                        else
                        {
                            C.Sign = B.Sign;
                        }
                    }
                    else
                    {
                        byte[] zero = new byte[1];
                        zero[0] = 0;
                        return new BigInt(false, zero);
                    }
                }

                if (A.Value.Length < B.Value.Length)
                {
                    byte[] Aux = X;
                    X = Y;
                    Y = Aux;
                }
                else if (A.Value.Length == B.Value.Length)
                {

                    int k = 0;
                    while (X[k] == Y[k])
                    {
                        k++;
                    }
                    if (X[k] < Y[k])
                    {
                        byte[] Aux = X;
                        X = Y;
                        Y = Aux;
                    }

                }

                for (int i = L - 1; i >= 0; i--)
                {
                    if (X[i] >= Y[i])
                    {
                        C.Value[i] = (byte)(X[i] - Y[i]);
                    }
                    else
                    {
                        int j = i - 1;
                        while (X[j] == 0)
                        {
                            X[j] = 9;
                            j--;
                        }
                        X[j]--;
                        X[i] += 10;
                        i++;
                    }
                }
                int m = 0;
                while (m < C.Value.Length && C.Value[m] == 0)
                {
                    m++;
                }
                //Console.WriteLine("m = " + m);
                byte[] vect = new byte[C.Value.Length];
                Array.Copy(C.Value, m, vect, 0, C.Value.Length - m);
                Array.Resize(ref vect, C.Value.Length - m);
                if (vect.Length == 0)
                {
                    vect = new byte[1];
                    vect[0] = 0;
                }
                C = new BigInt(C.Sign, vect);
            }
            return C;
        }

        public static BigInt operator *(BigInt A, BigInt B)
        {
            BigInt C = new BigInt(A.Sign || B.Sign, new byte[A.Value.Length + B.Value.Length]);
            if (A.Sign == true && B.Sign == true) { C.Sign = false; }
            for (int i = A.Value.Length - 1; i >= 0; i--)
            {
                byte carry = 0;
                for (int j = B.Value.Length - 1; j >= 0; j--)
                {
                    byte temp = (byte)(A.Value[i] * B.Value[j] + C.Value[i + j + 1] + carry);
                    C.Value[i + j + 1] = (byte)(temp % 10);
                    carry = (byte)(temp / 10);
                }

                C.Value[i] += carry;
            }

            int k = 0;
            while (k < C.Value.Length && C.Value[k] == 0)
            {
                k++;
            }
            byte[] vect = new byte[C.Value.Length];
            vect = C.Value;
            Array.Copy(vect, k, vect, 0, C.Value.Length - k);
            Array.Resize(ref vect, C.Value.Length - k);
            if (vect.Length == 0)
            {
                vect = new byte[1];
                vect[0] = 0;
            }
            return new BigInt(C.Sign, vect);
        }

        ///<summary>
        ///Returns  0 if equal, -1 if A less than B, 1 if A greater than B
        ///</summary> 
        static int Compare(BigInt A, BigInt B)
        {
            if (!A.Sign && B.Sign)
            {
                return 1;
            }
            if (A.Sign && !B.Sign)
            {
                return -1;
            }

            if (A.Value.Length > B.Value.Length)
            {
                return A.Sign ? -1 : 1;
            }
            if (A.Value.Length < B.Value.Length)
            {
                return A.Sign ? 1 : -1;
            }


            for (int i = 0; i < A.Value.Length; i++)
            {
                if (A.Value[i] > B.Value[i])
                {
                    return A.Sign ? -1 : 1;
                }
                if (A.Value[i] < B.Value[i])
                {
                    return A.Sign ? 1 : -1;
                }
            }

            return 0;
        }

        public static bool operator ==(BigInt A, BigInt B)
        {
            return Compare(A, B) == 0;
        }
        public static bool operator !=(BigInt A, BigInt B)
        {
            return Compare(A, B) != 0;
        }

        public static bool operator <(BigInt A, BigInt B)
        {
            return Compare(A, B) == -1;
        }
        public static bool operator <=(BigInt A, BigInt B)
        {
            int comparison = Compare(A, B);
            return comparison == 0 || comparison == -1;
        }
        public static bool operator >(BigInt A, BigInt B)
        {
            return !(A <= B);
        }
        public static bool operator >=(BigInt A, BigInt B)
        {
            return !(A < B);
        }

        public static BigInt operator -(BigInt A, BigInt B)
        {
            BigInt Aux = new BigInt(B.Sign, B.Value);
            Aux.Sign = !Aux.Sign;
            return A + Aux;
        }

        public static BigInt operator /(BigInt A, BigInt B)
        {
            BigInt C = Zero;
            BigInt R = Zero;

            BigInt B1 = new BigInt(false, B.Value);

            int index = 0;
            while (index < A.Value.Length)
            {
                R = R * Ten + new BigInt(A.Value[index].ToString());

                BigInt div = Zero;

                while (R >= B1)
                {
                    R = R - B1;
                    div += One;
                }

                C = C * Ten + div;
                index++;
            }

            if (A.Sign != B.Sign)
            {
                return new BigInt(true, C.Value);
            }

            return C;
        }

        public static BigInt operator %(BigInt A, BigInt B)
        {
            if (B.Sign)
            {
                throw new ArithmeticException("Second argument must be non-negative");
            }

            BigInt R = Zero;

            int index = 0;

            while (index < A.Value.Length)
            {
                if (A.Sign)
                {
                    R = R * Ten - new BigInt(A.Value[index].ToString());
                }
                else
                {
                    R = R * Ten + new BigInt(A.Value[index].ToString());
                }

                if (A.Sign)
                {
                    while (R <= new BigInt(true, B.Value))
                    {
                        R = R + B;
                    }
                }
                else
                {
                    while (R >= B)
                    {
                        R = R - B;
                    }
                }

                index++;
            }

            if (R < Zero)
            {
                R = R + B;
            }

            return R;
        }

        public static BigInt PowModN(BigInt A, BigInt b, BigInt n)
        {

            if (n == Zero) throw new ArgumentException("n should be >= 0");

            if (b == Zero) return One;
            BigInt I = A;
            for (BigInt i = One; i < b; i += One)
            {
                A = A * I;
                A = A % n;
            }
            return A % n;
        }

        //public static BigInt Invers(BigInt A,BigInt mod)
        //{
        //    if (mod == Zero) throw new ArgumentException("n should be >= 0");
        //    if (A == Zero) throw new ArgumentException("Zero has no inverse in mod n");
        //    //if(BigInt.cmmdc(A, mod) == 1)
        //        for(BigInt i = One; i < mod; i += One)
        //        {
        //            if (((A % mod) * (i % mod)) % mod == One)
        //                return i;
        //        }
        //    //else throw new ArgumentException("numbers cannnot have cmmdc != 1")
        //}

        public static BigInt cmmdc(BigInt A, BigInt B)
        {
            BigInt A1 = new BigInt(A.Sign, A.Value);
            BigInt B1 = new BigInt(B.Sign, B.Value);

            if (A == Zero || B == Zero)
            {
                return A + B;
            }

            BigInt R = A1 % B1;

            while (R != Zero)
            {
                R = A1 % B1;
                A1 = B1;
                B1 = R;
            }

            return A1;
        }

        public static BigInt Inverse(BigInt A, BigInt mod)
        {
            if (mod == Zero) throw new ArgumentException("n should be >= 0");
            if (A == Zero) throw new ArgumentException("Zero has no inverse in mod n");

            if (cmmdc(A, mod) == One)
            {
                for (BigInt i = One; i < mod; i += One)
                {
                    if (((A % mod) * (i % mod)) % mod == One)
                        return i;
                }

                throw new ArgumentException("No inverse");
            }
            else
            {
                throw new ArgumentException("numbers cannnot have cmmdc != 1");
            }
        }

        public static BigInt Opposite(BigInt A, BigInt mod)
        {
            if (A == Zero)
                return Zero;

            return (mod - A) % mod;
        }

        public static BigInt Pow(BigInt A, int b)
        {
            if (b == 0) return One;
            BigInt I = A;
            for (int i = 1; i < b; i++)
            {
                A = A * I;
            }

            return A;
        }

        public static BigInt operator <<(BigInt A, int shift)
        {
            return A * Pow(Two, shift);
        }

        public static BigInt operator >>(BigInt A, int shift)
        {
            return A / Pow(Two, shift);
        }

        public static BigInt SqrtModN(BigInt A, BigInt mod)
        {
            if (A < Zero)
            {
                throw new ArithmeticException("Number must be non-negative.");
            }

            if (A < Two)
            {
                return A;
            }

            BigInt A1 = new BigInt(false, A.Value);

            BigInt root = Zero;
            BigInt maxBit = One << 254;

            while (maxBit > A1)
            {
                maxBit >>= 2;
            }

            while (maxBit != Zero)
            {
                if (A1 >= root + maxBit)
                {
                    A1 -= root + maxBit;
                    root = (root >> 1) + maxBit;
                }
                else
                {
                    root >>= 1;
                }

                maxBit >>= 2;
            }

            return root % mod;
        }

        public void Show()
        {
            String s = new String("");
            if (this.Sign == true)
            {
                s += "-";
            }
            for (int i = 0; i < this.Value.Length; i++)
            {
                s += this.Value[i];
            }
            Console.Write(s);
        }

    }
}
