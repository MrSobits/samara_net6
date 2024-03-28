using Bars.B4.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.GkhGji.Regions.Voronezh.Enums
{
    public enum SMEVLivingPlaceHasReg
    {
        [Display("Регистрация по месту жительства отсутствует")]
        NoRegistration = 0,

        [Display("Зарегистрирован по адресу")]
        AddressReg = 1,

        [Display("Архивные сведения отсутствуют")]
        NoArchieveData = 2,

        [Display("Архивные сведения не внесены в течении 5 дней")]
        NotIncludedArchieve = 3
    }
}
