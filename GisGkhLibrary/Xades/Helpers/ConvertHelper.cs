using System.Numerics;

namespace GisGkhLibrary.Xades.Helpers
{
    public static class ConvertHelper
    {
        public static string BigIntegerToHex(string str)
        {
            return BigInteger.Parse(str).ToString("X");
        }
    }
}