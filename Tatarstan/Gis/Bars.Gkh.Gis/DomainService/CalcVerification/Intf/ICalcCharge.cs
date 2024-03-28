namespace Bars.Gkh.Gis.DomainService.CalcVerification.Intf
{
    using B4;

    using Bars.Gkh.Gis.KP_legacy;

    /// <summary>
    /// Расчет начислений 
    /// </summary>
    public interface ICalcCharge
    {
        IDataResult CalcCharge(ref CalcTypes.ParamCalc Params, string TableMOCharges);
    }
}