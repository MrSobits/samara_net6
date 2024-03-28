namespace Bars.GkhGji.DomainService.FuelInfo
{
    using Bars.B4;

    /// <summary>
    /// Сервис для работы со сведениями о наличии и расходе топлива
    /// </summary>
    public interface IFuelInfoService
    {
        /// <summary>
        /// Массовое создание периодов
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        IDataResult MassCreate(BaseParams  baseParams);
    }
}