namespace Bars.Gkh.RegOperator.Domain.Repository.PersonalAccounts
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    public interface IPersonalAccountRepository
    {

        /// <summary>
        /// Возвращает лицевые счета, с кошельков которых были сняты деньги при операции,
        /// переданной в качестве аргумента.
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        IQueryable<BasePersonalAccount> GetParticipatedAsSourceIn(MoneyOperation operation);

        /// <summary>
        /// Возвращает лицевые счета, на кошельки которых были переведены деньги при операции,
        /// переданной в качестве аргумента.
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        IQueryable<BasePersonalAccount> GetParticipatedAsTargetIn(MoneyOperation operation);

        /// <summary>
        /// Получить ЛС, для обновления
        /// </summary>
        /// <param name="accountsIds">Фильтр по идентификаторам ЛС</param>
        /// <returns></returns>
        IQueryable<BasePersonalAccount> GetForBalanceUpdateInOpenedPeriod(IEnumerable<long> accountsIds);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IQueryable<BasePersonalAccount> GetByIds(long[] ids);
    }
}
