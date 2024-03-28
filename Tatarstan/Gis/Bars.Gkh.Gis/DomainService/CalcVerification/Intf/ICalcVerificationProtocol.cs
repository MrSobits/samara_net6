using Bars.B4;

namespace Bars.Gkh.Gis.DomainService.CalcVerification.Intf
{
    /// <summary>
    /// Получение протокола по начислениям
    /// </summary>
    public interface ICalcVerificationProtocol
    {
        string GetProtocol(BaseParams param);
    }
}