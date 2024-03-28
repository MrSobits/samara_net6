using System.Collections.Generic;

namespace Bars.Gkh1468.ProxyModels
{
    using Bars.Gkh1468.Entities;

    public class Element
    {
        public Element()
        {
            Childrens = new List<Element>();
        }

        public Part Part { get; set; }

        public MetaAttribute Attribute { get; set; }

        public List<Element> Childrens { get; set; }
    }
}
