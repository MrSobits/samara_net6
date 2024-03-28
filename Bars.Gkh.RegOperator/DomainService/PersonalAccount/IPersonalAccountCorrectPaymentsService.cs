namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount
{
    using Bars.B4;

    /// <summary>
    /// Сервис корректировки оплат ЛС
    /// </summary>
    public interface IPersonalAccountCorrectPaymentsService
    {
        /// <summary>
        /// Произвести корректировку оплат
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult MovePayments(BaseParams baseParams);

        /// <summary>
        /// Вернуть данные по оплатам/задолжностям
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult GetAccountPaymentInfo(BaseParams baseParams);
    }
}