namespace Bars.GisIntegration.UI.Service
{
    using Bars.B4;

    /// <summary>
    /// Сервис для получения объектов при выполнении импорта/экспорта данных через сервис CapitalRepair
    /// </summary>
    public interface ICapitalRepairService
    {
        /// <summary>
        /// Метод получения списка МО
        /// </summary>
        IDataResult GetMunicipalities(BaseParams baseParams);

        /// <summary>
        /// Получить список планов по кап. ремонту
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult GetCapitalRepairPlan(BaseParams baseParams);
    }
}
