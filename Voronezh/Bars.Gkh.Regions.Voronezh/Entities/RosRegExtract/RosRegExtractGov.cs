using Bars.B4.DataAccess;
using Bars.Gkh.Entities;

namespace Bars.Gkh.Regions.Voronezh.Entities
{
    public class RosRegExtractGov : PersistentObject
    {
        //Governance
        public virtual string Gov_Code_SP { get; set; }
        public virtual string Gov_Content { get; set; }
        public virtual string Gov_Name { get; set; }
        public virtual string Gov_OKATO_Code { get; set; }
        public virtual string Gov_Country { get; set; }
        public virtual string Gov_Address { get; set; }
    }
}
