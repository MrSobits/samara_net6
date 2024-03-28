using Bars.B4.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.GkhGji.Regions.Voronezh.Enums
{
    public enum TypeLicenseRequest
    {
        [Display("Административный штраф")]
        NotSet = 0,

        [Display("Выдача лицензии")]
        First = 10,

        [Display("Переоформление лицензии")]
        Reissuance = 20,

        [Display("Дубликат лицензии")]
        Copy = 30
    }
}
