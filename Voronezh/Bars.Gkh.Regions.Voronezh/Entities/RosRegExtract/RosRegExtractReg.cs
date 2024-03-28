using Bars.B4.DataAccess;
using Bars.Gkh.Entities;

namespace Bars.Gkh.Regions.Voronezh.Entities
{
    public class RosRegExtractReg : PersistentObject
    {
        //Registration
        public virtual string Reg_ID_Record { get; set; }
        public virtual string Reg_RegNumber { get; set; }
        public virtual string Reg_Type { get; set; }
        public virtual string Reg_Name { get; set; }
        public virtual string Reg_RegDate { get; set; }
        public virtual string Reg_ShareText { get; set; }

    }
}
