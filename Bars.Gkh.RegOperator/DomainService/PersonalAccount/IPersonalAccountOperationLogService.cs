namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount
{
    using B4;

    /// <summary>
    /// Интерфейс для сервиса истории изменения лс
    /// </summary>
    public interface IPersonalAccountOperationLogService
    {
        /// <summary>
        /// Получить изменения лс
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult GetOperationLog(BaseParams baseParams);
    }
}