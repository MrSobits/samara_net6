namespace Bars.Gkh.RegOperator.DomainService.Import.Ches
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities.Import.Ches;

    /// <summary>
    /// Интерфейс сопоставления абонентов
    /// </summary>
    public interface IChesAccountOwnerComparingService
    {
        /// <summary>
        /// Сопоставить абонентов автоматически
        /// </summary>
        /// <param name="accountOwners">Подзапрос абонентов</param>
        /// <returns>Сопоставленные данные</returns>
        IDataResult<IEnumerable<ChesMatchAccountOwner>> MatchAutomatically(IQueryable<ChesNotMatchAccountOwner> accountOwners);
    }
}