using Bars.B4.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.GkhGji.Regions.Chelyabinsk.Enums
{
    public enum PayerType
    {
        [Display("Индивидуальный предприниматель")]
        IP = 10,

        [Display("Физическое лицо")]
        Physical = 20,

        [Display("Юридическое лицо")]
        Juridical = 30
    }

   
}
