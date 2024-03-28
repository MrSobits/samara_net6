namespace Bars.Gkh.RegOperator.DomainService.PartialOperationCancellation
{
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    /// <summary>
    /// Поставщик сервисов, поддерживаемых частичную отмену
    /// </summary>
    public interface ICancellableProviderFactory
    {
        /// <summary>
        /// Вернуть поставщика
        /// </summary>
        /// <param name="operation">Операция для идентификации источника</param>
        /// <returns>Интерфейс объекта, поддерживающего отмену операции</returns>
        ICancellableSourceProvider GetProvider(MoneyOperation operation);
    }
}