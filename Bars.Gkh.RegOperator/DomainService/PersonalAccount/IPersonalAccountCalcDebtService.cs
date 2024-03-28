namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount
{
    using Bars.B4;

    /// <summary>
    /// Сервис расчета сальдо для ЧЭС
    /// </summary>
    public interface IPersonalAccountCalcDebtService
    {
        /// <summary>
        /// Расчитать долг
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult CalcDebtTransfer(BaseParams baseParams);

        /// <summary>
        /// Сохранить перенос долга
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult SaveDebtTransfer(BaseParams baseParams);

        /// <summary>
        /// Выгрузить данные
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult Export(BaseParams baseParams);

        /// <summary>
        /// Печать
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult Print(BaseParams baseParams);

        /// <summary>
        /// Получить дату начало первого периода и дату окончания текущего
        /// </summary>
        /// <returns>Результат</returns>
        IDataResult GetPeriodInfo();
    }
}