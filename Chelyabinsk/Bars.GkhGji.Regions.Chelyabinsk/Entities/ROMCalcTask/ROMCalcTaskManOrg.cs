using Bars.B4.DataAccess;
using Bars.Gkh.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    public class ROMCalcTaskManOrg : BaseEntity
    {
        public virtual ROMCalcTask ROMCalcTask { get; set; }

        public virtual Contragent Contragent { get; set; }
    }
}
