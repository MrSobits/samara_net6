using Bars.B4;

namespace Bars.Gkh.Gis.DomainService.CalcVerification.Intf
{
    using Bars.Gkh.Gis.KP_legacy;

    /// <summary>
    /// Получение тарифов на услуги из ЦХД или УК
    /// </summary>
    public interface ITariff
    {
        IDataResult ApplyTariffs(ref CalcTypes.ParamCalc param, string TargetTable);
    }
}