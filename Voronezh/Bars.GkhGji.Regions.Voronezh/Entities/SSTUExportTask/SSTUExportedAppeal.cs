using Bars.B4.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bars.GkhGji.Entities;

namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    public class SSTUExportedAppeal : BaseEntity
    {

        public virtual AppealCits AppealCits { get; set; }

        public virtual DateTime? ExportDate { get; set; }
    }
}
