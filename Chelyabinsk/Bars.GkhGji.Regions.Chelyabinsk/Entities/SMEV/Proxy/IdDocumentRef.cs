using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bars.GkhGji.Regions.Chelyabinsk.Entities.SMEV.Proxy
{
    public class IdDocumentRef
    {
        [XmlAttribute(AttributeName = "documentId")]
        public virtual Guid DocumentId { get; set; }
    }
}
