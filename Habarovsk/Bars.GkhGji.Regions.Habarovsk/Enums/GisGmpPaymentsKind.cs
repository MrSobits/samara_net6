﻿using Bars.B4.Utils;

namespace Bars.GkhGji.Regions.Habarovsk.Enums
{
    public enum GisGmpPaymentsKind
    {
        //[Display("Запрос неоплаченных начислений")]
        //CHARGE = 10,

        //[Display("Запроса начислений не полностью сквитированных с платежами")]
        //CHARGENOTFULLMATCHED = 20,

        //[Display("Запрос начислений и статусов их квитирования")]
        //CHARGESTATUS = 30,

        //[Display("Запрос неоплаченных предварительных начислений")]
        //CHARGE_PRIOR = 40,

        //[Display("Запрос предварительных начислений и статусов их квитирования")]
        //CHARGE_PRIOR_STATUS = 50,

        //[Display("Запрос неоплаченных предварительных начислений, сформированных ГИС ГМП")]
        //TEMP_CHARGING = 60,

        //[Display("Запрос предварительных начислений, сформированных ГИС ГМП, не полностью сквитированных с платежами")]
        //TEMP_CHARGING_NOTFULLMATCHED = 70,

        //[Display("Запрос предварительных начислений, сформированных ГИС ГМП, и статусов их квитирования")]
        //TEMP_CHARGING_STATUS = 80,

        [Display("Все активные (неаннулированные) платежи")]
        PAYMENT = 90,

        [Display("Все платежи, имеющие статус уточнения или статус аннулирования")]
        PAYMENTMODIFIED = 100,

        [Display("Все активные (неаннулированные) платежи, для которых в системе отсутствуют соответствующие начисления (не создана ни одна квитанция)")]
        PAYMENTUNMATCHED = 110,

        [Display("Аннулированные платежи ")]
        PAYMENTCANCELLED = 120,

        [Display("Зпрос результатов квитирования, за исключением неактивных (возвращается результат квитирования с последним полученным платежом)")]
        QUITTANCE = 130,

        [Display("Запрос всех результатов квитирования")]
        ALLQUITTANCE = 140
    }
}
