namespace Bars.Gkh1468.Domain.ProviderPassport.Serialize
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using Bars.Gkh1468.Entities;

    public class PassportElement
    {
        public PassportElement()
        {
            Elements = new List<PassportElement>();
        }

        public virtual PassportElementType Type { get; set; }

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public string Value { get; set; }

        [XmlIgnore]
        public Part Part { get; set; }

        [XmlIgnore]
        public MetaAttribute Attribute { get; set; }

        public List<PassportElement> Elements { get; set; }
    }

    public class PassportElementComparer : IComparer<PassportElement>
    {
        public int Compare(PassportElement x, PassportElement y)
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

            int i = System.String.Compare(x.Code, y.Code, System.StringComparison.Ordinal);

            if (i == 0)
            {
                return 0;
            }
            if (i < 0)
            {
                return -1;
            }
            if (i > 0)
            {
                return 1;
            }
            return 0;
        }
    }
}