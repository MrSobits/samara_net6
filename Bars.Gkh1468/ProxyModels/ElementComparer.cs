namespace Bars.Gkh1468.ProxyModels
{
    using System.Collections.Generic;

    public class ElementComparer : IComparer<Element>
    {
        public int Compare(Element x, Element y)
        {
            if (x == null && y == null)
            {
                return 0;
            }

            if (x == null)
            {
                return -1;
            }

            if (y == null)
            {
                return 1;
            }

            return x.Part != null
                ? x.Part.SortNumber.CompareTo(y.Part.SortNumber)
                : x.Attribute.SortNumber.CompareTo(y.Attribute.SortNumber);
        }
    }
}