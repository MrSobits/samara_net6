namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount
{
    using Bars.B4;

    /// <summary>
    /// Сервис корректировки оплат ЛС
    /// </summary>
    public interface IPersonalAccountCancelChargeService
    {
        /// <summary>
        /// Произвести отмену начислений
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult CancelCharges(BaseParams baseParams);

        /// <summary>
        /// Получение данных на клиентскую часть
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        IDataResult GetDataForUI(BaseParams @params);
    }
}
