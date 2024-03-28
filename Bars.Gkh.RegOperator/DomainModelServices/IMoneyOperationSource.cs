namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using Bars.Gkh.Entities;

    using Entities.ValueObjects;

    /// <summary>
    /// Источник контекста выполнения операции по передвижению денег.
    /// Например, при подтверждении оплаты по ЛС таким источником будет пакет неподтвержденных оплат
    /// </summary>
    public interface IMoneyOperationSource
    {
        /// <summary>
        /// Создать операцию по передвижению денег  
        /// </summary>
        /// <param name="period">Период операции</param>
        /// <returns><see cref="MoneyOperation"/></returns>
        MoneyOperation CreateOperation(ChargePeriod period);
    }
}