using Bars.B4.DataAccess;
using Bars.Gkh.Entities;
using Bars.Gkh.Regions.Voronezh.Schema;

namespace Bars.Gkh.Regions.Voronezh.Entities
{
    public class RosRegExtractOwner : PersistentObject
    {
        public virtual RosRegExtractGov gov_id { get; set; }
        public virtual RosRegExtractOrg org_id { get; set; }
        public virtual RosRegExtractPers pers_id { get; set; }

    }
}
