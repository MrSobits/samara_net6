using Bars.B4.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.Gkh.Regions.Chelyabinsk.Entities
{
    public class RosRegExtractBigOwner : BaseEntity
    {
        public virtual long ExtractId { get; set; }
        public virtual string OwnerName { get; set; }
        public virtual int AreaShareNum { get; set; }
        public virtual int AreaShareDen { get; set; }
        public virtual int RightNumber { get; set; }
    }
}