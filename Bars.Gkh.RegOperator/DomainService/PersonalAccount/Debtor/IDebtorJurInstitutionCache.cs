namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Debtor
{
    using System.Linq;

    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    /// <summary>
    /// Кэш для заполнения поля Учреждение в судебной практике 
    /// </summary>
    public interface IDebtorJurInstitutionCache
    {
        /// <summary>
        /// Инициализировать кэш
        /// </summary>
        /// <param name="roIds">Id домов</param>
        void InitCache(long[] roIds);

        /// <summary>
        /// Инициализировать кэш
        /// </summary>
        /// <param name="roIdQuery">Id домов</param>
        void InitCache(IQueryable<long> roIdQuery);

        /// <summary>
        /// Установить в должника учреждение в судебной практике
        /// </summary>
        /// <param name="debtor">Должник</param>
        /// <param name="account">Лс</param>
        /// <returns>Изменилось ли учреждение</returns>
        bool SetJurInstitution(Debtor debtor, DebtorCalcService.BasePersonalAccountDto account);

        /// <summary>
        /// Установить в должника учреждение в судебной практике
        /// </summary>
        /// <param name="debtor">Должник</param>
        /// <param name="account">Лс</param>
        /// <returns>Изменилось ли учреждение</returns>
        bool SetJurInstitution(Debtor debtor, BasePersonalAccount account);
    }
}