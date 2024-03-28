using Bars.B4.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.GkhGji.Regions.Voronezh.Enums
{
    public enum GasuMessageType
    {
        [Display("Добавление сведений")]
        ImportFull = 10,

        [Display("Изменение сведений")]
        ImportDelta = 20,

        [Display("Удаление сведений")]
        Delete = 30
    }
}
