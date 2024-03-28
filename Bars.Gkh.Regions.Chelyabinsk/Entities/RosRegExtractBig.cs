using Bars.B4.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.Gkh.Regions.Chelyabinsk.Entities
{
    public class RosRegExtractBig : BaseEntity
    {
        //public virtual string XML { get; set; }

        public virtual string CadastralNumber { get; set; }
        public virtual string Address { get; set; }
        public virtual string ExtractDate { get; set; }
        public virtual string ExtractNumber { get; set; }
        public virtual string RoomArea { get; set; }
    }
}