namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс сервиса перераспределения оплат
    /// </summary>
    public interface IPersonalAccountRepaymentService
    {
        /// <summary>
        /// Выполнить перераспределение средств
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult Execute(BaseParams baseParams);

        /// <summary>
        /// Вернуть список оплат
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult GetDataForUI(BaseParams baseParams);
    }
}