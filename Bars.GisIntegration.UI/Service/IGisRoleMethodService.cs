namespace Bars.GisIntegration.UI.Service
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс сервиса GisRoleMethod
    /// </summary>
    public interface IGisRoleMethodService
    {
        /// <summary>
        /// Добавить методы интеграции для роли ГИС
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult AddRoleMetods(BaseParams baseParams);
    }
}
