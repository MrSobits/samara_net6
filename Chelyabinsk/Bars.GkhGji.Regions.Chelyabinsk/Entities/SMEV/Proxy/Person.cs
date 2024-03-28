using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bars.GkhGji.Regions.Chelyabinsk.Entities.SMEV.Proxy
{
    [XmlRootAttribute("person", Namespace = "http://rosreestr.ru/services/v0.1/commons/Subjects", IsNullable = false)]
    public class Person
    {
        [XmlAttribute(Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public virtual string Type { get; set; }

        [XmlElement(ElementName = "surname", Order = 1)]
        public virtual string Surname { get; set; }

        [XmlElement(ElementName = "firstname", Order = 2)]
        public String Name { get; set; }

        //[XmlElement(ElementName = "patronymicName", Order = 3)]
        //public String PatronymicName { get; set; }

        [XmlElement(ElementName = "idDocumentRef", Order = 3)]
        public IdDocumentRef IdDocumentRef { get; set; }
    }
}
