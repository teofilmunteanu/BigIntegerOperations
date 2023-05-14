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

            Value = new byte[input.Length - 1];
            Value = input.Select(x => (byte)(x - '0')).ToArray();
        }
        //public BigInt(bool Sign, byte[] Value)
        //{
        //    this.Sign = Sign;
        //    this.Value = Value;
        //}

        //public static BigInt operator +(BigInt A, BigInt B)
        //{
        //    BigInt C = new BigInt("0");
        //    if (A.Sign == B.Sign)
        //    {
        //        C.Sign = A.Sign;
        //    }
        //    else
        //    {

        //    }
        //    return C;
        //}

    }
}
