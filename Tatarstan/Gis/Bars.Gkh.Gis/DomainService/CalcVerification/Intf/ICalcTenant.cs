namespace Bars.Gkh.Gis.DomainService.CalcVerification.Intf
{
    using B4;

    using Bars.Gkh.Gis.KP_legacy;

    /// <summary>
    /// Подсчет количества жильцов
    /// </summary>
    public interface ICalcTenant
    {
        IDataResult CalcTenant(ref CalcTypes.ParamCalc Params);
    }
}