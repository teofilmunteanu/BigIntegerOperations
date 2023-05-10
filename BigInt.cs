using System.Text.RegularExpressions;

namespace BigIntegerOperations
{
    internal class BigInt
    {
        public bool Sign { get; set; }
        public byte[] Value { get; set; }

        public BigInt(string? input)
        {
            if (input == null || input.Length == 0) throw new ArgumentNullException("Invalid input");

            input = Regex.Replace(input, @"\s+|[^0-9-]+", string.Empty);

            if (input.StartsWith("-"))
            {
                Sign = true;
                input = input.Substring(1);
            }
            input = string.Join("", input.SkipWhile(x => x == '0'));

            Value = new byte[input.Length - 1];
            Value = input.Select(x => (byte)(x - '0')).ToArray();
        }
    }
}
