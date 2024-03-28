using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.GkhGji.Regions.Voronezh.Enums
{
    public enum QueryTypeType
    {
        /// <remarks/>
        [B4.Utils.Display("Адрес")]
        AddressQuery,

        /// <remarks/>
        [B4.Utils.Display("Кадастровый номер")]
        CadasterNumberQuery,

        /// <remarks/>
        [B4.Utils.Display("Номер реестра")]
        RegisterNumberQuery
    }
}
