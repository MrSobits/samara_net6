using Bars.B4;

namespace Bars.Gkh.Gis.DomainService.CalcVerification.Intf
{
    /// <summary>
    /// Получение нормативов из ЦХД или УК
    /// </summary>
    public interface INormative
    {
        IDataResult ApplyNormatives(string TargetTable);
    }
}