using Bars.B4.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.GkhGji.Regions.Chelyabinsk.Enums
{
    /// <summary>
    /// Тип проверки
    /// </summary>
    public enum ERPInspectionType
    {
        [Display("Тип не указан")]
        NotSet = 0,

        [Display("Плановая")]
        PP = 10,

        [Display("Внеплановая")]
        VP = 20
        
    }
}
