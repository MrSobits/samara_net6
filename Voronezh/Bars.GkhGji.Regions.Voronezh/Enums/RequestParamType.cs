using Bars.B4.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.GkhGji.Regions.Voronezh.Enums
{
    public enum RequestParamType
    {
        [Display("Номер разрешения")]
        PermitNumber = 10,

        [Display("Кадастровый номер земельного участка")]
        CadastralNumberZU = 20,

        [Display("Адрес")]
        Address = 30
    }
}
