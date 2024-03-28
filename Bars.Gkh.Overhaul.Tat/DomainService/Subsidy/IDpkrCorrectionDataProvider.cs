namespace Bars.Gkh.Overhaul.Tat.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Entities;

    /// <summary>
    /// Интерфейс для получения данных корректировки субсидирования
    /// </summary>
    public interface IDpkrCorrectionDataProvider
    {
        IQueryable<DpkrCorrectionStage2> GetCorrectionData(BaseParams baseParams);
    }
}