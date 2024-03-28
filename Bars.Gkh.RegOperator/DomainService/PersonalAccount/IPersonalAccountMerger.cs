namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount
{
    using Bars.B4;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;

    public interface IPersonalAccountMerger
    {
        void Merge(PersonalAccountMergeArgs args);
        
        /// <summary>
        /// Слияние счетов
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Результат закрытия</returns>
        IDataResult MergeAccounts(BaseParams baseParams);
    }
}
