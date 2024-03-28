namespace Bars.Gkh.Utils
{
    using System.Collections.Generic;

    public class NumericComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return StringLogicalComparer.Compare(x, y);
        }
    }
}