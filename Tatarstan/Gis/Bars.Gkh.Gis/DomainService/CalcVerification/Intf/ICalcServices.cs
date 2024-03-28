using Bars.B4;

namespace Bars.Gkh.Gis.DomainService.CalcVerification.Intf
{
    /// <summary>
    /// Расчет расходов по услугам с применением формул расчета
    /// </summary>
    public interface ICalcServices
    {
        IDataResult CalcServices(string TableMOCharges);
    }
}