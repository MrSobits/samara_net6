namespace Bars.Gkh.RegOperator.Modules.ClaimWork.Repository
{
    using System.Linq;

    using Bars.Gkh.RegOperator.Entities;

    public interface IDebtorClaimworkRepository
    {
        /// <summary>
        /// Получить основание ПИР для неплательщиков по абоненту
        /// </summary>
        /// <param name="ownerId">id абонента</param>
        /// <returns>Основание ПИР</returns>
        IQueryable<DebtorClaimWork> GetByOwnerId(long ownerId);
    }
}