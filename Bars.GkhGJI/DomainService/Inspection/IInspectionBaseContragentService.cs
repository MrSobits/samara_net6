namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    /// <summary>
    /// Сервис для работы с Органами совместной проверки
    /// </summary>
    public interface IInspectionBaseContragentService
    {
        /// <summary>
        /// Добавить Органы совместной проверки
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения</returns>
        IDataResult AddContragents(BaseParams baseParams);

        /// <summary>
        /// Список категорий рисков контрагента
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult ListContragentRisk(BaseParams baseParams);
    }
}