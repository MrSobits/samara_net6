namespace Bars.Gkh.Modules.RegOperator.DomainService
{
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;

    using Gkh.Entities;

    /// <summary>
    /// Сервис для работы с региональным оператором
    /// </summary>
    public interface IRegopService
    {
        /// <summary>
        /// Получить текущегго рег. оператора
        /// </summary>
        RegOperator GetCurrentRegOperator();

        /// <summary>
        /// Получить контрагента по идентификатору рег. оператора
        /// </summary>
        /// <param name="regopId">Идентификатор рег. оператора</param>
        Contragent GetRegopContragent(long regopId);
    }
}