using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bars.GkhOverhaulTp.Regions.Chelyabinsk
{
    class PermissionMap : B4.PermissionMap
    {
        public PermissionMap()
        {
            Permission("Reports.GKH.AreaDataSheetReport", "Отчет по площадям из техпаспорта");
        }
    }
}
