using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using Bars.B4.Modules.NHibernateChangeLog;
using Bars.B4.Utils;
using Bars.Gkh.Overhaul.Entities;

namespace Bars.Gkh.Overhaul.LogMap
{
    public class RealityObjectStructuralElementAttributeValueLogMap : AuditLogMap<RealityObjectStructuralElementAttributeValue>
    {
	    public RealityObjectStructuralElementAttributeValueLogMap()
        {
			Name("Атрибут конструктивного элемента жилого дома");

			Description(x => x.ReturnSafe(y => string.Format("{0}-{1}", y.Object.RealityObject.Address, y.Object.StructuralElement.Name)));

			MapProperty(x => x.Value, "Value", "Аттрибут");
        }
    }
}
