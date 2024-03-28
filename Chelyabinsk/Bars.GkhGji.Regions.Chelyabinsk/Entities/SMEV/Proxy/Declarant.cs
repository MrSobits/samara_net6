using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bars.GkhGji.Regions.Chelyabinsk.Entities.SMEV.Proxy
{
    [XmlRootAttribute("declarant", Namespace = "http://rosreestr.ru/services/v0.18/TStatementRequestEGRN", IsNullable = false)]
    public class Declarant
    {
        [XmlAttribute(AttributeName = "_id")]
        public virtual Guid Id { get; set; }

        [XmlElement(ElementName = "person", Order = 0)]
        public virtual Person Person { get; set; }

        [XmlElement(ElementName = "declarantKind", Order = 1, Namespace = "http://rosreestr.ru/services/v0.1/commons/Subjects")]
        public virtual string DeclarantKind { get; set; }
    }
}
