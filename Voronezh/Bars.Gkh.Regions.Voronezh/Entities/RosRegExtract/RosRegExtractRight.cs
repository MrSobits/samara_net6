using Bars.B4.DataAccess;
using Bars.Gkh.Entities;

namespace Bars.Gkh.Regions.Voronezh.Entities
{
    public class RosRegExtractRight : PersistentObject
    {
        public virtual RosRegExtractReg reg_id { get; set; }
        public virtual RosRegExtractOwner owner_id { get; set; }
        public virtual string RightNumber { get; set; }

    }
}
