using System.Text.RegularExpressions;

namespace BigIntegerOperations
{
    internal class BigInt
    {
        const int blockLength = 9;
        static int w, k;// w = nr of bytes in each block of length blockLength, k = 2^(max number of bits in block)
        static BigInt mu;

        public bool Sign { get; set; }
        public byte[] Value { get; set; }

        public static BigInt One = new("1");
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

            SetModuloConstants();
        }

        public BigInt(bool Sign, byte[] Value)
        {
            this.Sign = Sign;
            this.Value = Value;

            SetModuloConstants();
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
                    while (X[k] == Y[k])
                    {
                        k++;
                    }
                    if (X[k] > Y[k])
                    {
                        C.Sign = A.Sign;
                    }
                    else
                    {
                        C.Sign = B.Sign;
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
            if (!A.Sign && B.Sign || A.Value.Length > B.Value.Length)
            {
                return 1;
            }

            if (A.Sign && !B.Sign || A.Value.Length < B.Value.Length)
            {
                return -1;
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

            int index = 0;
            while (index < A.Value.Length)
            {
                R = R * Ten + new BigInt(A.Value[index].ToString());

                BigInt div = Zero;
                while (R >= B)
                {
                    R = R - B;
                    div += One;
                }

                C = C * Ten + div;
                index++;
            }

            return C;
        }

        //eventually test with BigInteger.Divide
        //public static BigInt operator %(BigInt A, BigInt B)
        //{
        //    mu = new BigInt(k.ToString()) / B;

        //    for (int i = 0; i < A.Value.Length; i += blockLength)
        //    {
        //        for (int j = i; j < i + blockLength && j < A.Value.Length; j++)
        //        {

        //        }
        //    }
        //}

        private void SetModuloConstants()
        {
            int bitsRequiredInBlock = 4 * blockLength;//4 = nr bits in decimal digit
            w = (bitsRequiredInBlock + 7) / 8;
            k = (int)Math.Pow(2, w * 8);
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
