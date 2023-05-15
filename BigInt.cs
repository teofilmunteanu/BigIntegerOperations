using System.Text.RegularExpressions;

namespace BigIntegerOperations
{
    internal class BigInt
    {
        public bool Sign { get; set; }
        public byte[] Value { get; set; }

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

            BigInt C = new BigInt(false, new byte[L+1]);


            byte carry = 0;
            if (A.Sign == B.Sign)
            {
                C.Sign = A.Sign;
                for(int i = L - 1  ;i >= 0; i--)
                {
                    byte digit = (byte)(X[i] + Y[i] + carry);
                    if(digit > 9)
                    {
                        carry = (byte)(digit/10);
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
                else if(A.Value.Length == B.Value.Length)
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

                for(int i = L - 1 ; i >= 0 ; i--)
                {
                    if (X[i] >= Y[i])
                    {
                        C.Value[i] = (byte)(X[i] - Y[i]);
                    }
                    else
                    {
                        int j = i-1;
                        while (X[j] == 0)
                        {
                            X[j] = 9;
                            j--;
                        }
                        X[j] --;
                        X[i] += 10;
                        i++;
                    }
                }
            }
            return C;
        }

        public static BigInt operator *(BigInt A, BigInt B)
        {
            BigInt C = new BigInt(A.Sign || B.Sign,new byte[A.Value.Length + B.Value.Length]);
            if(A.Sign == true && B.Sign == true) { C.Sign = false; }
            for(int i = A.Value.Length - 1; i >= 0; i--)
            {
                byte carry = 0;
                for (int j = B.Value.Length - 1; j >= 0; j--)
                {
                    byte temp =(byte)( A.Value[i] * B.Value[j] + C.Value[i+j+1] + carry);
                    C.Value[i+j+1] = (byte)(temp % 10);
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
            Array.Copy(vect, k  ,vect, 0,C.Value.Length - k);
            Array.Resize(ref vect, C.Value.Length - k);
            if(vect.Length == 0)
            {
                vect = new byte[1];
                vect[0] = 0;
            }
            return new BigInt(C.Sign,vect);
        }

    }
}
