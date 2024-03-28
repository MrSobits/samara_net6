namespace Bars.Gkh.Modules.ClaimWork.DomainService
{
    using System.Linq;
    using Entities;

    /// <summary>
    /// Интерфейс для работы с исковым заявлением
    /// </summary>
    public interface ILawSuitService
    {
        /// <summary>
        /// Обновить информацию
        /// </summary>
        void UpdateInfo(IQueryable<BaseClaimWork> claimWorkQuery);
    }
}
